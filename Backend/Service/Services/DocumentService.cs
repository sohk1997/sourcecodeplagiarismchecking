using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Root.Data.Infrastructure;
using Root.Data.Repository;
using Root.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Document;

namespace Service.Services
{
    public interface IDocumentService
    {
        List<DocumentInList> DocumentProcedure(int Id, string Name);
        DocumentInfo Get(int Id);
        ResponseResult GetResult(int id);
        Task<int> UploadToCloud(IFormFile file, bool webcheck, bool peercheck, int userId);
        List<DocumentInList> GetAll();
        List<DocumentInList> GetAll(int userId);
    }
    public class DocumentService : IDocumentService
    {
        //private string baseUriOfBlob = "https://sourceproject.blob.core.windows.net/";        
        private ISourceCodeRepository _sourceCodeRepository;
        private readonly IResultRepository _resultRepository;
        private readonly IMethodRepository _methodRepository;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IStoreProcedureRepository _storeProcedure;

        public DocumentService(ISourceCodeRepository documentRepository, IUnitOfWork unitOfWork, IMapper mapper,
            IResultRepository resultRepository,
            IMethodRepository methodRepository,
            IStoreProcedureRepository storeProcedure)
        {
            _sourceCodeRepository = documentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storeProcedure = storeProcedure;
            _resultRepository = resultRepository;
            _methodRepository = methodRepository;
        }

        public async Task<int> UploadToCloud(IFormFile file, bool webcheck, bool peercheck, int userId)
        {
            try
            {
                Submission document = _mapper.Map<Submission>(file);
                document.DocumentName = file.FileName;
                document.Status = Root.CommonEnum.SourceCodeStatus.PENDING;
                document.Type = Root.CommonEnum.SourceCodeType.PEER;
                document.UploadDate = DateTime.Now;
                document.UserId = userId;

                if (webcheck && peercheck)
                {
                    document.CheckType = CheckType.BOTH;
                }
                else if (webcheck)
                {
                    document.CheckType = CheckType.WEB_CHECK;
                }
                else if (peercheck)
                {
                    document.CheckType = CheckType.PEER_CHECK;
                }
                //----Upload - To - Azure - Blob----
                CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials
                    ("sourceproject", "+03nTRvnSMevowugQfMm5BU7mGCTrs3VDa9SkzNP+qVl7aaVHO6imOnqKMLPwK2fQrsfg3f5CWlUihgvzSu3lA=="), true);
                // Create a blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                // Get a reference to a container named "project."
                CloudBlobContainer container = blobClient.GetContainerReference("project");
                // Get a reference to a blob named "file.FileName".
                Guid guid = Guid.NewGuid();
                string genKeyName = guid.ToString() + Path.GetExtension(file.FileName).ToLower();
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(genKeyName);

                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var fileContent = reader.ReadToEnd();
                    //var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);                                
                    var content = Encoding.ASCII.GetBytes(fileContent);
                    await blockBlob.UploadFromByteArrayAsync(content, 0, content.Length);
                    document.FileUrl = blockBlob.Uri.AbsoluteUri;
                }
                _sourceCodeRepository.Add(document);
                Commit();

                Guid queryId = document.Id;
                document = _sourceCodeRepository.GetAllQueryable().FirstOrDefault(d => d.Id == queryId);
                var jsonObject = new
                {
                    id = document.Id,
                    webCheck = webcheck,
                    peerCheck = peercheck
                };
                RabbitMQHelper.SendMessage(JsonConvert.SerializeObject(jsonObject));
                return document.DocumentId;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.StackTrace);
                throw ex;
            }
        }

        public List<DocumentInList> DocumentProcedure(int Id, string Name)
        {

            return Mapper.Map<List<Submission>, List<DocumentInList>>(_sourceCodeRepository.GetAllQueryable().ToList());
        }

        public List<DocumentInList> GetAll()
        {
            var list = _sourceCodeRepository.GetAllQueryable().Where(d => d.Type != Root.CommonEnum.SourceCodeType.WEB)
            .OrderByDescending(d => d.UploadDate)
            .Select(d => new
            {
                Id = d.DocumentId,
                Name = d.DocumentName,
                Status = (int)d.Status,
                UploadDate = d.UploadDate
            })
            .ToList()
            .Select(d => new DocumentInList
            {
                Id = d.Id,
                Name = d.Name,
                Status = (int)d.Status,
                UploadDate = d.UploadDate == null ? "" : d.UploadDate.Value.ToString("dd-MM-yyyy hh:MM")
            });
            return list.ToList();
        }

        public List<DocumentInList> GetAll(int userId)
        {
            var list = _sourceCodeRepository.GetAllQueryable().Where(d => d.Type != Root.CommonEnum.SourceCodeType.WEB && d.UserId == userId)
            .OrderByDescending(d => d.UploadDate)
            .Select(d => new
            {
                Id = d.DocumentId,
                Name = d.DocumentName,
                Status = (int)d.Status,
                UploadDate = d.UploadDate
            })
            .ToList()
            .Select(d => new DocumentInList
            {
                Id = d.Id,
                Name = d.Name,
                Status = (int)d.Status,
                UploadDate = d.UploadDate == null ? "" : d.UploadDate.Value.ToString("dd-MM-yyyy hh:MM")
            });
            return list.ToList();
        }

        public DocumentInfo Get(int Id)
        {
            var document = _sourceCodeRepository.Get(x => x.DocumentId == Id);
            return _mapper.Map<DocumentInfo>(document);
        }

        private void Commit()
        {
            _unitOfWork.Commit();
        }

        public ResponseResult GetResult(int id)
        {
            var result = new ResponseResult();
            var document = _sourceCodeRepository.GetAllQueryable().FirstOrDefault(r => r.DocumentId == id);

            if (document.CheckType == CheckType.PEER_CHECK
               || document.CheckType == CheckType.BOTH)
            {
                result.PeerCheckResult = GetPeerResult(id);
            }
            if (document.CheckType == CheckType.WEB_CHECK
               || document.CheckType == CheckType.BOTH)
            {
                result.WebCheckResult = GetWebResult(id);
            }
            return result;

        }

        public DocumentResult GetWebResult(int id)
        {
            var query = (from m in _methodRepository.GetAllQueryable().Where(m => m.SourceCodeId == id)
                         join rr in
                         (from r in _resultRepository.GetAllQueryable()
                         join sm in _methodRepository.GetAllQueryable() on r.SimMethodId equals sm.Id into rmm
                         from sm in rmm
                         join so in _sourceCodeRepository.GetAllQueryable().Where(s => s.Type == Root.CommonEnum.SourceCodeType.WEB) on sm.SourceCodeId equals so.DocumentId into som
                         from so in som
                         select new { Result = r, SimMethod = sm, Type = so == null ? 0 : so.Type, Url = so == null ? "" : so.DocumentName }
                         ) on m.Id equals rr.Result.BaseMethodId into mrr
                         from rr in mrr.DefaultIfEmpty()
                         select new { Result = rr.Result, Method = m, SimMethod = rr.SimMethod, Type = rr == null ? 0 : rr.Type, Url = rr == null ? "" : rr.Url }).OrderBy(r => r.Method.Id).ToList();
            var document = _sourceCodeRepository.GetAllQueryable().FirstOrDefault(r => r.DocumentId == id);
            List<DocumentResultDetail> details = new List<DocumentResultDetail>();
            string url = "";
            foreach (var m in query)
            {
                DocumentResultDetail item = new DocumentResultDetail
                {
                    MethodName = m.Method.MethodName,
                    BaseMethod = m.Method.MethodString,
                };
                if (m.Result != null)
                {
                    if (m.Type == Root.CommonEnum.SourceCodeType.WEB)
                    {
                        url = m.Url;
                        item.Url = m.Url;
                        item.SimMethod = m.SimMethod.MethodString;
                        item.Position = JsonConvert.DeserializeObject<SimilarityPositions>(m.Result.ResultDetail);
                        item.SimRatio = m.Result.SimRatio;
                        details.Add(item);
                    }

                }
                else
                {
                    item.SimMethod = null;
                    item.Position = null;
                    item.SimRatio = 0;
                    details.Add(item);
                }
            }

            details.ForEach(d => d.Url = url);

            var result = new DocumentResult()
            {
                FileName = document.DocumentName,
                GeneralSimRatio = details.Sum(d => d.SimRatio) / details.Count,
                Details = details
            };
            if (result.Details.Count > 0 && result.GeneralSimRatio > 0)
            {
                return result;
            }
            else
            {
                return null;
            }
        }
        public DocumentResult GetPeerResult(int id)
        {
            var query = (from m in _methodRepository.GetAllQueryable().Where(m => m.SourceCodeId == id)
                         join rr in
                         (from r in _resultRepository.GetAllQueryable()
                          join sm in _methodRepository.GetAllQueryable() on r.SimMethodId equals sm.Id into rmm
                          from sm in rmm
                          join so in _sourceCodeRepository.GetAllQueryable().Where(s => s.Type == Root.CommonEnum.SourceCodeType.PEER) on sm.SourceCodeId equals so.DocumentId into som
                          from so in som
                          select new { Result = r, SimMethod = sm, Type = so == null ? 0 : so.Type, Url = so == null ? "" : so.DocumentName }
                         ) on m.Id equals rr.Result.BaseMethodId into mrr
                         from rr in mrr.DefaultIfEmpty()
                         select new { Result = rr.Result, Method = m, SimMethod = rr.SimMethod, Type = rr == null ? 0 : rr.Type, Url = rr == null ? "" : rr.Url }).OrderBy(r => r.Method.Id).ToList();
            var document = _sourceCodeRepository.GetAllQueryable().FirstOrDefault(r => r.DocumentId == id);
            List<DocumentResultDetail> details = new List<DocumentResultDetail>();

            foreach (var m in query)
            {
                DocumentResultDetail item = new DocumentResultDetail
                {
                    Id = m.Method.Id,
                    MethodName = m.Method.MethodName,
                    BaseMethod = m.Method.MethodString,
                };
                if (m.Result != null)
                {
                    if (m.Type == Root.CommonEnum.SourceCodeType.PEER)
                    {

                        item.SimMethod = m.SimMethod.MethodString;
                        item.Position = JsonConvert.DeserializeObject<SimilarityPositions>(m.Result.ResultDetail);
                        item.SimRatio = m.Result.SimRatio;
                        details.Add(item);
                    }

                }
                else
                {
                    item.SimMethod = null;
                    item.Position = null;
                    item.SimRatio = 0;
                    details.Add(item);

                }
            }
            var result = new DocumentResult()
            {
                FileName = document.DocumentName,
                GeneralSimRatio = details.Sum(d => d.SimRatio) / details.Count,
                Details = details
            };

            if (result.Details.Count > 0 && result.GeneralSimRatio > 0)
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}

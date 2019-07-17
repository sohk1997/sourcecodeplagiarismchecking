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
        DocumentResult GetResult(int id);
        Task<int> UploadToCloud(IFormFile file, bool webcheck, bool peercheck);
        List<DocumentInList> GetAll();
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

        public async Task<int> UploadToCloud(IFormFile file, bool webcheck, bool peercheck)
        {
            SourceCode document = _mapper.Map<SourceCode>(file);
            document.DocumentName = file.FileName;
            document.Status = Root.CommonEnum.SourceCodeStatus.PENDING;
            document.Type = Root.CommonEnum.SourceCodeType.PEER;

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

        public List<DocumentInList> DocumentProcedure(int Id, string Name)
        {

            return Mapper.Map<List<SourceCode>, List<DocumentInList>>(_sourceCodeRepository.GetAllQueryable().ToList());
        }

        public List<DocumentInList> GetAll()
        {
            var list = _sourceCodeRepository.GetAllQueryable().Where(d => d.Type != Root.CommonEnum.SourceCodeType.WEB).Select(d => new DocumentInList {
                Id = d.DocumentId,
                Name = d.DocumentName,
                Status = (int)d.Status
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

        public DocumentResult GetResult(int id)
        {
            var query = from m in _methodRepository.GetAllQueryable().Where(m => m.SourceCodeId == id)
                        join r in _resultRepository.GetAllQueryable() on m.Id equals r.BaseMethodId into rm
                        from r in rm.DefaultIfEmpty()
                        join sm in _methodRepository.GetAllQueryable() on r.SimMethodId equals sm.Id into rmm
                        from sm in rmm
                        select new { Result = r, Method = m, SimMethod = sm};
            var document = _sourceCodeRepository.GetAllQueryable().FirstOrDefault(r => r.DocumentId == id);
            List<DocumentResultDetail> details = new List<DocumentResultDetail>();
            foreach(var m in query)
            {
                DocumentResultDetail item = new DocumentResultDetail
                {
                    MethodName = m.Method.MethodName,
                    BaseMethod = m.Method.MethodString,
                };
                if(m.Result != null)
                {
                    item.SimMethod = m.SimMethod.MethodString;
                    item.Position = JsonConvert.DeserializeObject<SimilarityPositions>(m.Result.ResultDetail.Replace("'", "\""));
                    item.SimRatio = m.Result.SimRatio;
                }
                else
                {
                    item.SimMethod = null;
                    item.Position = null;
                    item.SimRatio = 0;
                }
                details.Add(item);
            }
            var result = new DocumentResult()
            {
                FileName = document.DocumentName,
                GeneralSimRatio = details.Sum(d => d.SimRatio) / details.Count,
                Details = details
            };

            return result;
        }
    }
}

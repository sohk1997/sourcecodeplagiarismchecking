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
        Task<int> UploadToCloud(IFormFile file);
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

        public async Task<int> UploadToCloud(IFormFile file)
        {
            SourceCode document = _mapper.Map<SourceCode>(file);
            document.DocumentName = file.FileName;

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

            RabbitMQHelper.SendMessage(document.Id.ToString());
            return document.DocumentId;
        }

        public List<DocumentInList> DocumentProcedure(int Id, string Name)
        {
            return Mapper.Map<List<SourceCode>, List<DocumentInList>>(_sourceCodeRepository.GetAllQueryable().ToList());
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
            var query = from r in _resultRepository.GetAllQueryable()
                        join m in _methodRepository.GetAllQueryable().Where(m => m.SourceCodeId == id) on r.BaseMethodId equals m.Id into rm
                        from m in rm
                        join sm in _methodRepository.GetAllQueryable() on r.SimMethodId equals sm.Id into rmm
                        from sm in rmm
                        select new { Result = r, Method = m, SimMethod = sm};
            var document = _sourceCodeRepository.GetAllQueryable().FirstOrDefault(r => r.DocumentId == id);
            List<DocumentResultDetail> details = new List<DocumentResultDetail>();
            Console.WriteLine(query.ToList());
            foreach(var m in query)
            {
                Console.WriteLine(m.Result.BaseMethodId);
                Console.WriteLine(m.SimMethod.Id);
                Console.WriteLine(m.Method);
                DocumentResultDetail item = new DocumentResultDetail
                {
                    BaseMethod = m.Method.MethodString,
                    SimMethod = m.SimMethod.MethodString,
                    Position = JsonConvert.DeserializeObject<SimilarityPositions>(m.Result.ResultDetail.Replace("'", "\"")),
                    SimRatio = m.Result.SimRatio
                };
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

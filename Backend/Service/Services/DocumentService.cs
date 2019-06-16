using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
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
        Task<int> UploadToCloud(IFormFile file);
    }
    public class DocumentService : IDocumentService
    {
        //private string baseUriOfBlob = "https://sourceproject.blob.core.windows.net/";        
        private IDocumentRepository _documentRepository;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IStoreProcedureRepository _storeProcedure;

        public DocumentService(IDocumentRepository documentRepository, IUnitOfWork unitOfWork, IMapper mapper,
            IStoreProcedureRepository storeProcedure)
        {
            _documentRepository = documentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storeProcedure = storeProcedure;
        }

        public async Task<int> UploadToCloud(IFormFile file)
        {
            Document document = _mapper.Map<Document>(file);
            document.DocumentExtn = (Path.GetExtension(file.FileName).ToLower() == ".java") ? "java" :
                (Path.GetExtension(file.FileName).ToLower() == ".cs") ? "cs" : "Undefine";
            document.DocumentName = file.FileName;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var fileContent = reader.ReadToEnd();
                //var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);                                
                document.DocumentContent = Encoding.ASCII.GetBytes(fileContent);
            }
            _documentRepository.Add(document);
            Commit();
            //----Upload - To - Azure - Blob----
            CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials
                ("sourceproject", "+03nTRvnSMevowugQfMm5BU7mGCTrs3VDa9SkzNP+qVl7aaVHO6imOnqKMLPwK2fQrsfg3f5CWlUihgvzSu3lA=="), true);
            // Create a blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Get a reference to a container named "project."
            CloudBlobContainer container = blobClient.GetContainerReference("project");
            // Get a reference to a blob named "file.FileName".
            Guid guid = Guid.NewGuid();
            string genKeyName = file.FileName + "-" + guid.ToString();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(genKeyName);
            await blockBlob.UploadFromByteArrayAsync(document.DocumentContent, 0, document.DocumentContent.Length);
            return document.DocumentId;
        }

        public List<DocumentInList> DocumentProcedure(int Id, string Name)
        {
            return Mapper.Map<List<Document>, List<DocumentInList>>(_documentRepository.GetAllQueryable().ToList());
        }

        public DocumentInfo Get(int Id)
        {
            var document = _documentRepository.Get(x => x.DocumentId == Id);
            return _mapper.Map<DocumentInfo>(document);
        }

        private void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}

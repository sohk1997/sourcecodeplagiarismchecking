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
using System.Text;
using System.Threading.Tasks;
using ViewModel.Document;

namespace Service.Services
{
    public interface IDocumentService
    {
        List<DocumentInList> DocumentProcedure(int Id, string Name);
        DocumentInfo Get(int Id);
        Task<int> UploadToCloud(IFormFile file, string filePath);
        void Update(IFormFile file);
        void Delete(int Id);
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

        public async Task<int> UploadToCloud(IFormFile file, string filePath)
        {
            Document document = _mapper.Map<Document>(file);
            document.DocumentExtn = (Path.GetExtension(file.FileName).ToLower() == ".java") ? "java" :
                (Path.GetExtension(file.FileName).ToLower() == ".cs") ? "cs" : "Undefine";
            document.DocumentName = file.FileName;
            _documentRepository.AddToUpload(document, filePath); // read stream to byte[]
            _documentRepository.Add(document);
            Commit();
            //----Upload - To - Azure - Blo----
            CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials
                ("sourceproject", "+03nTRvnSMevowugQfMm5BU7mGCTrs3VDa9SkzNP+qVl7aaVHO6imOnqKMLPwK2fQrsfg3f5CWlUihgvzSu3lA=="), true);
            // Create a blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Get a reference to a container named "mycontainer."
            CloudBlobContainer container = blobClient.GetContainerReference("project");
            // Get a reference to a blob named "myblob".            
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(file.FileName);
            //await blockBlob.UploadFromByteArrayAsync(document.DocumentContent, 0, document.DocumentContent.Length);
            return document.DocumentId;
        }

        public void Delete(int Id)
        {
            _documentRepository.Delete(x => x.DocumentId == Id);
            Commit();
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

        public void Update(IFormFile file)
        {
            //var document = _documentRepository.Get(c => c.DocumentId == file.Id);
            //document = _mapper.Map<IFormFile, Document>(file, document);
            //_documentRepository.Update(document);
            //Commit();
        }

        private void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}

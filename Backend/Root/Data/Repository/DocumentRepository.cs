using Microsoft.AspNetCore.Http;
using Root.Data.Infrastructure;
using Root.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Root.Data.Repository
{
    public interface IDocumentRepository : IRepository<Document>
    {
        void AddToUpload(Document document, string filePath);
    }

    public class DocumentRepository : Repository<Document>, IDocumentRepository
    {
        public DocumentRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public async void AddToUpload(Document document, string filePath)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open))
                {
                    document.DocumentContent = new byte[fileStream.Length];
                    byte[] bytes = new byte[fileStream.Length];
                    int numBytesToRead = (int)fileStream.Length;
                    int numBytesRead = 0;
                    while (numBytesToRead > 0)
                    {
                        // Read may return anything from 0 to numBytesToRead.
                        int n = fileStream.Read(document.DocumentContent, numBytesRead, numBytesToRead);

                        // Break when the end of the file is reached.
                        if (n == 0)
                            break;

                        numBytesRead += n;
                        numBytesToRead -= n;
                    }
                    numBytesToRead = document.DocumentContent.Length;
                }

                //foreach (var item in filesTemp)
                //{
                //    // Delete temporary files
                //}
            }
            catch (Exception ex)
            {
                throw new Exception("Error message.", ex);
            }
        }
    }
}

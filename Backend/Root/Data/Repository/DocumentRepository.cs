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
    }

    public class DocumentRepository : Repository<Document>, IDocumentRepository
    {
        public DocumentRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}

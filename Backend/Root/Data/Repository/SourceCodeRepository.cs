using Microsoft.AspNetCore.Http;
using Root.Data.Infrastructure;
using Root.Model;
using Root.Model.Map;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Root.Data.Repository
{
    public interface ISourceCodeRepository : IRepository<Submission>
    {
    }

    public class SourceCodeRepository : Repository<Submission>, ISourceCodeRepository
    {
        public SourceCodeRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }



}

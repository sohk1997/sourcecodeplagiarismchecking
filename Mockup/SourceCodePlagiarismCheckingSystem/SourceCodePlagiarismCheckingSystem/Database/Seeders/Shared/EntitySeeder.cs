using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SourceCodePlagiarismCheckingSystem.Database
{
    public abstract class EntitySeeder
    {
        public abstract void Seed(AppDbContext appDbContext);
    }
}
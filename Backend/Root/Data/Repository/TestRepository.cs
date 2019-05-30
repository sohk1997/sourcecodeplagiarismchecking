using Root.Data.Infrastructure;
using Root.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Root.Data.Repository
{
    public interface ITestRepository : IRepository<TestModel>
    {

    }

    public class TestRepository : Repository<TestModel>, ITestRepository
    {
        public TestRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}

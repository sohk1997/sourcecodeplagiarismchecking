using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Root.Data.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private TestContext _dataContext;
        private IConfiguration _configuration;
        public DatabaseFactory(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public TestContext Get()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestContext>();
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("TestDB"));
            return _dataContext ?? (_dataContext = new TestContext(optionsBuilder.Options));
        }

        public TestContext GetNew(){
            var optionsBuilder = new DbContextOptionsBuilder<TestContext>();
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("TestDB"));
            return new TestContext(optionsBuilder.Options);
        }
    }
}

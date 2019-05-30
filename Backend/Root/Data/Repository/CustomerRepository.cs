using Root.Data.Infrastructure;
using Root.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Root.Data.Repository
{
    public interface ICustomerRepository : IRepository<Customer>
    {

    }

    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}

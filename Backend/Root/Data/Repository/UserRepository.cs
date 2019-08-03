using Root.Data.Infrastructure;
using Root.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Root.Data.Repository
{
    public interface IUserRepository : IRepository<User>
    {

    }

    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}

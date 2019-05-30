using Root.Data.Infrastructure;
using Root.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Root.Data.Repository
{
    public interface IRoleFeatureRepository : IRepository<RoleFeature>
    {

    }

    public class RoleFeatureRepository : Repository<RoleFeature>, IRoleFeatureRepository
    {
        public RoleFeatureRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}

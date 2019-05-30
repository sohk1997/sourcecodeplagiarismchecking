using Root.Data.Infrastructure;
using Root.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Root.Data.Repository
{
    public interface IFeatureRepository : IRepository<Feature>
    {

    }

    public class FeatureRepository : Repository<Feature>, IFeatureRepository
    {
        public FeatureRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}

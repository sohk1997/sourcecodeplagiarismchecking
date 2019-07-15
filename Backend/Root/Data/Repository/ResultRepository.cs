using Root.Data.Infrastructure;
using Root.Model;

namespace Root.Data.Repository
{
    public class ResultRepository : Repository<Result>, IResultRepository
    {
        public ResultRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface IResultRepository : IRepository<Result>
    {
    }
}

using Root.Data.Infrastructure;
using Root.Model;

namespace Root.Data.Repository
{
    public class MethodRepository : Repository<CodeDetail>, IMethodRepository
    {
        public MethodRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
    public interface IMethodRepository : IRepository<CodeDetail>
    {
    }
}

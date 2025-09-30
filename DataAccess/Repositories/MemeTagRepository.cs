using Domain.Interfaces.IMemeTag;
using Domain.Models;

namespace DataAccess.Repositories
{
    public class MemeTagRepository : RepositoryBase<MemeTag>, IMemeTagRepository
    {
        public MemeTagRepository(MisContext repositoryContext)
            : base(repositoryContext)
        { }
    }
}
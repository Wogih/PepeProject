using Domain.Interfaces.ICollection;
using Domain.Models;

namespace DataAccess.Repositories
{
    internal class CollectionRepository : RepositoryBase<Collection>, ICollectionRepository
    {
        public CollectionRepository(MisContext repositoryContext)
            : base(repositoryContext)
        { }
    }
}
using Domain.Interfaces.ICollectionMeme;
using Domain.Models;

namespace DataAccess.Repositories
{
    internal class CollectionMemeRepository : RepositoryBase<CollectionMeme>, ICollectionMemeRepository
    {
        public CollectionMemeRepository(MisContext repositoryContext)
            : base(repositoryContext)
        { }
    }
}
using Domain.Interfaces.ITag;
using Domain.Models;

namespace DataAccess.Repositories
{
    internal class TagRepository : RepositoryBase<Tag>, ITagRepository
    {
        public TagRepository(MisContext repositoryContext)
            : base(repositoryContext)
        { }
    }
}
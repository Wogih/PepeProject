using Domain.Interfaces.IComment;
using Domain.Models;

namespace DataAccess.Repositories
{
    internal class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(MisContext repositoryContext)
            : base(repositoryContext)
        { }
    }
}
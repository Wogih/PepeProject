using Domain.Interfaces.IReaction;
using Domain.Models;

namespace DataAccess.Repositories
{
    internal class ReactionRepository : RepositoryBase<Reaction>, IReactionRepository
    {
        public ReactionRepository(MisContext repositoryContext)
            : base(repositoryContext)
        { }
    }
}
using Domain.Interfaces.IMeme;
using Domain.Models;

namespace DataAccess.Repositories
{
    public class MemeRepository : RepositoryBase<Meme>, IMemeRepository
    {
        public MemeRepository(MisContext repositoryContext)
            : base(repositoryContext)
        { }
    }
}
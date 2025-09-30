using Domain.Interfaces.IRole;
using Domain.Models;

namespace DataAccess.Repositories
{
    internal class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(MisContext repositoryContext)
            : base(repositoryContext)
        { }
    }
}
using Domain.Interfaces.IUserRole;
using Domain.Models;

namespace DataAccess.Repositories
{
    public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(MisContext repositoryContext)
            : base(repositoryContext)
        { }
    }
}
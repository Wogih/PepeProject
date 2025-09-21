using DataAccess.Models;
using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Wrapper;

namespace DataAccess.Wrapper
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private MisContext _repoContext;

        private IUserRepository _user;
        public IUserRepository User
        {
            get
            {
                _user ??= new UserRepository(_repoContext);
                return _user;
            }
        }
        public RepositoryWrapper(MisContext repoContext)
        {
            _repoContext = repoContext;
        }
        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}

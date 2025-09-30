using Domain.Interfaces;
using Domain.Interfaces.IUserRole;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class UserRoleService : IUserRoleService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public UserRoleService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public Task<List<UserRole>> GetAll()
        {
            return _repositoryWrapper.UserRole.FindAll().ToListAsync();
        }

        public Task<UserRole> GetById(int id)
        {
            var userRole = _repositoryWrapper.UserRole
                .FindByCondition(x => x.UserRoleId == id).First();
            return Task.FromResult(userRole);
        }

        public Task Create(UserRole model)
        {
            _repositoryWrapper.UserRole.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(UserRole model)
        {
            _repositoryWrapper.UserRole.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var userRole = _repositoryWrapper.UserRole
                .FindByCondition(x => x.UserRoleId == id).First();

            _repositoryWrapper.UserRole.Delete(userRole);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
using Domain.Interfaces;
using Domain.Interfaces.IRole;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class RoleService : IRoleService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public RoleService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public Task<List<Role>> GetAll()
        {
            return _repositoryWrapper.Role.FindAll().ToListAsync();
        }

        public Task<Role> GetById(int id)
        {
            var role = _repositoryWrapper.Role
                .FindByCondition(x => x.RoleId == id).First();
            return Task.FromResult(role);
        }

        public Task Create(Role model)
        {
            _repositoryWrapper.Role.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(Role model)
        {
            _repositoryWrapper.Role.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var role = _repositoryWrapper.Role
                .FindByCondition(x => x.RoleId == id).First();

            _repositoryWrapper.Role.Delete(role);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
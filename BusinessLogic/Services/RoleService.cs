using Domain.Interfaces;
using Domain.Interfaces.IRole;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class RoleService(IRepositoryWrapper repositoryWrapper) : IRoleService
    {
        public async Task<List<Role>> GetAll()
        {
            return await repositoryWrapper.Role.FindAll();
        }

        public async Task<Role> GetById(int id)
        {
            var roles = await repositoryWrapper.Role
                .FindByCondition(x => x.RoleId == id);

            if (!roles.Any())
            {
                throw new InvalidOperationException("Role not found.");
            }

            if (roles.Count > 1)
            {
                throw new InvalidOperationException("Multiple roles found with the same ID.");
            }

            return roles.First();
        }

        public async Task Create(Role model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (string.IsNullOrWhiteSpace(model.RoleName))
            {
                throw new ArgumentException("Name cannot be null, empty, or whitespace.", nameof(model.RoleName));
            }

            await repositoryWrapper.Role.Create(model);
            await repositoryWrapper.Save();
        }

        public async Task Update(Role model)
        {
            ArgumentNullException.ThrowIfNull(model);

            await repositoryWrapper.Role.Update(model);
            await repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var roles = await repositoryWrapper.Role
                .FindByCondition(x => x.RoleId == id);
            
            if (!roles.Any())
            {
                throw new InvalidOperationException("Role not found.");
            }

            if (roles.Count > 1)
            {
                throw new InvalidOperationException("Multiple roles found with the same ID.");
            }

            await repositoryWrapper.Role.Delete(roles.First());
            await repositoryWrapper.Save();
        }
    }
}
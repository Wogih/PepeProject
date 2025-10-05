using Domain.Interfaces;
using Domain.Interfaces.IUserRole;
using Domain.Models;

namespace BusinessLogic.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public UserRoleService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<UserRole>> GetAll()
        {
            return await _repositoryWrapper.UserRole.FindAll();
        }

        public async Task<UserRole> GetById(int id)
        {
            var userRoles = await _repositoryWrapper.UserRole
                .FindByCondition(x => x.UserRoleId == id);

            if (!userRoles.Any())
            {
                throw new InvalidOperationException("UserRole not found.");
            }

            if (userRoles.Count > 1)
            {
                throw new InvalidOperationException("Multiple user roles found with the same ID.");
            }

            return userRoles.First();
        }

        public async Task Create(UserRole model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (model.UserId <= 0)
            {
                throw new ArgumentException("UserId must be greater than 0.", nameof(model.UserId));
            }

            if (model.RoleId <= 0)
            {
                throw new ArgumentException("RoleId must be greater than 0.", nameof(model.RoleId));
            }

            await _repositoryWrapper.UserRole.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(UserRole model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (model.UserId <= 0)
            {
                throw new ArgumentException("UserId must be greater than 0.", nameof(model.UserId));
            }

            if (model.RoleId <= 0)
            {
                throw new ArgumentException("RoleId must be greater than 0.", nameof(model.RoleId));
            }

            await _repositoryWrapper.UserRole.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var userRoles = await _repositoryWrapper.UserRole
                .FindByCondition(x => x.UserRoleId == id);

            if (!userRoles.Any())
            {
                throw new InvalidOperationException("UserRole not found.");
            }

            if (userRoles.Count > 1)
            {
                throw new InvalidOperationException("Multiple user roles found with the same ID.");
            }

            await _repositoryWrapper.UserRole.Delete(userRoles.First());
            await _repositoryWrapper.Save();
        }

        public async Task<List<UserRole>> GetByUserId(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("UserId must be greater than 0.", nameof(userId));
            }

            return await _repositoryWrapper.UserRole
                .FindByCondition(x => x.UserId == userId);
        }

        public async Task<List<UserRole>> GetByRoleId(int roleId)
        {
            if (roleId <= 0)
            {
                throw new ArgumentException("RoleId must be greater than 0.", nameof(roleId));
            }

            return await _repositoryWrapper.UserRole
                .FindByCondition(x => x.RoleId == roleId);
        }
    }
}
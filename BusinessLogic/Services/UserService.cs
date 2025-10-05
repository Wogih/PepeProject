using Domain.Interfaces;
using Domain.Interfaces.IUser;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class UserService(IRepositoryWrapper repositoryWrapper) : IUserService
    {
        public async Task<List<User>> GetAll()
        {
            return await repositoryWrapper.User.FindAll();
        }

        public async Task<User> GetById(int id)
        {
            var users = await repositoryWrapper.User
                .FindByCondition(x => x.UserId == id);

            if (!users.Any())
            {
                throw new InvalidOperationException("User not found.");
            }

            if (users.Count > 1)
            {
                throw new InvalidOperationException("Multiple users found with the same ID.");
            }

            return users.First();
        }

        public async Task Create(User model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (string.IsNullOrWhiteSpace(model.Username))
            {
                throw new ArgumentException("Username cannot be null, empty, or whitespace.", nameof(model.Username));
            }

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                throw new ArgumentException("Email cannot be null, empty, or whitespace.", nameof(model.Email));
            }

            if (string.IsNullOrWhiteSpace(model.PasswordHash))
            {
                throw new ArgumentException("PasswordHash cannot be null, empty, or whitespace.", nameof(model.PasswordHash));
            }

            await repositoryWrapper.User.Create(model);
            await repositoryWrapper.Save();
        }

        public async Task Update(User model)
        {
            ArgumentNullException.ThrowIfNull(model);

            await repositoryWrapper.User.Update(model);
            await repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var users = await repositoryWrapper.User
                .FindByCondition(x => x.UserId == id);
            
            if (!users.Any())
            {
                throw new InvalidOperationException("User not found.");
            }

            if (users.Count > 1)
            {
                throw new InvalidOperationException("Multiple users found with the same ID.");
            }

            await repositoryWrapper.User.Delete(users.First());
            await repositoryWrapper.Save();
        }
    }
}
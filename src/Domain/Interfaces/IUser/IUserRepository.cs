using Domain.Models;

namespace Domain.Interfaces.IUser
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> GetByIdWithToken(int userId);
        Task<User> GetByEmailWithToken(string email);
    }
}
using Domain.Models;

namespace Domain.Interfaces.IUserRole
{
    public interface IUserRoleService
    {
        Task<List<UserRole>> GetAll();
        Task<UserRole> GetById(int id);
        Task Create(UserRole model);
        Task Update(UserRole model);
        Task Delete(int id);
    }
}
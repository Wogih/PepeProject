using Domain.Models;

namespace Domain.Interfaces.ICollection
{
    public interface ICollectionService
    {
        Task<List<Collection>> GetAll();
        Task<Collection> GetById(int id);
        Task Create(Collection model);
        Task Update(Collection model);
        Task Delete(int id);
    }
}
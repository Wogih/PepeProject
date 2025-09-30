using Domain.Models;

namespace Domain.Interfaces.IMemeTag
{
    public interface IMemeTagService
    {
        Task<List<MemeTag>> GetAll();
        Task<MemeTag> GetById(int id);
        Task Create(MemeTag model);
        Task Update(MemeTag model);
        Task Delete(int id);
    }
}
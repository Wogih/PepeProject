using Domain.Models;

namespace Domain.Interfaces.ITag
{
    public interface ITagService
    {
        Task<List<Tag>> GetAll();
        Task<Tag> GetById(int id);
        Task Create(Tag model);
        Task Update(Tag model);
        Task Delete(int id);
    }
}
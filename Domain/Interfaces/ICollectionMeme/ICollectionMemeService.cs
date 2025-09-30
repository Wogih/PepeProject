using Domain.Models;

namespace Domain.Interfaces.ICollectionMeme
{
    public interface ICollectionMemeService
    {
        Task<List<CollectionMeme>> GetAll();
        Task<CollectionMeme> GetById(int id);
        Task Create(CollectionMeme model);
        Task Update(CollectionMeme model);
        Task Delete(int id);
    }
}
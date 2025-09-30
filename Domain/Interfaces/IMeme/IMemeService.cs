using Domain.Models;

namespace Domain.Interfaces.IMeme
{
    public interface IMemeService
    {
        Task<List<Meme>> GetAll();
        Task<Meme> GetById(int id);
        Task Create(Meme model);
        Task Update(Meme model);
        Task Delete(int id);
    }
}
using Domain.Models;

namespace Domain.Interfaces.IReaction
{
    public interface IReactionService
    {
        Task<List<Reaction>> GetAll();
        Task<Reaction> GetById(int id);
        Task Create(Reaction model);
        Task Update(Reaction model);
        Task Delete(int id);
    }
}
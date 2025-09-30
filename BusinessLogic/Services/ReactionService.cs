using Domain.Interfaces;
using Domain.Interfaces.IReaction;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class ReactionService : IReactionService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public ReactionService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public Task<List<Reaction>> GetAll()
        {
            return _repositoryWrapper.Reaction.FindAll().ToListAsync();
        }

        public Task<Reaction> GetById(int id)
        {
            var reaction = _repositoryWrapper.Reaction
                .FindByCondition(x => x.ReactionId == id).First();
            return Task.FromResult(reaction);
        }

        public Task Create(Reaction model)
        {
            _repositoryWrapper.Reaction.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(Reaction model)
        {
            _repositoryWrapper.Reaction.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var reaction = _repositoryWrapper.Reaction
                .FindByCondition(x => x.ReactionId == id).First();

            _repositoryWrapper.Reaction.Delete(reaction);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
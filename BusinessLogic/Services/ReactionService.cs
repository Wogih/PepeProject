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

        public async Task<List<Reaction>> GetAll()
        {
            return await _repositoryWrapper.Reaction.FindAll();
        }

        public async Task<Reaction> GetById(int id)
        {
            var reactions = await _repositoryWrapper.Reaction
                .FindByCondition(x => x.ReactionId == id);

            if (!reactions.Any())
            {
                throw new InvalidOperationException("Reaction not found.");
            }

            if (reactions.Count > 1)
            {
                throw new InvalidOperationException("Multiple reactions found with the same ID.");
            }

            return reactions.First();
        }

        public async Task<List<Reaction>> GetByMemeId(int memeId)
        {
            return await _repositoryWrapper.Reaction
                .FindByCondition(x => x.MemeId == memeId);
        }

        public async Task<List<Reaction>> GetByUserId(int userId)
        {
            return await _repositoryWrapper.Reaction
                .FindByCondition(x => x.UserId == userId);
        }

        public async Task<Reaction> GetByMemeAndUser(int memeId, int userId)
        {
            var reactions = await _repositoryWrapper.Reaction
                .FindByCondition(x => x.MemeId == memeId && x.UserId == userId);

            if (!reactions.Any())
            {
                throw new InvalidOperationException("Reaction not found for specified meme and user.");
            }

            if (reactions.Count > 1)
            {
                throw new InvalidOperationException("Multiple reactions found for the same meme and user.");
            }

            return reactions.First();
        }

        public async Task<Dictionary<string, int>> GetReactionCounts(int memeId)
        {
            var reactions = await _repositoryWrapper.Reaction
                .FindByCondition(x => x.MemeId == memeId);

            return reactions
                .GroupBy(r => r.ReactionType)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task Create(Reaction model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (model.MemeId <= 0)
            {
                throw new ArgumentException("MemeId must be greater than 0.", nameof(model.MemeId));
            }

            if (model.UserId <= 0)
            {
                throw new ArgumentException("UserId must be greater than 0.", nameof(model.UserId));
            }

            if (string.IsNullOrWhiteSpace(model.ReactionType))
            {
                throw new ArgumentException("ReactionType cannot be null, empty, or whitespace.", nameof(model.ReactionType));
            }

            // Проверяем, не существует ли уже реакция от этого пользователя на этот мем
            var existingReactions = await _repositoryWrapper.Reaction
                .FindByCondition(x => x.MemeId == model.MemeId && x.UserId == model.UserId);
            
            if (existingReactions.Any())
            {
                throw new InvalidOperationException("User has already reacted to this meme.");
            }

            await _repositoryWrapper.Reaction.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(Reaction model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (model.MemeId <= 0)
            {
                throw new ArgumentException("MemeId must be greater than 0.", nameof(model.MemeId));
            }

            if (model.UserId <= 0)
            {
                throw new ArgumentException("UserId must be greater than 0.", nameof(model.UserId));
            }

            if (string.IsNullOrWhiteSpace(model.ReactionType))
            {
                throw new ArgumentException("ReactionType cannot be null, empty, or whitespace.", nameof(model.ReactionType));
            }

            // Убеждаемся, что реакция существует
            var existingReactions = await _repositoryWrapper.Reaction
                .FindByCondition(x => x.ReactionId == model.ReactionId);
            
            if (!existingReactions.Any())
            {
                throw new InvalidOperationException("Reaction not found.");
            }

            await _repositoryWrapper.Reaction.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task UpdateReaction(int memeId, int userId, string newReactionType)
        {
            if (string.IsNullOrWhiteSpace(newReactionType))
            {
                throw new ArgumentException("ReactionType cannot be null, empty, or whitespace.", nameof(newReactionType));
            }

            var reactions = await _repositoryWrapper.Reaction
                .FindByCondition(x => x.MemeId == memeId && x.UserId == userId);

            if (!reactions.Any())
            {
                throw new InvalidOperationException("Reaction not found for specified meme and user.");
            }

            if (reactions.Count > 1)
            {
                throw new InvalidOperationException("Multiple reactions found for the same meme and user.");
            }

            var reaction = reactions.First();
            reaction.ReactionType = newReactionType;

            await _repositoryWrapper.Reaction.Update(reaction);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var reactions = await _repositoryWrapper.Reaction
                .FindByCondition(x => x.ReactionId == id);
            
            if (!reactions.Any())
            {
                throw new InvalidOperationException("Reaction not found.");
            }

            if (reactions.Count > 1)
            {
                throw new InvalidOperationException("Multiple reactions found with the same ID.");
            }

            await _repositoryWrapper.Reaction.Delete(reactions.First());
            await _repositoryWrapper.Save();
        }

        public async Task DeleteByMemeAndUser(int memeId, int userId)
        {
            var reactions = await _repositoryWrapper.Reaction
                .FindByCondition(x => x.MemeId == memeId && x.UserId == userId);
            
            if (!reactions.Any())
            {
                throw new InvalidOperationException("Reaction not found for specified meme and user.");
            }

            if (reactions.Count > 1)
            {
                throw new InvalidOperationException("Multiple reactions found for the same meme and user.");
            }

            await _repositoryWrapper.Reaction.Delete(reactions.First());
            await _repositoryWrapper.Save();
        }
    }
}
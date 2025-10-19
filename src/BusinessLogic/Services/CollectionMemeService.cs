using Domain.Interfaces;
using Domain.Interfaces.ICollectionMeme;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class CollectionMemeService : ICollectionMemeService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public CollectionMemeService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<CollectionMeme>> GetAll()
        {
            return await _repositoryWrapper.CollectionMeme.FindAll();
        }

        public async Task<CollectionMeme> GetById(int id)
        {
            var collectionMemes = await _repositoryWrapper.CollectionMeme
                .FindByCondition(x => x.CollectionMemeId == id);

            if (!collectionMemes.Any())
            {
                throw new InvalidOperationException("CollectionMeme not found.");
            }

            if (collectionMemes.Count > 1)
            {
                throw new InvalidOperationException("Multiple collection memes found with the same ID.");
            }

            return collectionMemes.First();
        }

        public async Task<List<CollectionMeme>> GetByCollectionId(int collectionId)
        {
            return await _repositoryWrapper.CollectionMeme
                .FindByCondition(x => x.CollectionId == collectionId);
        }

        public async Task<List<CollectionMeme>> GetByMemeId(int memeId)
        {
            return await _repositoryWrapper.CollectionMeme
                .FindByCondition(x => x.MemeId == memeId);
        }

        public async Task<CollectionMeme> GetByCollectionAndMeme(int collectionId, int memeId)
        {
            var collectionMemes = await _repositoryWrapper.CollectionMeme
                .FindByCondition(x => x.CollectionId == collectionId && x.MemeId == memeId);

            if (!collectionMemes.Any())
            {
                throw new InvalidOperationException("CollectionMeme not found for specified collection and meme.");
            }

            if (collectionMemes.Count > 1)
            {
                throw new InvalidOperationException("Multiple collection memes found for the same collection and meme.");
            }

            return collectionMemes.First();
        }

        public async Task<bool> ExistsInCollection(int collectionId, int memeId)
        {
            var collectionMemes = await _repositoryWrapper.CollectionMeme
                .FindByCondition(x => x.CollectionId == collectionId && x.MemeId == memeId);

            return collectionMemes.Any();
        }

        public async Task<int> GetMemeCountInCollection(int collectionId)
        {
            var collectionMemes = await _repositoryWrapper.CollectionMeme
                .FindByCondition(x => x.CollectionId == collectionId);

            return collectionMemes.Count;
        }

        public async Task Create(CollectionMeme model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (model.CollectionId <= 0)
            {
                throw new ArgumentException("CollectionId must be greater than 0.", nameof(model.CollectionId));
            }

            if (model.MemeId <= 0)
            {
                throw new ArgumentException("MemeId must be greater than 0.", nameof(model.MemeId));
            }

            var collection = await _repositoryWrapper.Collection
                .FindByCondition(x => x.CollectionId == model.CollectionId);

            if (!collection.Any())
            {
                throw new InvalidOperationException("Collection not found.");
            }

            var meme = await _repositoryWrapper.Meme
                .FindByCondition(x => x.MemeId == model.MemeId);

            if (!meme.Any())
            {
                throw new InvalidOperationException("Meme not found.");
            }

            var existingCollectionMemes = await _repositoryWrapper.CollectionMeme
                .FindByCondition(x => x.CollectionId == model.CollectionId && x.MemeId == model.MemeId);

            if (existingCollectionMemes.Any())
            {
                throw new InvalidOperationException("Meme is already in this collection.");
            }

            await _repositoryWrapper.CollectionMeme.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task AddMemeToCollection(int collectionId, int memeId)
        {
            var collectionMeme = new CollectionMeme
            {
                CollectionId = collectionId,
                MemeId = memeId
            };

            await Create(collectionMeme);
        }

        public async Task Update(CollectionMeme model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (model.CollectionId <= 0)
            {
                throw new ArgumentException("CollectionId must be greater than 0.", nameof(model.CollectionId));
            }

            if (model.MemeId <= 0)
            {
                throw new ArgumentException("MemeId must be greater than 0.", nameof(model.MemeId));
            }

            var existingCollectionMemes = await _repositoryWrapper.CollectionMeme
                .FindByCondition(x => x.CollectionMemeId == model.CollectionMemeId);

            if (!existingCollectionMemes.Any())
            {
                throw new InvalidOperationException("CollectionMeme not found.");
            }

            var collection = await _repositoryWrapper.Collection
                .FindByCondition(x => x.CollectionId == model.CollectionId);

            if (!collection.Any())
            {
                throw new InvalidOperationException("Collection not found.");
            }

            var meme = await _repositoryWrapper.Meme
                .FindByCondition(x => x.MemeId == model.MemeId);

            if (!meme.Any())
            {
                throw new InvalidOperationException("Meme not found.");
            }

            var duplicateCollectionMemes = await _repositoryWrapper.CollectionMeme
                .FindByCondition(x => x.CollectionId == model.CollectionId &&
                                     x.MemeId == model.MemeId &&
                                     x.CollectionMemeId != model.CollectionMemeId);

            if (duplicateCollectionMemes.Any())
            {
                throw new InvalidOperationException("Another CollectionMeme with the same collection and meme already exists.");
            }

            await _repositoryWrapper.CollectionMeme.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var collectionMemes = await _repositoryWrapper.CollectionMeme
                .FindByCondition(x => x.CollectionMemeId == id);

            if (!collectionMemes.Any())
            {
                throw new InvalidOperationException("CollectionMeme not found.");
            }

            if (collectionMemes.Count > 1)
            {
                throw new InvalidOperationException("Multiple collection memes found with the same ID.");
            }

            await _repositoryWrapper.CollectionMeme.Delete(collectionMemes.First());
            await _repositoryWrapper.Save();
        }

        public async Task RemoveMemeFromCollection(int collectionId, int memeId)
        {
            var collectionMemes = await _repositoryWrapper.CollectionMeme
                .FindByCondition(x => x.CollectionId == collectionId && x.MemeId == memeId);

            if (!collectionMemes.Any())
            {
                throw new InvalidOperationException("CollectionMeme not found for specified collection and meme.");
            }

            if (collectionMemes.Count > 1)
            {
                throw new InvalidOperationException("Multiple collection memes found for the same collection and meme.");
            }

            await _repositoryWrapper.CollectionMeme.Delete(collectionMemes.First());
            await _repositoryWrapper.Save();
        }

        public async Task ClearCollection(int collectionId)
        {
            var collectionMemes = await _repositoryWrapper.CollectionMeme
                .FindByCondition(x => x.CollectionId == collectionId);

            foreach (var collectionMeme in collectionMemes)
            {
                await _repositoryWrapper.CollectionMeme.Delete(collectionMeme);
            }

            if (collectionMemes.Any())
            {
                await _repositoryWrapper.Save();
            }
        }

        public async Task<List<int>> GetMemeIdsInCollection(int collectionId)
        {
            var collectionMemes = await _repositoryWrapper.CollectionMeme
                .FindByCondition(x => x.CollectionId == collectionId);

            return collectionMemes.Select(cm => cm.MemeId).ToList();
        }

        public async Task<List<int>> GetCollectionIdsForMeme(int memeId)
        {
            var collectionMemes = await _repositoryWrapper.CollectionMeme
                .FindByCondition(x => x.MemeId == memeId);

            return collectionMemes.Select(cm => cm.CollectionId).ToList();
        }
    }
}
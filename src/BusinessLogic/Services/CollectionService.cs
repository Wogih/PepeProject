using Domain.Interfaces;
using Domain.Interfaces.ICollection;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class CollectionService : ICollectionService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public CollectionService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<Collection>> GetAll()
        {
            return await _repositoryWrapper.Collection.FindAll();
        }

        public async Task<List<Collection>> GetPublicCollections()
        {
            return await _repositoryWrapper.Collection
                .FindByCondition(x => x.IsPublic == true);
        }

        public async Task<List<Collection>> GetByUserId(int userId)
        {
            return await _repositoryWrapper.Collection
                .FindByCondition(x => x.UserId == userId);
        }

        public async Task<Collection> GetById(int id)
        {
            var collections = await _repositoryWrapper.Collection
                .FindByCondition(x => x.CollectionId == id);

            if (!collections.Any())
            {
                throw new InvalidOperationException("Collection not found.");
            }

            if (collections.Count > 1)
            {
                throw new InvalidOperationException("Multiple collections found with the same ID.");
            }

            return collections.First();
        }

        public async Task<Collection> GetUserCollectionById(int userId, int collectionId)
        {
            var collections = await _repositoryWrapper.Collection
                .FindByCondition(x => x.CollectionId == collectionId && x.UserId == userId);

            if (!collections.Any())
            {
                throw new InvalidOperationException("Collection not found for specified user.");
            }

            if (collections.Count > 1)
            {
                throw new InvalidOperationException("Multiple collections found with the same ID for the same user.");
            }

            return collections.First();
        }

        public async Task Create(Collection model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (model.UserId <= 0)
            {
                throw new ArgumentException("UserId must be greater than 0.", nameof(model.UserId));
            }

            if (string.IsNullOrWhiteSpace(model.CollectionName))
            {
                throw new ArgumentException("CollectionName cannot be null, empty, or whitespace.", nameof(model.CollectionName));
            }

            // Проверяем, не существует ли уже коллекция с таким именем у этого пользователя
            var existingCollections = await _repositoryWrapper.Collection
                .FindByCondition(x => x.UserId == model.UserId && x.CollectionName == model.CollectionName);

            if (existingCollections.Any())
            {
                throw new InvalidOperationException("Collection with this name already exists for the user.");
            }

            // Устанавливаем значения по умолчанию
            model.IsPublic ??= false;
            model.Description ??= string.Empty;

            await _repositoryWrapper.Collection.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(Collection model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (model.UserId <= 0)
            {
                throw new ArgumentException("UserId must be greater than 0.", nameof(model.UserId));
            }

            if (string.IsNullOrWhiteSpace(model.CollectionName))
            {
                throw new ArgumentException("CollectionName cannot be null, empty, or whitespace.", nameof(model.CollectionName));
            }

            // Убеждаемся, что коллекция существует
            var existingCollections = await _repositoryWrapper.Collection
                .FindByCondition(x => x.CollectionId == model.CollectionId);

            if (!existingCollections.Any())
            {
                throw new InvalidOperationException("Collection not found.");
            }

            // Проверяем, не существует ли другой коллекции с таким именем у этого пользователя
            var duplicateCollections = await _repositoryWrapper.Collection
                .FindByCondition(x => x.UserId == model.UserId &&
                                     x.CollectionName == model.CollectionName &&
                                     x.CollectionId != model.CollectionId);

            if (duplicateCollections.Any())
            {
                throw new InvalidOperationException("Another collection with this name already exists for the user.");
            }

            await _repositoryWrapper.Collection.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task UpdateCollectionName(int collectionId, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Collection name cannot be null, empty, or whitespace.", nameof(newName));
            }

            var collections = await _repositoryWrapper.Collection
                .FindByCondition(x => x.CollectionId == collectionId);

            if (!collections.Any())
            {
                throw new InvalidOperationException("Collection not found.");
            }

            var collection = collections.First();
            collection.CollectionName = newName;

            await _repositoryWrapper.Collection.Update(collection);
            await _repositoryWrapper.Save();
        }

        public async Task UpdateCollectionVisibility(int collectionId, bool isPublic)
        {
            var collections = await _repositoryWrapper.Collection
                .FindByCondition(x => x.CollectionId == collectionId);

            if (!collections.Any())
            {
                throw new InvalidOperationException("Collection not found.");
            }

            var collection = collections.First();
            collection.IsPublic = isPublic;

            await _repositoryWrapper.Collection.Update(collection);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var collections = await _repositoryWrapper.Collection
                .FindByCondition(x => x.CollectionId == id);

            if (!collections.Any())
            {
                throw new InvalidOperationException("Collection not found.");
            }

            if (collections.Count > 1)
            {
                throw new InvalidOperationException("Multiple collections found with the same ID.");
            }

            // Проверяем, есть ли мемы в коллекции
            var collectionMemes = await _repositoryWrapper.CollectionMeme
                .FindByCondition(x => x.CollectionId == id);

            if (collectionMemes.Any())
            {
                throw new InvalidOperationException("Cannot delete collection that contains memes. Remove memes first.");
            }

            await _repositoryWrapper.Collection.Delete(collections.First());
            await _repositoryWrapper.Save();
        }

        public async Task<bool> IsCollectionOwner(int collectionId, int userId)
        {
            var collections = await _repositoryWrapper.Collection
                .FindByCondition(x => x.CollectionId == collectionId && x.UserId == userId);

            return collections.Any();
        }

        public async Task<int> GetCollectionMemeCount(int collectionId)
        {
            var collectionMemes = await _repositoryWrapper.CollectionMeme
                .FindByCondition(x => x.CollectionId == collectionId);

            return collectionMemes.Count;
        }
    }
}
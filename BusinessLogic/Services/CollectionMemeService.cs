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

        public Task<List<CollectionMeme>> GetAll()
        {
            return _repositoryWrapper.CollectionMeme.FindAll().ToListAsync();
        }

        public Task<CollectionMeme> GetById(int id)
        {
            var collectionMeme = _repositoryWrapper.CollectionMeme
                .FindByCondition(x => x.CollectionMemeId == id).First();
            return Task.FromResult(collectionMeme);
        }

        public Task Create(CollectionMeme model)
        {
            _repositoryWrapper.CollectionMeme.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(CollectionMeme model)
        {
            _repositoryWrapper.CollectionMeme.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var collectionMeme = _repositoryWrapper.CollectionMeme
                .FindByCondition(x => x.CollectionMemeId == id).First();

            _repositoryWrapper.CollectionMeme.Delete(collectionMeme);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
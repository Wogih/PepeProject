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

        public Task<List<Collection>> GetAll()
        {
            return _repositoryWrapper.Collection.FindAll().ToListAsync();
        }

        public Task<Collection> GetById(int id)
        {
            var collection = _repositoryWrapper.Collection
                .FindByCondition(x => x.CollectionId == id).First();
            return Task.FromResult(collection);
        }

        public Task Create(Collection model)
        {
            _repositoryWrapper.Collection.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(Collection model)
        {
            _repositoryWrapper.Collection.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var collection = _repositoryWrapper.Collection
                .FindByCondition(x => x.CollectionId == id).First();

            _repositoryWrapper.Collection.Delete(collection);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
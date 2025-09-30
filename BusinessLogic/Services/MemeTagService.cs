using Domain.Interfaces;
using Domain.Interfaces.IMemeTag;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class MemeTagService : IMemeTagService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public MemeTagService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public Task<List<MemeTag>> GetAll()
        {
            return _repositoryWrapper.MemeTag.FindAll().ToListAsync();
        }

        public Task<MemeTag> GetById(int id)
        {
            var memeTag = _repositoryWrapper.MemeTag
                .FindByCondition(x => x.MemeTagId == id).First();
            return Task.FromResult(memeTag);
        }

        public Task Create(MemeTag model)
        {
            _repositoryWrapper.MemeTag.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(MemeTag model)
        {
            _repositoryWrapper.MemeTag.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var memeTag = _repositoryWrapper.MemeTag
                .FindByCondition(x => x.MemeTagId == id).First();

            _repositoryWrapper.MemeTag.Delete(memeTag);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
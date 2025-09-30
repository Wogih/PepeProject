using Domain.Interfaces;
using Domain.Interfaces.IMeme;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class MemeService : IMemeService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public MemeService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public Task<List<Meme>> GetAll()
        {
            return _repositoryWrapper.Meme.FindAll().ToListAsync();
        }

        public Task<Meme> GetById(int id)
        {
            var meme = _repositoryWrapper.Meme
                .FindByCondition(x => x.MemeId == id).First();
            return Task.FromResult(meme);
        }

        public Task Create(Meme model)
        {
            _repositoryWrapper.Meme.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(Meme model)
        {
            _repositoryWrapper.Meme.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var meme = _repositoryWrapper.Meme
                .FindByCondition(x => x.MemeId == id).First();

            _repositoryWrapper.Meme.Delete(meme);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
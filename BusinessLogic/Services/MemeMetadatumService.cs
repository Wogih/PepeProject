using Domain.Interfaces;
using Domain.Interfaces.IMemeMetadatum;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class MemeMetadatumService : IMemeMetadatumService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public MemeMetadatumService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public Task<List<MemeMetadatum>> GetAll()
        {
            return _repositoryWrapper.MemeMetadatum.FindAll().ToListAsync();
        }

        public Task<MemeMetadatum> GetById(int id)
        {
            var memeMetadatum = _repositoryWrapper.MemeMetadatum
                .FindByCondition(x => x.MemeId == id).First();
            return Task.FromResult(memeMetadatum);
        }

        public Task Create(MemeMetadatum model)
        {
            _repositoryWrapper.MemeMetadatum.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(MemeMetadatum model)
        {
            _repositoryWrapper.MemeMetadatum.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var memeMetadatum = _repositoryWrapper.MemeMetadatum
                .FindByCondition(x => x.MemeId == id).First();

            _repositoryWrapper.MemeMetadatum.Delete(memeMetadatum);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
using Domain.Interfaces;
using Domain.Interfaces.IUploadStat;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class UploadStatService : IUploadStatService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public UploadStatService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public Task<List<UploadStat>> GetAll()
        {
            return _repositoryWrapper.UploadStat.FindAll().ToListAsync();
        }

        public Task<UploadStat> GetById(int id)
        {
            var uploadStat = _repositoryWrapper.UploadStat
                .FindByCondition(x => x.StatId == id).First();
            return Task.FromResult(uploadStat);
        }

        public Task Create(UploadStat model)
        {
            _repositoryWrapper.UploadStat.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(UploadStat model)
        {
            _repositoryWrapper.UploadStat.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var uploadStat = _repositoryWrapper.UploadStat
                .FindByCondition(x => x.StatId == id).First();

            _repositoryWrapper.UploadStat.Delete(uploadStat);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
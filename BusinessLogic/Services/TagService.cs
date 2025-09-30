using Domain.Interfaces;
using Domain.Interfaces.ITag;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class TagService : ITagService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public TagService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public Task<List<Tag>> GetAll()
        {
            return _repositoryWrapper.Tag.FindAll().ToListAsync();
        }

        public Task<Tag> GetById(int id)
        {
            var tag = _repositoryWrapper.Tag
                .FindByCondition(x => x.TagId == id).First();
            return Task.FromResult(tag);
        }

        public Task Create(Tag model)
        {
            _repositoryWrapper.Tag.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(Tag model)
        {
            _repositoryWrapper.Tag.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var tag = _repositoryWrapper.Tag
                .FindByCondition(x => x.TagId == id).First();

            _repositoryWrapper.Tag.Delete(tag);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
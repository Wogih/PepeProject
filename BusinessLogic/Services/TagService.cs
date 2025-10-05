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

        public async Task<List<Tag>> GetAll()
        {
            return await _repositoryWrapper.Tag.FindAll();
        }

        public async Task<Tag> GetById(int id)
        {
            var tags = await _repositoryWrapper.Tag
                .FindByCondition(x => x.TagId == id);
            
            if (!tags.Any())
            {
                throw new InvalidOperationException("Tag not found.");
            }

            if (tags.Count > 1)
            {
                throw new InvalidOperationException("Multiple tags found with the same ID.");
            }
            
            return tags.First();
        }

        public async Task Create(Tag model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (string.IsNullOrWhiteSpace(model.TagName))
            {
                throw new ArgumentException("TagName cannot be null, empty, or whitespace.", nameof(model.TagName));
            }
            
            await _repositoryWrapper.Tag.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(Tag model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (string.IsNullOrWhiteSpace(model.TagName))
            {
                throw new ArgumentException("TagName cannot be null, empty, or whitespace.", nameof(model.TagName));
            }
            
            await _repositoryWrapper.Tag.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var tags = await _repositoryWrapper.Tag
                .FindByCondition(x => x.TagId == id);
            
            if (!tags.Any())
            {
                throw new InvalidOperationException("Tag not found.");
            }

            if (tags.Count > 1)
            {
                throw new InvalidOperationException("Multiple tags found with the same ID.");
            }
    
            await _repositoryWrapper.Tag.Delete(tags.First());
            await _repositoryWrapper.Save();
        }
    }
}
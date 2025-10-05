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

        public async Task<List<MemeTag>> GetAll()
        {
            return await _repositoryWrapper.MemeTag.FindAll();
        }

        public async Task<MemeTag> GetById(int id)
        {
            var memeTags = await _repositoryWrapper.MemeTag
                .FindByCondition(x => x.MemeTagId == id);

            if (!memeTags.Any())
            {
                throw new InvalidOperationException("MemeTag not found.");
            }

            if (memeTags.Count > 1)
            {
                throw new InvalidOperationException("Multiple meme tags found with the same ID.");
            }

            return memeTags.First();
        }

        public async Task<List<MemeTag>> GetByMemeId(int memeId)
        {
            return await _repositoryWrapper.MemeTag
                .FindByCondition(x => x.MemeId == memeId);
        }

        public async Task<List<MemeTag>> GetByTagId(int tagId)
        {
            return await _repositoryWrapper.MemeTag
                .FindByCondition(x => x.TagId == tagId);
        }

        public async Task<MemeTag> GetByMemeAndTag(int memeId, int tagId)
        {
            var memeTags = await _repositoryWrapper.MemeTag
                .FindByCondition(x => x.MemeId == memeId && x.TagId == tagId);

            if (!memeTags.Any())
            {
                throw new InvalidOperationException("MemeTag not found for specified meme and tag.");
            }

            if (memeTags.Count > 1)
            {
                throw new InvalidOperationException("Multiple meme tags found for the same meme and tag.");
            }

            return memeTags.First();
        }

        public async Task<bool> ExistsForMemeAndTag(int memeId, int tagId)
        {
            var memeTags = await _repositoryWrapper.MemeTag
                .FindByCondition(x => x.MemeId == memeId && x.TagId == tagId);
            
            return memeTags.Any();
        }

        public async Task<List<int>> GetTagIdsForMeme(int memeId)
        {
            var memeTags = await _repositoryWrapper.MemeTag
                .FindByCondition(x => x.MemeId == memeId);
            
            return memeTags.Select(mt => mt.TagId).ToList();
        }

        public async Task<List<int>> GetMemeIdsForTag(int tagId)
        {
            var memeTags = await _repositoryWrapper.MemeTag
                .FindByCondition(x => x.TagId == tagId);
            
            return memeTags.Select(mt => mt.MemeId).ToList();
        }

        public async Task<int> GetTagCountForMeme(int memeId)
        {
            var memeTags = await _repositoryWrapper.MemeTag
                .FindByCondition(x => x.MemeId == memeId);
            
            return memeTags.Count;
        }

        public async Task<int> GetMemeCountForTag(int tagId)
        {
            var memeTags = await _repositoryWrapper.MemeTag
                .FindByCondition(x => x.TagId == tagId);
            
            return memeTags.Count;
        }

        public async Task Create(MemeTag model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (model.MemeId <= 0)
            {
                throw new ArgumentException("MemeId must be greater than 0.", nameof(model.MemeId));
            }

            if (model.TagId <= 0)
            {
                throw new ArgumentException("TagId must be greater than 0.", nameof(model.TagId));
            }

            var meme = await _repositoryWrapper.Meme
                .FindByCondition(x => x.MemeId == model.MemeId);
            
            if (!meme.Any())
            {
                throw new InvalidOperationException("Meme not found.");
            }

            var tag = await _repositoryWrapper.Tag
                .FindByCondition(x => x.TagId == model.TagId);
            
            if (!tag.Any())
            {
                throw new InvalidOperationException("Tag not found.");
            }

            var existingMemeTags = await _repositoryWrapper.MemeTag
                .FindByCondition(x => x.MemeId == model.MemeId && x.TagId == model.TagId);
            
            if (existingMemeTags.Any())
            {
                throw new InvalidOperationException("MemeTag relationship already exists.");
            }

            await _repositoryWrapper.MemeTag.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task AddTagToMeme(int memeId, int tagId)
        {
            var memeTag = new MemeTag
            {
                MemeId = memeId,
                TagId = tagId
            };

            await Create(memeTag);
        }

        public async Task AddTagsToMeme(int memeId, IEnumerable<int> tagIds)
        {
            foreach (int tagId in tagIds)
            {
                await AddTagToMeme(memeId, tagId);
            }
        }

        public async Task Update(MemeTag model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (model.MemeId <= 0)
            {
                throw new ArgumentException("MemeId must be greater than 0.", nameof(model.MemeId));
            }

            if (model.TagId <= 0)
            {
                throw new ArgumentException("TagId must be greater than 0.", nameof(model.TagId));
            }

            var existingMemeTags = await _repositoryWrapper.MemeTag
                .FindByCondition(x => x.MemeTagId == model.MemeTagId);
            
            if (!existingMemeTags.Any())
            {
                throw new InvalidOperationException("MemeTag not found.");
            }

            var meme = await _repositoryWrapper.Meme
                .FindByCondition(x => x.MemeId == model.MemeId);
            
            if (!meme.Any())
            {
                throw new InvalidOperationException("Meme not found.");
            }

            var tag = await _repositoryWrapper.Tag
                .FindByCondition(x => x.TagId == model.TagId);
            
            if (!tag.Any())
            {
                throw new InvalidOperationException("Tag not found.");
            }

            var duplicateMemeTags = await _repositoryWrapper.MemeTag
                .FindByCondition(x => x.MemeId == model.MemeId && 
                                     x.TagId == model.TagId && 
                                     x.MemeTagId != model.MemeTagId);
            
            if (duplicateMemeTags.Any())
            {
                throw new InvalidOperationException("Another MemeTag with the same meme and tag already exists.");
            }

            await _repositoryWrapper.MemeTag.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var memeTags = await _repositoryWrapper.MemeTag
                .FindByCondition(x => x.MemeTagId == id);
            
            if (!memeTags.Any())
            {
                throw new InvalidOperationException("MemeTag not found.");
            }

            if (memeTags.Count > 1)
            {
                throw new InvalidOperationException("Multiple meme tags found with the same ID.");
            }

            await _repositoryWrapper.MemeTag.Delete(memeTags.First());
            await _repositoryWrapper.Save();
        }

        public async Task RemoveTagFromMeme(int memeId, int tagId)
        {
            var memeTags = await _repositoryWrapper.MemeTag
                .FindByCondition(x => x.MemeId == memeId && x.TagId == tagId);
            
            if (!memeTags.Any())
            {
                throw new InvalidOperationException("MemeTag not found for specified meme and tag.");
            }

            if (memeTags.Count > 1)
            {
                throw new InvalidOperationException("Multiple meme tags found for the same meme and tag.");
            }

            await _repositoryWrapper.MemeTag.Delete(memeTags.First());
            await _repositoryWrapper.Save();
        }

        public async Task ClearTagsFromMeme(int memeId)
        {
            var memeTags = await _repositoryWrapper.MemeTag
                .FindByCondition(x => x.MemeId == memeId);
            
            foreach (var memeTag in memeTags)
            {
                await _repositoryWrapper.MemeTag.Delete(memeTag);
            }

            if (memeTags.Any())
            {
                await _repositoryWrapper.Save();
            }
        }

        public async Task ClearMemesFromTag(int tagId)
        {
            var memeTags = await _repositoryWrapper.MemeTag
                .FindByCondition(x => x.TagId == tagId);
            
            foreach (var memeTag in memeTags)
            {
                await _repositoryWrapper.MemeTag.Delete(memeTag);
            }

            if (memeTags.Any())
            {
                await _repositoryWrapper.Save();
            }
        }

        public async Task<List<Meme>> GetMemesByTag(int tagId)
        {
            var memeTags = await _repositoryWrapper.MemeTag
                .FindByCondition(x => x.TagId == tagId);
            
            var memeIds = memeTags.Select(mt => mt.MemeId).ToList();
            
            if (!memeIds.Any())
            {
                return new List<Meme>();
            }

            var memes = await _repositoryWrapper.Meme
                .FindByCondition(x => memeIds.Contains(x.MemeId));
            
            return memes;
        }

        public async Task<List<Tag>> GetTagsByMeme(int memeId)
        {
            var memeTags = await _repositoryWrapper.MemeTag
                .FindByCondition(x => x.MemeId == memeId);
            
            var tagIds = memeTags.Select(mt => mt.TagId).ToList();
            
            if (!tagIds.Any())
            {
                return new List<Tag>();
            }

            var tags = await _repositoryWrapper.Tag
                .FindByCondition(x => tagIds.Contains(x.TagId));
            
            return tags;
        }
    }
}
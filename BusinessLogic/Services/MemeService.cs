using Domain.Interfaces;
using Domain.Interfaces.IMeme;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace BusinessLogic.Services
{
    public class MemeService : IMemeService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public MemeService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<Meme>> GetAll()
        {
            return await _repositoryWrapper.Meme.FindAll();
        }

        public async Task<Meme> GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid id.");
            }

            var meme = await _repositoryWrapper.Meme
                .FindByCondition(x => x.MemeId == id);
            
            if (!meme.Any())
            {
                throw new InvalidOperationException("Meme not found.");
            }

            if (meme.Count > 1)
            {
                throw new InvalidOperationException("Multiple memes found with the same ID.");
            }
            
            return meme.First();
        }

        public async Task Create(Meme model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (model.MemeId <= 0)
            {
                throw new ArgumentException("Invalid id.");
            }

            if (string.IsNullOrWhiteSpace(model.Title))
            {
                throw new ArgumentException("Title is required.");
            }

            if (string.IsNullOrWhiteSpace(model.ImageUrl))
            {
                throw new ArgumentException("ImageUrl is required.");
            }
            
            await _repositoryWrapper.Meme.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(Meme model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (model.MemeId <= 0)
            {
                throw new ArgumentException("Invalid id.");
            }

            if (string.IsNullOrWhiteSpace(model.Title))
            {
                throw new ArgumentException("Title is required.");
            }

            if (string.IsNullOrWhiteSpace(model.ImageUrl))
            {
                throw new ArgumentException("ImageUrl is required.");
            }

            var existingMeme = await _repositoryWrapper.Meme
                .FindByCondition(x => x.MemeId == model.MemeId);
            
            if (!existingMeme.Any())
            {
                throw new InvalidOperationException("Meme not found.");
            }

            if (existingMeme.Count > 1)
            {
                throw new InvalidOperationException("Multiple memes found with the same ID.");
            }
            
            await _repositoryWrapper.Meme.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid id.");
            }

            var meme = await _repositoryWrapper.Meme
                .FindByCondition(x => x.MemeId == id);
            
            if (!meme.Any())
            {
                throw new InvalidOperationException("Meme not found.");
            }

            if (meme.Count > 1)
            {
                throw new InvalidOperationException("Multiple memes found with the same ID.");
            }
    
            await _repositoryWrapper.Meme.Delete(meme.First());
            await _repositoryWrapper.Save();
        }
    }
}
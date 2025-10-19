using Domain.Interfaces;
using Domain.Interfaces.IUploadStat;
using Domain.Models;

namespace BusinessLogic.Services
{
    public class UploadStatService : IUploadStatService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public UploadStatService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<UploadStat>> GetAll()
        {
            return await _repositoryWrapper.UploadStat.FindAll();
        }

        public async Task<UploadStat> GetById(int id)
        {
            var uploadStats = await _repositoryWrapper.UploadStat
                .FindByCondition(x => x.StatId == id);

            if (!uploadStats.Any())
            {
                throw new InvalidOperationException("UploadStat not found.");
            }

            if (uploadStats.Count > 1)
            {
                throw new InvalidOperationException("Multiple upload stats found with the same ID.");
            }

            return uploadStats.First();
        }

        public async Task<UploadStat> GetByMemeId(int memeId)
        {
            var uploadStats = await _repositoryWrapper.UploadStat
                .FindByCondition(x => x.MemeId == memeId);

            if (!uploadStats.Any())
            {
                throw new InvalidOperationException("UploadStat for specified meme not found.");
            }

            if (uploadStats.Count > 1)
            {
                throw new InvalidOperationException("Multiple upload stats found for the same meme.");
            }

            return uploadStats.First();
        }

        public async Task Create(UploadStat model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (model.MemeId <= 0)
            {
                throw new ArgumentException("MemeId must be greater than 0.", nameof(model.MemeId));
            }

            var existingStats = await _repositoryWrapper.UploadStat
                .FindByCondition(x => x.MemeId == model.MemeId);

            if (existingStats.Any())
            {
                throw new InvalidOperationException("UploadStat for this meme already exists.");
            }

            model.ViewsCount ??= 0;
            model.DownloadCount ??= 0;
            model.ShareCount ??= 0;

            await _repositoryWrapper.UploadStat.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(UploadStat model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (model.MemeId <= 0)
            {
                throw new ArgumentException("MemeId must be greater than 0.", nameof(model.MemeId));
            }

            var existingStats = await _repositoryWrapper.UploadStat
                .FindByCondition(x => x.StatId == model.StatId);

            if (!existingStats.Any())
            {
                throw new InvalidOperationException("UploadStat not found.");
            }

            await _repositoryWrapper.UploadStat.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task IncrementViews(int memeId)
        {
            await IncrementStatistic(memeId, stat => stat.ViewsCount++);
        }

        public async Task IncrementDownloads(int memeId)
        {
            await IncrementStatistic(memeId, stat => stat.DownloadCount++);
        }

        public async Task IncrementShares(int memeId)
        {
            await IncrementStatistic(memeId, stat => stat.ShareCount++);
        }

        private async Task IncrementStatistic(int memeId, Action<UploadStat> incrementAction)
        {
            var uploadStats = await _repositoryWrapper.UploadStat
                .FindByCondition(x => x.MemeId == memeId);

            if (!uploadStats.Any())
            {
                throw new InvalidOperationException("UploadStat for specified meme not found.");
            }

            if (uploadStats.Count > 1)
            {
                throw new InvalidOperationException("Multiple upload stats found for the same meme.");
            }

            var uploadStat = uploadStats.First();
            incrementAction(uploadStat);

            await _repositoryWrapper.UploadStat.Update(uploadStat);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var uploadStats = await _repositoryWrapper.UploadStat
                .FindByCondition(x => x.StatId == id);

            if (!uploadStats.Any())
            {
                throw new InvalidOperationException("UploadStat not found.");
            }

            if (uploadStats.Count > 1)
            {
                throw new InvalidOperationException("Multiple upload stats found with the same ID.");
            }

            await _repositoryWrapper.UploadStat.Delete(uploadStats.First());
            await _repositoryWrapper.Save();
        }
    }
}
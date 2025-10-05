using Domain.Interfaces;
using Domain.Interfaces.IMemeMetadatum;
using Domain.Models;

namespace BusinessLogic.Services
{
    public class MemeMetadatumService : IMemeMetadatumService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public MemeMetadatumService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<MemeMetadatum>> GetAll()
        {
            return await _repositoryWrapper.MemeMetadatum.FindAll();
        }

        public async Task<MemeMetadatum> GetById(int id)
        {
            var memeMetadata = await _repositoryWrapper.MemeMetadatum
                .FindByCondition(x => x.MetadataId == id);

            if (!memeMetadata.Any())
            {
                throw new InvalidOperationException("MemeMetadatum not found.");
            }

            if (memeMetadata.Count > 1)
            {
                throw new InvalidOperationException("Multiple meme metadata found with the same ID.");
            }

            return memeMetadata.First();
        }

        public async Task<MemeMetadatum> GetByMemeId(int memeId)
        {
            var memeMetadata = await _repositoryWrapper.MemeMetadatum
                .FindByCondition(x => x.MemeId == memeId);

            if (!memeMetadata.Any())
            {
                throw new InvalidOperationException("MemeMetadatum for specified meme not found.");
            }

            if (memeMetadata.Count > 1)
            {
                throw new InvalidOperationException("Multiple meme metadata found for the same meme.");
            }

            return memeMetadata.First();
        }

        public async Task Create(MemeMetadatum model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (model.MemeId <= 0)
            {
                throw new ArgumentException("MemeId must be greater than 0.", nameof(model.MemeId));
            }

            if (model.FileSize <= 0)
            {
                throw new ArgumentException("FileSize must be greater than 0.", nameof(model.FileSize));
            }

            if (model.Width <= 0)
            {
                throw new ArgumentException("Width must be greater than 0.", nameof(model.Width));
            }

            if (model.Height <= 0)
            {
                throw new ArgumentException("Height must be greater than 0.", nameof(model.Height));
            }

            if (string.IsNullOrWhiteSpace(model.FileFormat))
            {
                throw new ArgumentException("FileFormat cannot be null, empty, or whitespace.", nameof(model.FileFormat));
            }

            if (string.IsNullOrWhiteSpace(model.MimeType))
            {
                throw new ArgumentException("MimeType cannot be null, empty, or whitespace.", nameof(model.MimeType));
            }

            var existingMetadata = await _repositoryWrapper.MemeMetadatum
                .FindByCondition(x => x.MemeId == model.MemeId);

            if (existingMetadata.Any())
            {
                throw new InvalidOperationException("MemeMetadatum for this meme already exists.");
            }

            await _repositoryWrapper.MemeMetadatum.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(MemeMetadatum model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (model.MemeId <= 0)
            {
                throw new ArgumentException("MemeId must be greater than 0.", nameof(model.MemeId));
            }

            if (model.FileSize <= 0)
            {
                throw new ArgumentException("FileSize must be greater than 0.", nameof(model.FileSize));
            }

            if (model.Width <= 0)
            {
                throw new ArgumentException("Width must be greater than 0.", nameof(model.Width));
            }

            if (model.Height <= 0)
            {
                throw new ArgumentException("Height must be greater than 0.", nameof(model.Height));
            }

            if (string.IsNullOrWhiteSpace(model.FileFormat))
            {
                throw new ArgumentException("FileFormat cannot be null, empty, or whitespace.", nameof(model.FileFormat));
            }

            if (string.IsNullOrWhiteSpace(model.MimeType))
            {
                throw new ArgumentException("MimeType cannot be null, empty, or whitespace.", nameof(model.MimeType));
            }

            var existingMetadata = await _repositoryWrapper.MemeMetadatum
                .FindByCondition(x => x.MetadataId == model.MetadataId);

            if (!existingMetadata.Any())
            {
                throw new InvalidOperationException("MemeMetadatum not found.");
            }

            await _repositoryWrapper.MemeMetadatum.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var memeMetadata = await _repositoryWrapper.MemeMetadatum
                .FindByCondition(x => x.MetadataId == id);

            if (!memeMetadata.Any())
            {
                throw new InvalidOperationException("MemeMetadatum not found.");
            }

            if (memeMetadata.Count > 1)
            {
                throw new InvalidOperationException("Multiple meme metadata found with the same ID.");
            }

            await _repositoryWrapper.MemeMetadatum.Delete(memeMetadata.First());
            await _repositoryWrapper.Save();
        }

        public async Task DeleteByMemeId(int memeId)
        {
            var memeMetadata = await _repositoryWrapper.MemeMetadatum
                .FindByCondition(x => x.MemeId == memeId);

            if (!memeMetadata.Any())
            {
                throw new InvalidOperationException("MemeMetadatum for specified meme not found.");
            }

            if (memeMetadata.Count > 1)
            {
                throw new InvalidOperationException("Multiple meme metadata found for the same meme.");
            }

            await _repositoryWrapper.MemeMetadatum.Delete(memeMetadata.First());
            await _repositoryWrapper.Save();
        }

        public async Task<bool> ExistsForMeme(int memeId)
        {
            var memeMetadata = await _repositoryWrapper.MemeMetadatum
                .FindByCondition(x => x.MemeId == memeId);

            return memeMetadata.Any();
        }

        public async Task ValidateImageDimensions(int memeId, int maxWidth, int maxHeight)
        {
            var memeMetadata = await GetByMemeId(memeId);

            if (memeMetadata.Width > maxWidth || memeMetadata.Height > maxHeight)
            {
                throw new InvalidOperationException($"Image dimensions exceed maximum allowed size. Maximum: {maxWidth}x{maxHeight}, Actual: {memeMetadata.Width}x{memeMetadata.Height}");
            }
        }

        public async Task ValidateFileSize(int memeId, long maxFileSize)
        {
            var memeMetadata = await GetByMemeId(memeId);

            if (memeMetadata.FileSize > maxFileSize)
            {
                throw new InvalidOperationException($"File size exceeds maximum allowed size. Maximum: {maxFileSize} bytes, Actual: {memeMetadata.FileSize} bytes");
            }
        }
    }
}
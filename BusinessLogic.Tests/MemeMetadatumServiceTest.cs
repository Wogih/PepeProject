using BusinessLogic.Services;
using Domain.Interfaces;
using Domain.Interfaces.IMemeMetadatum;
using Domain.Models;
using Moq;
using System.Linq.Expressions;

namespace BusinessLogic.Tests
{
    public class MemeMetadatumServiceTest
    {
        private readonly MemeMetadatumService _memeMetadatumService;
        private readonly Mock<IMemeMetadatumRepository> _mockMemeMetadatumRepository;
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;

        public MemeMetadatumServiceTest()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockMemeMetadatumRepository = new Mock<IMemeMetadatumRepository>();
            
            _mockRepositoryWrapper.Setup(x => x.MemeMetadatum).Returns(_mockMemeMetadatumRepository.Object);
            _mockRepositoryWrapper.Setup(x => x.Save()).Returns(Task.CompletedTask);
            
            _memeMetadatumService = new MemeMetadatumService(_mockRepositoryWrapper.Object);
        }

        #region GET ALL MEME METADATA TESTS

        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoMemeMetadata()
        {
            // Arrange
            var emptyList = new List<MemeMetadatum>();
            _mockMemeMetadatumRepository.Setup(x => x.FindAll()).ReturnsAsync(emptyList);
            
            // Act
            var result = await _memeMetadatumService.GetAll();
            
            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAll_ReturnsListMemeMetadata_WhenMemeMetadataExist()
        {
            // Arrange
            var memeMetadata = new List<MemeMetadatum>()
            {
                new MemeMetadatum() { MetadataId = 1, MemeId = 1, FileSize = 1024, Width = 800, Height = 600, FileFormat = "JPEG", MimeType = "image/jpeg" },
                new MemeMetadatum() { MetadataId = 2, MemeId = 2, FileSize = 2048, Width = 1024, Height = 768, FileFormat = "PNG", MimeType = "image/png" }
            };
            _mockMemeMetadatumRepository.Setup(x => x.FindAll()).ReturnsAsync(memeMetadata);
            
            // Act
            var result = await _memeMetadatumService.GetAll();
            
            // Assert
            Assert.Equal(memeMetadata, result);
        }

        [Fact]
        public async Task GetAll_ThrowsException_WhenRepositoryThrows()
        {
            // Arrange
            _mockMemeMetadatumRepository.Setup(x => x.FindAll()).ThrowsAsync(new InvalidOperationException("DB Error"));
            
            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeMetadatumService.GetAll());
        }
        #endregion

        #region GET BY ID TESTS
        [Fact]
        public async Task GetById_MemeMetadatumExists_ReturnsCorrectMemeMetadatum()
        {
            // Arrange
            var existingMemeMetadatum = new MemeMetadatum 
            { 
                MetadataId = 1, 
                MemeId = 1, 
                FileSize = 1024, 
                Width = 800, 
                Height = 600, 
                FileFormat = "JPEG", 
                MimeType = "image/jpeg" 
            };
            _mockMemeMetadatumRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeMetadatum, bool>>>()))
                .ReturnsAsync(new List<MemeMetadatum> { existingMemeMetadatum });

            // Act
            var result = await _memeMetadatumService.GetById(existingMemeMetadatum.MetadataId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingMemeMetadatum.MetadataId, result.MetadataId);
            Assert.Equal(existingMemeMetadatum.MemeId, result.MemeId);
            Assert.Equal(existingMemeMetadatum.FileFormat, result.FileFormat);
        }

        [Fact]
        public async Task GetById_MemeMetadatumDoesntExist_ThrowsInvalidOperationException()
        {
            // Arrange
            const int nonexistentMetadataId = 999;
            _mockMemeMetadatumRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeMetadatum, bool>>>()))
                .ReturnsAsync(new List<MemeMetadatum>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeMetadatumService.GetById(nonexistentMetadataId));
        }

        [Fact]
        public async Task GetById_MultipleMemeMetadataWithSameId_ThrowsInvalidOperationException()
        {
            // Arrange
            const int metadataId = 1;
            var memeMetadata = new List<MemeMetadatum>
            {
                new MemeMetadatum { MetadataId = metadataId, MemeId = 1, FileSize = 1024, Width = 800, Height = 600, FileFormat = "JPEG", MimeType = "image/jpeg" },
                new MemeMetadatum { MetadataId = metadataId, MemeId = 2, FileSize = 2048, Width = 1024, Height = 768, FileFormat = "PNG", MimeType = "image/png" }
            };
            _mockMemeMetadatumRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeMetadatum, bool>>>()))
                .ReturnsAsync(memeMetadata);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeMetadatumService.GetById(metadataId));
        }

        [Fact]
        public async Task GetById_ThrowsException_WhenRepositoryThrows()
        {
            // Arrange
            const int metadataId = 1;
            _mockMemeMetadatumRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeMetadatum, bool>>>()))
                .ThrowsAsync(new InvalidOperationException("DB Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeMetadatumService.GetById(metadataId));
        }
        #endregion

        #region GET BY MEME ID TESTS
        [Fact]
        public async Task GetByMemeId_MemeMetadatumExists_ReturnsCorrectMemeMetadatum()
        {
            // Arrange
            const int memeId = 1;
            var existingMemeMetadatum = new MemeMetadatum 
            { 
                MetadataId = 1, 
                MemeId = memeId, 
                FileSize = 1024, 
                Width = 800, 
                Height = 600, 
                FileFormat = "JPEG", 
                MimeType = "image/jpeg" 
            };
            _mockMemeMetadatumRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeMetadatum, bool>>>()))
                .ReturnsAsync(new List<MemeMetadatum> { existingMemeMetadatum });

            // Act
            var result = await _memeMetadatumService.GetByMemeId(memeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(memeId, result.MemeId);
            Assert.Equal(existingMemeMetadatum.FileFormat, result.FileFormat);
        }

        [Fact]
        public async Task GetByMemeId_MemeMetadatumDoesntExist_ThrowsInvalidOperationException()
        {
            // Arrange
            const int nonexistentMemeId = 999;
            _mockMemeMetadatumRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeMetadatum, bool>>>()))
                .ReturnsAsync(new List<MemeMetadatum>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeMetadatumService.GetByMemeId(nonexistentMemeId));
        }

        [Fact]
        public async Task GetByMemeId_MultipleMemeMetadataForSameMeme_ThrowsInvalidOperationException()
        {
            // Arrange
            const int memeId = 1;
            var memeMetadata = new List<MemeMetadatum>
            {
                new MemeMetadatum { MetadataId = 1, MemeId = memeId, FileSize = 1024, Width = 800, Height = 600, FileFormat = "JPEG", MimeType = "image/jpeg" },
                new MemeMetadatum { MetadataId = 2, MemeId = memeId, FileSize = 2048, Width = 1024, Height = 768, FileFormat = "PNG", MimeType = "image/png" }
            };
            _mockMemeMetadatumRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeMetadatum, bool>>>()))
                .ReturnsAsync(memeMetadata);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeMetadatumService.GetByMemeId(memeId));
        }
        #endregion

        #region CREATE MEME METADATUM TESTS
        [Fact]
        public async Task Create_NullMemeMetadatum_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _memeMetadatumService.Create(null));
            _mockMemeMetadatumRepository.Verify(x => x.Create(It.IsAny<MemeMetadatum>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectMemeMetadata))]
        public async Task Create_IncorrectMemeMetadatum_ThrowsArgumentException(MemeMetadatum memeMetadatum)
        {
            // Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _memeMetadatumService.Create(memeMetadatum));

            // Assert
            _mockMemeMetadatumRepository.Verify(x => x.Create(It.IsAny<MemeMetadatum>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Theory]
        [MemberData(nameof(GetWhitespaceMemeMetadata))]
        public async Task Create_WhitespaceMemeMetadatum_ThrowsArgumentException(MemeMetadatum memeMetadatum)
        {
            // Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _memeMetadatumService.Create(memeMetadatum));

            // Assert
            _mockMemeMetadatumRepository.Verify(x => x.Create(It.IsAny<MemeMetadatum>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task Create_MemeMetadatumForExistingMeme_ThrowsInvalidOperationException()
        {
            // Arrange
            var existingMemeMetadatum = new MemeMetadatum { MetadataId = 1, MemeId = 1, FileSize = 1024, Width = 800, Height = 600, FileFormat = "JPEG", MimeType = "image/jpeg" };
            var newMemeMetadatum = new MemeMetadatum { MemeId = 1, FileSize = 2048, Width = 1024, Height = 768, FileFormat = "PNG", MimeType = "image/png" };
            
            _mockMemeMetadatumRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeMetadatum, bool>>>()))
                .ReturnsAsync(new List<MemeMetadatum> { existingMemeMetadatum });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeMetadatumService.Create(newMemeMetadatum));
            _mockMemeMetadatumRepository.Verify(x => x.Create(It.IsAny<MemeMetadatum>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_CorrectMemeMetadatum_CallsCreateAndSave()
        {
            // Arrange
            var validMemeMetadatum = new MemeMetadatum 
            { 
                MemeId = 1, 
                FileSize = 1024, 
                Width = 800, 
                Height = 600, 
                FileFormat = "JPEG", 
                MimeType = "image/jpeg" 
            };
            _mockMemeMetadatumRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeMetadatum, bool>>>()))
                .ReturnsAsync(new List<MemeMetadatum>());

            // Act
            await _memeMetadatumService.Create(validMemeMetadatum);

            // Assert
            _mockMemeMetadatumRepository.Verify(x => x.Create(validMemeMetadatum), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Create_ThrowsException_WhenRepositoryCreateThrows()
        {
            // Arrange
            var validMemeMetadatum = new MemeMetadatum 
            { 
                MemeId = 1, 
                FileSize = 1024, 
                Width = 800, 
                Height = 600, 
                FileFormat = "JPEG", 
                MimeType = "image/jpeg" 
            };
            _mockMemeMetadatumRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeMetadatum, bool>>>()))
                .ReturnsAsync(new List<MemeMetadatum>());
            _mockMemeMetadatumRepository.Setup(x => x.Create(validMemeMetadatum)).ThrowsAsync(new InvalidOperationException("DB Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeMetadatumService.Create(validMemeMetadatum));
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_ThrowsException_WhenSaveThrows()
        {
            // Arrange
            var validMemeMetadatum = new MemeMetadatum 
            { 
                MemeId = 1, 
                FileSize = 1024, 
                Width = 800, 
                Height = 600, 
                FileFormat = "JPEG", 
                MimeType = "image/jpeg" 
            };
            _mockMemeMetadatumRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeMetadatum, bool>>>()))
                .ReturnsAsync(new List<MemeMetadatum>());
            _mockRepositoryWrapper.Setup(x => x.Save()).ThrowsAsync(new InvalidOperationException("Save Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeMetadatumService.Create(validMemeMetadatum));
        }
        #endregion

        #region UPDATE MEME METADATUM TESTS
        [Fact]
        public async Task Update_NullMemeMetadatum_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _memeMetadatumService.Update(null));
            _mockMemeMetadatumRepository.Verify(x => x.Update(It.IsAny<MemeMetadatum>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectMemeMetadata))]
        public async Task Update_IncorrectMemeMetadatum_ThrowsArgumentException(MemeMetadatum memeMetadatum)
        {
            // Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _memeMetadatumService.Update(memeMetadatum));

            // Assert
            _mockMemeMetadatumRepository.Verify(x => x.Update(It.IsAny<MemeMetadatum>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task Update_NonExistentMemeMetadatum_ThrowsInvalidOperationException()
        {
            // Arrange
            var memeMetadatum = new MemeMetadatum 
            { 
                MetadataId = 1, 
                MemeId = 1, 
                FileSize = 1024, 
                Width = 800, 
                Height = 600, 
                FileFormat = "JPEG", 
                MimeType = "image/jpeg" 
            };
            _mockMemeMetadatumRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeMetadatum, bool>>>()))
                .ReturnsAsync(new List<MemeMetadatum>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeMetadatumService.Update(memeMetadatum));
            _mockMemeMetadatumRepository.Verify(x => x.Update(It.IsAny<MemeMetadatum>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Update_ValidMemeMetadatum_CallsUpdateAndSave()
        {
            // Arrange
            var updatedMemeMetadatum = new MemeMetadatum 
            { 
                MetadataId = 1, 
                MemeId = 1, 
                FileSize = 2048, 
                Width = 1024, 
                Height = 768, 
                FileFormat = "PNG", 
                MimeType = "image/png" 
            };
            _mockMemeMetadatumRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeMetadatum, bool>>>()))
                .ReturnsAsync(new List<MemeMetadatum> { updatedMemeMetadatum });

            // Act
            await _memeMetadatumService.Update(updatedMemeMetadatum);

            // Assert
            _mockMemeMetadatumRepository.Verify(x => x.Update(updatedMemeMetadatum), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }
        #endregion

        #region VALIDATION TESTS
        [Fact]
        public async Task ValidateImageDimensions_ValidDimensions_DoesNotThrow()
        {
            // Arrange
            const int memeId = 1;
            const int maxWidth = 1920;
            const int maxHeight = 1080;
            var memeMetadatum = new MemeMetadatum 
            { 
                MetadataId = 1, 
                MemeId = memeId, 
                FileSize = 1024, 
                Width = 800, 
                Height = 600, 
                FileFormat = "JPEG", 
                MimeType = "image/jpeg" 
            };
            _mockMemeMetadatumRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeMetadatum, bool>>>()))
                .ReturnsAsync(new List<MemeMetadatum> { memeMetadatum });

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _memeMetadatumService.ValidateImageDimensions(memeId, maxWidth, maxHeight));
            Assert.Null(exception);
        }

        [Fact]
        public async Task ValidateImageDimensions_InvalidDimensions_ThrowsInvalidOperationException()
        {
            // Arrange
            const int memeId = 1;
            const int maxWidth = 800;
            const int maxHeight = 600;
            var memeMetadatum = new MemeMetadatum 
            { 
                MetadataId = 1, 
                MemeId = memeId, 
                FileSize = 1024, 
                Width = 1024, 
                Height = 768, 
                FileFormat = "JPEG", 
                MimeType = "image/jpeg" 
            };
            _mockMemeMetadatumRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeMetadatum, bool>>>()))
                .ReturnsAsync(new List<MemeMetadatum> { memeMetadatum });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeMetadatumService.ValidateImageDimensions(memeId, maxWidth, maxHeight));
        }

        [Fact]
        public async Task ValidateFileSize_ValidFileSize_DoesNotThrow()
        {
            // Arrange
            const int memeId = 1;
            const long maxFileSize = 2048;
            var memeMetadatum = new MemeMetadatum 
            { 
                MetadataId = 1, 
                MemeId = memeId, 
                FileSize = 1024, 
                Width = 800, 
                Height = 600, 
                FileFormat = "JPEG", 
                MimeType = "image/jpeg" 
            };
            _mockMemeMetadatumRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeMetadatum, bool>>>()))
                .ReturnsAsync(new List<MemeMetadatum> { memeMetadatum });

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _memeMetadatumService.ValidateFileSize(memeId, maxFileSize));
            Assert.Null(exception);
        }

        [Fact]
        public async Task ValidateFileSize_InvalidFileSize_ThrowsInvalidOperationException()
        {
            // Arrange
            const int memeId = 1;
            const long maxFileSize = 512;
            var memeMetadatum = new MemeMetadatum 
            { 
                MetadataId = 1, 
                MemeId = memeId, 
                FileSize = 1024, 
                Width = 800, 
                Height = 600, 
                FileFormat = "JPEG", 
                MimeType = "image/jpeg" 
            };
            _mockMemeMetadatumRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeMetadatum, bool>>>()))
                .ReturnsAsync(new List<MemeMetadatum> { memeMetadatum });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeMetadatumService.ValidateFileSize(memeId, maxFileSize));
        }

        [Fact]
        public async Task ExistsForMeme_ReturnsTrue_WhenMetadataExists()
        {
            // Arrange
            const int memeId = 1;
            var memeMetadatum = new MemeMetadatum { MetadataId = 1, MemeId = memeId };
            _mockMemeMetadatumRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeMetadatum, bool>>>()))
                .ReturnsAsync(new List<MemeMetadatum> { memeMetadatum });

            // Act
            var result = await _memeMetadatumService.ExistsForMeme(memeId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ExistsForMeme_ReturnsFalse_WhenNoMetadataExists()
        {
            // Arrange
            const int memeId = 999;
            _mockMemeMetadatumRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeMetadatum, bool>>>()))
                .ReturnsAsync(new List<MemeMetadatum>());

            // Act
            var result = await _memeMetadatumService.ExistsForMeme(memeId);

            // Assert
            Assert.False(result);
        }
        #endregion

        #region DELETE TESTS
        [Fact]
        public async Task DeleteByMemeId_MemeMetadatumExists_CallsDeleteAndSave()
        {
            // Arrange
            const int memeId = 1;
            var existingMemeMetadatum = new MemeMetadatum { MetadataId = 1, MemeId = memeId };
            _mockMemeMetadatumRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeMetadatum, bool>>>()))
                .ReturnsAsync(new List<MemeMetadatum> { existingMemeMetadatum });

            // Act
            await _memeMetadatumService.DeleteByMemeId(memeId);

            // Assert
            _mockMemeMetadatumRepository.Verify(x => x.Delete(existingMemeMetadatum), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }
        #endregion

        public static IEnumerable<object[]> GetIncorrectMemeMetadata()
        {
            return new List<object[]>
            {
                new object[] { new MemeMetadatum() { MemeId = 0, FileSize = 1024, Width = 800, Height = 600, FileFormat = "JPEG", MimeType = "image/jpeg" } },
                new object[] { new MemeMetadatum() { MemeId = -1, FileSize = 1024, Width = 800, Height = 600, FileFormat = "JPEG", MimeType = "image/jpeg" } },
                new object[] { new MemeMetadatum() { MemeId = 1, FileSize = 0, Width = 800, Height = 600, FileFormat = "JPEG", MimeType = "image/jpeg" } },
                new object[] { new MemeMetadatum() { MemeId = 1, FileSize = -1, Width = 800, Height = 600, FileFormat = "JPEG", MimeType = "image/jpeg" } },
                new object[] { new MemeMetadatum() { MemeId = 1, FileSize = 1024, Width = 0, Height = 600, FileFormat = "JPEG", MimeType = "image/jpeg" } },
                new object[] { new MemeMetadatum() { MemeId = 1, FileSize = 1024, Width = -1, Height = 600, FileFormat = "JPEG", MimeType = "image/jpeg" } },
                new object[] { new MemeMetadatum() { MemeId = 1, FileSize = 1024, Width = 800, Height = 0, FileFormat = "JPEG", MimeType = "image/jpeg" } },
                new object[] { new MemeMetadatum() { MemeId = 1, FileSize = 1024, Width = 800, Height = -1, FileFormat = "JPEG", MimeType = "image/jpeg" } },
                new object[] { new MemeMetadatum() { MemeId = 1, FileSize = 1024, Width = 800, Height = 600, FileFormat = "", MimeType = "image/jpeg" } },
                new object[] { new MemeMetadatum() { MemeId = 1, FileSize = 1024, Width = 800, Height = 600, FileFormat = null, MimeType = "image/jpeg" } },
                new object[] { new MemeMetadatum() { MemeId = 1, FileSize = 1024, Width = 800, Height = 600, FileFormat = "JPEG", MimeType = "" } },
                new object[] { new MemeMetadatum() { MemeId = 1, FileSize = 1024, Width = 800, Height = 600, FileFormat = "JPEG", MimeType = null } }
            };
        }

        public static IEnumerable<object[]> GetWhitespaceMemeMetadata()
        {
            return new List<object[]>
            {
                new object[] { new MemeMetadatum() { MemeId = 1, FileSize = 1024, Width = 800, Height = 600, FileFormat = " ", MimeType = "image/jpeg" } },
                new object[] { new MemeMetadatum() { MemeId = 1, FileSize = 1024, Width = 800, Height = 600, FileFormat = "   ", MimeType = "image/jpeg" } },
                new object[] { new MemeMetadatum() { MemeId = 1, FileSize = 1024, Width = 800, Height = 600, FileFormat = "JPEG", MimeType = " " } },
                new object[] { new MemeMetadatum() { MemeId = 1, FileSize = 1024, Width = 800, Height = 600, FileFormat = "JPEG", MimeType = "   " } }
            };
        }
    }
}
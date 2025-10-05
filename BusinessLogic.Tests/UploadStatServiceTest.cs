using BusinessLogic.Services;
using Domain.Interfaces;
using Domain.Interfaces.IUploadStat;
using Domain.Models;
using Moq;
using System.Linq.Expressions;

namespace BusinessLogic.Tests
{
    public class UploadStatServiceTest
    {
        private readonly UploadStatService _uploadStatService;
        private readonly Mock<IUploadStatRepository> _mockUploadStatRepository;
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;

        public UploadStatServiceTest()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockUploadStatRepository = new Mock<IUploadStatRepository>();

            _mockRepositoryWrapper.Setup(x => x.UploadStat).Returns(_mockUploadStatRepository.Object);
            _mockRepositoryWrapper.Setup(x => x.Save()).Returns(Task.CompletedTask);

            _uploadStatService = new UploadStatService(_mockRepositoryWrapper.Object);
        }

        #region GET ALL UPLOAD STATS TESTS

        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoUploadStats()
        {
            // Arrange
            var emptyList = new List<UploadStat>();
            _mockUploadStatRepository.Setup(x => x.FindAll()).ReturnsAsync(emptyList);

            // Act
            var result = await _uploadStatService.GetAll();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAll_ReturnsListUploadStats_WhenUploadStatsExist()
        {
            // Arrange
            var uploadStats = new List<UploadStat>()
            {
                new UploadStat() { StatId = 1, MemeId = 1, ViewsCount = 100, DownloadCount = 10, ShareCount = 5 },
                new UploadStat() { StatId = 2, MemeId = 2, ViewsCount = 200, DownloadCount = 20, ShareCount = 10 }
            };
            _mockUploadStatRepository.Setup(x => x.FindAll()).ReturnsAsync(uploadStats);

            // Act
            var result = await _uploadStatService.GetAll();

            // Assert
            Assert.Equal(uploadStats, result);
        }

        [Fact]
        public async Task GetAll_ThrowsException_WhenRepositoryThrows()
        {
            // Arrange
            _mockUploadStatRepository.Setup(x => x.FindAll()).ThrowsAsync(new InvalidOperationException("DB Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _uploadStatService.GetAll());
        }
        #endregion

        #region GET BY ID TESTS
        [Fact]
        public async Task GetById_UploadStatExists_ReturnsCorrectUploadStat()
        {
            // Arrange
            var existingUploadStat = new UploadStat { StatId = 1, MemeId = 1, ViewsCount = 100, DownloadCount = 10, ShareCount = 5 };
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ReturnsAsync(new List<UploadStat> { existingUploadStat });

            // Act
            var result = await _uploadStatService.GetById(existingUploadStat.StatId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingUploadStat.StatId, result.StatId);
            Assert.Equal(existingUploadStat.MemeId, result.MemeId);
            Assert.Equal(existingUploadStat.ViewsCount, result.ViewsCount);
        }

        [Fact]
        public async Task GetById_UploadStatDoesntExist_ThrowsInvalidOperationException()
        {
            // Arrange
            const int nonexistentStatId = 999;
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ReturnsAsync(new List<UploadStat>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _uploadStatService.GetById(nonexistentStatId));
        }

        [Fact]
        public async Task GetById_MultipleUploadStatsWithSameId_ThrowsInvalidOperationException()
        {
            // Arrange
            const int statId = 1;
            var uploadStats = new List<UploadStat>
            {
                new UploadStat { StatId = statId, MemeId = 1, ViewsCount = 100 },
                new UploadStat { StatId = statId, MemeId = 2, ViewsCount = 200 }
            };
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ReturnsAsync(uploadStats);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _uploadStatService.GetById(statId));
        }

        [Fact]
        public async Task GetById_ThrowsException_WhenRepositoryThrows()
        {
            // Arrange
            const int statId = 1;
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ThrowsAsync(new InvalidOperationException("DB Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _uploadStatService.GetById(statId));
        }
        #endregion

        #region GET BY MEME ID TESTS
        [Fact]
        public async Task GetByMemeId_UploadStatExists_ReturnsCorrectUploadStat()
        {
            // Arrange
            const int memeId = 1;
            var existingUploadStat = new UploadStat { StatId = 1, MemeId = memeId, ViewsCount = 100, DownloadCount = 10, ShareCount = 5 };
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ReturnsAsync(new List<UploadStat> { existingUploadStat });

            // Act
            var result = await _uploadStatService.GetByMemeId(memeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(memeId, result.MemeId);
            Assert.Equal(existingUploadStat.ViewsCount, result.ViewsCount);
        }

        [Fact]
        public async Task GetByMemeId_UploadStatDoesntExist_ThrowsInvalidOperationException()
        {
            // Arrange
            const int nonexistentMemeId = 999;
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ReturnsAsync(new List<UploadStat>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _uploadStatService.GetByMemeId(nonexistentMemeId));
        }

        [Fact]
        public async Task GetByMemeId_MultipleUploadStatsForSameMeme_ThrowsInvalidOperationException()
        {
            // Arrange
            const int memeId = 1;
            var uploadStats = new List<UploadStat>
            {
                new UploadStat { StatId = 1, MemeId = memeId, ViewsCount = 100 },
                new UploadStat { StatId = 2, MemeId = memeId, ViewsCount = 200 }
            };
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ReturnsAsync(uploadStats);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _uploadStatService.GetByMemeId(memeId));
        }
        #endregion

        #region CREATE UPLOAD STAT TESTS
        [Fact]
        public async Task Create_NullUploadStat_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _uploadStatService.Create(null));
            _mockUploadStatRepository.Verify(x => x.Create(It.IsAny<UploadStat>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectUploadStats))]
        public async Task Create_IncorrectUploadStat_ThrowsArgumentException(UploadStat uploadStat)
        {
            // Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _uploadStatService.Create(uploadStat));

            // Assert
            _mockUploadStatRepository.Verify(x => x.Create(It.IsAny<UploadStat>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task Create_UploadStatForExistingMeme_ThrowsInvalidOperationException()
        {
            // Arrange
            var existingUploadStat = new UploadStat { StatId = 1, MemeId = 1, ViewsCount = 100 };
            var newUploadStat = new UploadStat { MemeId = 1, ViewsCount = 0 };

            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ReturnsAsync(new List<UploadStat> { existingUploadStat });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _uploadStatService.Create(newUploadStat));
            _mockUploadStatRepository.Verify(x => x.Create(It.IsAny<UploadStat>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_CorrectUploadStat_SetsDefaultValuesAndCallsCreateAndSave()
        {
            // Arrange
            var validUploadStat = new UploadStat { MemeId = 1 }; // Nullable values not set
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ReturnsAsync(new List<UploadStat>());

            // Act
            await _uploadStatService.Create(validUploadStat);

            // Assert
            Assert.Equal(0, validUploadStat.ViewsCount);
            Assert.Equal(0, validUploadStat.DownloadCount);
            Assert.Equal(0, validUploadStat.ShareCount);
            _mockUploadStatRepository.Verify(x => x.Create(validUploadStat), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Create_CorrectUploadStatWithValues_CallsCreateAndSave()
        {
            // Arrange
            var validUploadStat = new UploadStat { MemeId = 1, ViewsCount = 50, DownloadCount = 5, ShareCount = 2 };
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ReturnsAsync(new List<UploadStat>());

            // Act
            await _uploadStatService.Create(validUploadStat);

            // Assert
            _mockUploadStatRepository.Verify(x => x.Create(validUploadStat), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Create_ThrowsException_WhenRepositoryCreateThrows()
        {
            // Arrange
            var validUploadStat = new UploadStat { MemeId = 1 };
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ReturnsAsync(new List<UploadStat>());
            _mockUploadStatRepository.Setup(x => x.Create(validUploadStat)).ThrowsAsync(new InvalidOperationException("DB Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _uploadStatService.Create(validUploadStat));
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_ThrowsException_WhenSaveThrows()
        {
            // Arrange
            var validUploadStat = new UploadStat { MemeId = 1 };
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ReturnsAsync(new List<UploadStat>());
            _mockRepositoryWrapper.Setup(x => x.Save()).ThrowsAsync(new InvalidOperationException("Save Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _uploadStatService.Create(validUploadStat));
        }
        #endregion

        #region UPDATE UPLOAD STAT TESTS
        [Fact]
        public async Task Update_NullUploadStat_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _uploadStatService.Update(null));
            _mockUploadStatRepository.Verify(x => x.Update(It.IsAny<UploadStat>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectUploadStats))]
        public async Task Update_IncorrectUploadStat_ThrowsArgumentException(UploadStat uploadStat)
        {
            // Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _uploadStatService.Update(uploadStat));

            // Assert
            _mockUploadStatRepository.Verify(x => x.Update(It.IsAny<UploadStat>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task Update_NonExistentUploadStat_ThrowsInvalidOperationException()
        {
            // Arrange
            var uploadStat = new UploadStat { StatId = 1, MemeId = 1, ViewsCount = 100 };
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ReturnsAsync(new List<UploadStat>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _uploadStatService.Update(uploadStat));
            _mockUploadStatRepository.Verify(x => x.Update(It.IsAny<UploadStat>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Update_ValidUploadStat_CallsUpdateAndSave()
        {
            // Arrange
            var updatedUploadStat = new UploadStat { StatId = 1, MemeId = 1, ViewsCount = 150, DownloadCount = 15, ShareCount = 8 };
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ReturnsAsync(new List<UploadStat> { updatedUploadStat });

            // Act
            await _uploadStatService.Update(updatedUploadStat);

            // Assert
            _mockUploadStatRepository.Verify(x => x.Update(updatedUploadStat), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_ThrowsException_WhenRepositoryUpdateThrows()
        {
            // Arrange
            var updatedUploadStat = new UploadStat { StatId = 1, MemeId = 1, ViewsCount = 150 };
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ReturnsAsync(new List<UploadStat> { updatedUploadStat });
            _mockUploadStatRepository.Setup(x => x.Update(updatedUploadStat)).ThrowsAsync(new InvalidOperationException("DB Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _uploadStatService.Update(updatedUploadStat));
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Update_ThrowsException_WhenSaveThrows()
        {
            // Arrange
            var updatedUploadStat = new UploadStat { StatId = 1, MemeId = 1, ViewsCount = 150 };
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ReturnsAsync(new List<UploadStat> { updatedUploadStat });
            _mockRepositoryWrapper.Setup(x => x.Save()).ThrowsAsync(new InvalidOperationException("Save Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _uploadStatService.Update(updatedUploadStat));
        }
        #endregion

        #region INCREMENT STATISTICS TESTS
        [Fact]
        public async Task IncrementViews_ValidMemeId_IncrementsViewsCount()
        {
            // Arrange
            const int memeId = 1;
            var uploadStat = new UploadStat { StatId = 1, MemeId = memeId, ViewsCount = 100, DownloadCount = 10, ShareCount = 5 };
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ReturnsAsync(new List<UploadStat> { uploadStat });

            // Act
            await _uploadStatService.IncrementViews(memeId);

            // Assert
            Assert.Equal(101, uploadStat.ViewsCount);
            _mockUploadStatRepository.Verify(x => x.Update(uploadStat), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task IncrementDownloads_ValidMemeId_IncrementsDownloadCount()
        {
            // Arrange
            const int memeId = 1;
            var uploadStat = new UploadStat { StatId = 1, MemeId = memeId, ViewsCount = 100, DownloadCount = 10, ShareCount = 5 };
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ReturnsAsync(new List<UploadStat> { uploadStat });

            // Act
            await _uploadStatService.IncrementDownloads(memeId);

            // Assert
            Assert.Equal(11, uploadStat.DownloadCount);
            _mockUploadStatRepository.Verify(x => x.Update(uploadStat), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task IncrementShares_ValidMemeId_IncrementsShareCount()
        {
            // Arrange
            const int memeId = 1;
            var uploadStat = new UploadStat { StatId = 1, MemeId = memeId, ViewsCount = 100, DownloadCount = 10, ShareCount = 5 };
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ReturnsAsync(new List<UploadStat> { uploadStat });

            // Act
            await _uploadStatService.IncrementShares(memeId);

            // Assert
            Assert.Equal(6, uploadStat.ShareCount);
            _mockUploadStatRepository.Verify(x => x.Update(uploadStat), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task IncrementViews_NonExistentMemeId_ThrowsInvalidOperationException()
        {
            // Arrange
            const int memeId = 999;
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ReturnsAsync(new List<UploadStat>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _uploadStatService.IncrementViews(memeId));
            _mockUploadStatRepository.Verify(x => x.Update(It.IsAny<UploadStat>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }
        #endregion

        #region DELETE UPLOAD STAT TESTS
        [Fact]
        public async Task Delete_UploadStatExists_CallsDeleteAndSave()
        {
            // Arrange
            int deleteStatId = 1;
            var existingUploadStat = new UploadStat { StatId = deleteStatId, MemeId = 1, ViewsCount = 100 };
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ReturnsAsync(new List<UploadStat> { existingUploadStat });

            // Act
            await _uploadStatService.Delete(deleteStatId);

            // Assert
            _mockUploadStatRepository.Verify(x => x.Delete(existingUploadStat), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_UploadStatDoesntExist_ThrowsInvalidOperationException()
        {
            // Arrange
            const int nonexistentStatId = 999;
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ReturnsAsync(new List<UploadStat>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _uploadStatService.Delete(nonexistentStatId));
            _mockUploadStatRepository.Verify(x => x.Delete(It.IsAny<UploadStat>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_MultipleUploadStatsWithSameId_ThrowsInvalidOperationException()
        {
            // Arrange
            const int statId = 1;
            var uploadStats = new List<UploadStat>
            {
                new UploadStat { StatId = statId, MemeId = 1, ViewsCount = 100 },
                new UploadStat { StatId = statId, MemeId = 2, ViewsCount = 200 }
            };
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ReturnsAsync(uploadStats);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _uploadStatService.Delete(statId));
            _mockUploadStatRepository.Verify(x => x.Delete(It.IsAny<UploadStat>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_ThrowsException_WhenFindByConditionThrows()
        {
            // Arrange
            const int statId = 1;
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ThrowsAsync(new InvalidOperationException("DB Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _uploadStatService.Delete(statId));
            _mockUploadStatRepository.Verify(x => x.Delete(It.IsAny<UploadStat>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_ThrowsException_WhenDeleteThrows()
        {
            // Arrange
            int deleteStatId = 1;
            var existingUploadStat = new UploadStat { StatId = deleteStatId, MemeId = 1, ViewsCount = 100 };
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ReturnsAsync(new List<UploadStat> { existingUploadStat });
            _mockUploadStatRepository.Setup(x => x.Delete(existingUploadStat)).ThrowsAsync(new InvalidOperationException("Delete Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _uploadStatService.Delete(deleteStatId));
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_ThrowsException_WhenSaveThrows()
        {
            // Arrange
            int deleteStatId = 1;
            var existingUploadStat = new UploadStat { StatId = deleteStatId, MemeId = 1, ViewsCount = 100 };
            _mockUploadStatRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UploadStat, bool>>>()))
                .ReturnsAsync(new List<UploadStat> { existingUploadStat });
            _mockRepositoryWrapper.Setup(x => x.Save()).ThrowsAsync(new InvalidOperationException("Save Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _uploadStatService.Delete(deleteStatId));
        }
        #endregion

        public static IEnumerable<object[]> GetIncorrectUploadStats()
        {
            return new List<object[]>
            {
                new object[] { new UploadStat() { MemeId = 0 } },
                new object[] { new UploadStat() { MemeId = -1 } }
            };
        }
    }
}
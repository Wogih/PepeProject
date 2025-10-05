using BusinessLogic.Services;
using Domain.Interfaces;
using Domain.Interfaces.ICollection;
using Domain.Interfaces.ICollectionMeme;
using Domain.Interfaces.IMeme;
using Domain.Models;
using Moq;
using System.Linq.Expressions;

namespace BusinessLogic.Tests
{
    public class CollectionMemeServiceTest
    {
        private readonly CollectionMemeService _collectionMemeService;
        private readonly Mock<ICollectionMemeRepository> _mockCollectionMemeRepository;
        private readonly Mock<ICollectionRepository> _mockCollectionRepository;
        private readonly Mock<IMemeRepository> _mockMemeRepository;
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;

        public CollectionMemeServiceTest()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockCollectionMemeRepository = new Mock<ICollectionMemeRepository>();
            _mockCollectionRepository = new Mock<ICollectionRepository>();
            _mockMemeRepository = new Mock<IMemeRepository>();
            
            _mockRepositoryWrapper.Setup(x => x.CollectionMeme).Returns(_mockCollectionMemeRepository.Object);
            _mockRepositoryWrapper.Setup(x => x.Collection).Returns(_mockCollectionRepository.Object);
            _mockRepositoryWrapper.Setup(x => x.Meme).Returns(_mockMemeRepository.Object);
            _mockRepositoryWrapper.Setup(x => x.Save()).Returns(Task.CompletedTask);
            
            _collectionMemeService = new CollectionMemeService(_mockRepositoryWrapper.Object);
        }

        #region GET ALL COLLECTION MEMES TESTS

        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoCollectionMemes()
        {
            // Arrange
            var emptyList = new List<CollectionMeme>();
            _mockCollectionMemeRepository.Setup(x => x.FindAll()).ReturnsAsync(emptyList);
            
            // Act
            var result = await _collectionMemeService.GetAll();
            
            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAll_ReturnsListCollectionMemes_WhenCollectionMemesExist()
        {
            // Arrange
            var collectionMemes = new List<CollectionMeme>()
            {
                new CollectionMeme() { CollectionMemeId = 1, CollectionId = 1, MemeId = 1 },
                new CollectionMeme() { CollectionMemeId = 2, CollectionId = 1, MemeId = 2 }
            };
            _mockCollectionMemeRepository.Setup(x => x.FindAll()).ReturnsAsync(collectionMemes);
            
            // Act
            var result = await _collectionMemeService.GetAll();
            
            // Assert
            Assert.Equal(collectionMemes, result);
        }

        [Fact]
        public async Task GetAll_ThrowsException_WhenRepositoryThrows()
        {
            // Arrange
            _mockCollectionMemeRepository.Setup(x => x.FindAll()).ThrowsAsync(new InvalidOperationException("DB Error"));
            
            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _collectionMemeService.GetAll());
        }
        #endregion

        #region GET BY ID TESTS
        [Fact]
        public async Task GetById_CollectionMemeExists_ReturnsCorrectCollectionMeme()
        {
            // Arrange
            var existingCollectionMeme = new CollectionMeme { CollectionMemeId = 1, CollectionId = 1, MemeId = 1 };
            _mockCollectionMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<CollectionMeme, bool>>>()))
                .ReturnsAsync(new List<CollectionMeme> { existingCollectionMeme });

            // Act
            var result = await _collectionMemeService.GetById(existingCollectionMeme.CollectionMemeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingCollectionMeme.CollectionMemeId, result.CollectionMemeId);
            Assert.Equal(existingCollectionMeme.CollectionId, result.CollectionId);
            Assert.Equal(existingCollectionMeme.MemeId, result.MemeId);
        }

        [Fact]
        public async Task GetById_CollectionMemeDoesntExist_ThrowsInvalidOperationException()
        {
            // Arrange
            const int nonexistentCollectionMemeId = 999;
            _mockCollectionMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<CollectionMeme, bool>>>()))
                .ReturnsAsync(new List<CollectionMeme>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _collectionMemeService.GetById(nonexistentCollectionMemeId));
        }
        #endregion

        #region GET BY COLLECTION ID TESTS
        [Fact]
        public async Task GetByCollectionId_ReturnsCollectionMemes_WhenTheyExist()
        {
            // Arrange
            const int collectionId = 1;
            var collectionMemes = new List<CollectionMeme>
            {
                new CollectionMeme { CollectionMemeId = 1, CollectionId = collectionId, MemeId = 1 },
                new CollectionMeme { CollectionMemeId = 2, CollectionId = collectionId, MemeId = 2 }
            };
            _mockCollectionMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<CollectionMeme, bool>>>()))
                .ReturnsAsync(collectionMemes);

            // Act
            var result = await _collectionMemeService.GetByCollectionId(collectionId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, cm => Assert.Equal(collectionId, cm.CollectionId));
        }
        #endregion

        #region GET BY MEME ID TESTS
        [Fact]
        public async Task GetByMemeId_ReturnsCollectionMemes_WhenTheyExist()
        {
            // Arrange
            const int memeId = 1;
            var collectionMemes = new List<CollectionMeme>
            {
                new CollectionMeme { CollectionMemeId = 1, CollectionId = 1, MemeId = memeId },
                new CollectionMeme { CollectionMemeId = 2, CollectionId = 2, MemeId = memeId }
            };
            _mockCollectionMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<CollectionMeme, bool>>>()))
                .ReturnsAsync(collectionMemes);

            // Act
            var result = await _collectionMemeService.GetByMemeId(memeId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, cm => Assert.Equal(memeId, cm.MemeId));
        }
        #endregion

        #region GET BY COLLECTION AND MEME TESTS
        [Fact]
        public async Task GetByCollectionAndMeme_Exists_ReturnsCollectionMeme()
        {
            // Arrange
            const int collectionId = 1;
            const int memeId = 1;
            var existingCollectionMeme = new CollectionMeme { CollectionMemeId = 1, CollectionId = collectionId, MemeId = memeId };
            _mockCollectionMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<CollectionMeme, bool>>>()))
                .ReturnsAsync(new List<CollectionMeme> { existingCollectionMeme });

            // Act
            var result = await _collectionMemeService.GetByCollectionAndMeme(collectionId, memeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(collectionId, result.CollectionId);
            Assert.Equal(memeId, result.MemeId);
        }

        [Fact]
        public async Task GetByCollectionAndMeme_DoesntExist_ThrowsInvalidOperationException()
        {
            // Arrange
            const int collectionId = 999;
            const int memeId = 999;
            _mockCollectionMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<CollectionMeme, bool>>>()))
                .ReturnsAsync(new List<CollectionMeme>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _collectionMemeService.GetByCollectionAndMeme(collectionId, memeId));
        }
        #endregion

        #region EXISTS IN COLLECTION TESTS
        [Fact]
        public async Task ExistsInCollection_Exists_ReturnsTrue()
        {
            // Arrange
            const int collectionId = 1;
            const int memeId = 1;
            var existingCollectionMeme = new CollectionMeme { CollectionMemeId = 1, CollectionId = collectionId, MemeId = memeId };
            _mockCollectionMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<CollectionMeme, bool>>>()))
                .ReturnsAsync(new List<CollectionMeme> { existingCollectionMeme });

            // Act
            var result = await _collectionMemeService.ExistsInCollection(collectionId, memeId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ExistsInCollection_DoesntExist_ReturnsFalse()
        {
            // Arrange
            const int collectionId = 999;
            const int memeId = 999;
            _mockCollectionMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<CollectionMeme, bool>>>()))
                .ReturnsAsync(new List<CollectionMeme>());

            // Act
            var result = await _collectionMemeService.ExistsInCollection(collectionId, memeId);

            // Assert
            Assert.False(result);
        }
        #endregion

        #region GET MEME COUNT TESTS
        [Fact]
        public async Task GetMemeCountInCollection_ReturnsCorrectCount()
        {
            // Arrange
            const int collectionId = 1;
            var collectionMemes = new List<CollectionMeme>
            {
                new CollectionMeme { CollectionMemeId = 1, CollectionId = collectionId, MemeId = 1 },
                new CollectionMeme { CollectionMemeId = 2, CollectionId = collectionId, MemeId = 2 },
                new CollectionMeme { CollectionMemeId = 3, CollectionId = collectionId, MemeId = 3 }
            };
            _mockCollectionMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<CollectionMeme, bool>>>()))
                .ReturnsAsync(collectionMemes);

            // Act
            var result = await _collectionMemeService.GetMemeCountInCollection(collectionId);

            // Assert
            Assert.Equal(3, result);
        }
        #endregion

        #region CREATE COLLECTION MEME TESTS
        [Fact]
        public async Task Create_NullCollectionMeme_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _collectionMemeService.Create(null));
            _mockCollectionMemeRepository.Verify(x => x.Create(It.IsAny<CollectionMeme>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectCollectionMemes))]
        public async Task Create_IncorrectCollectionMeme_ThrowsArgumentException(CollectionMeme collectionMeme)
        {
            // Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _collectionMemeService.Create(collectionMeme));

            // Assert
            _mockCollectionMemeRepository.Verify(x => x.Create(It.IsAny<CollectionMeme>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task Create_NonExistentCollection_ThrowsInvalidOperationException()
        {
            // Arrange
            var collectionMeme = new CollectionMeme { CollectionId = 999, MemeId = 1 };
            _mockCollectionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Collection, bool>>>()))
                .ReturnsAsync(new List<Collection>());
            _mockMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>()))
                .ReturnsAsync(new List<Meme> { new Meme() });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _collectionMemeService.Create(collectionMeme));
            _mockCollectionMemeRepository.Verify(x => x.Create(It.IsAny<CollectionMeme>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_NonExistentMeme_ThrowsInvalidOperationException()
        {
            // Arrange
            var collectionMeme = new CollectionMeme { CollectionId = 1, MemeId = 999 };
            _mockCollectionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Collection, bool>>>()))
                .ReturnsAsync(new List<Collection> { new Collection() });
            _mockMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>()))
                .ReturnsAsync(new List<Meme>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _collectionMemeService.Create(collectionMeme));
            _mockCollectionMemeRepository.Verify(x => x.Create(It.IsAny<CollectionMeme>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_DuplicateCollectionMeme_ThrowsInvalidOperationException()
        {
            // Arrange
            var existingCollectionMeme = new CollectionMeme { CollectionMemeId = 1, CollectionId = 1, MemeId = 1 };
            var newCollectionMeme = new CollectionMeme { CollectionId = 1, MemeId = 1 };
            
            _mockCollectionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Collection, bool>>>()))
                .ReturnsAsync(new List<Collection> { new Collection() });
            _mockMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>()))
                .ReturnsAsync(new List<Meme> { new Meme() });
            _mockCollectionMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<CollectionMeme, bool>>>()))
                .ReturnsAsync(new List<CollectionMeme> { existingCollectionMeme });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _collectionMemeService.Create(newCollectionMeme));
            _mockCollectionMemeRepository.Verify(x => x.Create(It.IsAny<CollectionMeme>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_CorrectCollectionMeme_CallsCreateAndSave()
        {
            // Arrange
            var validCollectionMeme = new CollectionMeme { CollectionId = 1, MemeId = 1 };
            _mockCollectionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Collection, bool>>>()))
                .ReturnsAsync(new List<Collection> { new Collection() });
            _mockMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>()))
                .ReturnsAsync(new List<Meme> { new Meme() });
            _mockCollectionMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<CollectionMeme, bool>>>()))
                .ReturnsAsync(new List<CollectionMeme>());

            // Act
            await _collectionMemeService.Create(validCollectionMeme);

            // Assert
            _mockCollectionMemeRepository.Verify(x => x.Create(validCollectionMeme), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task AddMemeToCollection_ValidParameters_CallsCreate()
        {
            // Arrange
            const int collectionId = 1;
            const int memeId = 1;
            _mockCollectionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Collection, bool>>>()))
                .ReturnsAsync(new List<Collection> { new Collection() });
            _mockMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>()))
                .ReturnsAsync(new List<Meme> { new Meme() });
            _mockCollectionMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<CollectionMeme, bool>>>()))
                .ReturnsAsync(new List<CollectionMeme>());

            // Act
            await _collectionMemeService.AddMemeToCollection(collectionId, memeId);

            // Assert
            _mockCollectionMemeRepository.Verify(x => x.Create(It.Is<CollectionMeme>(cm => 
                cm.CollectionId == collectionId && cm.MemeId == memeId)), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }
        #endregion

        #region DELETE COLLECTION MEME TESTS
        [Fact]
        public async Task Delete_CollectionMemeExists_CallsDeleteAndSave()
        {
            // Arrange
            int deleteCollectionMemeId = 1;
            var existingCollectionMeme = new CollectionMeme { CollectionMemeId = deleteCollectionMemeId, CollectionId = 1, MemeId = 1 };
            _mockCollectionMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<CollectionMeme, bool>>>()))
                .ReturnsAsync(new List<CollectionMeme> { existingCollectionMeme });

            // Act
            await _collectionMemeService.Delete(deleteCollectionMemeId);

            // Assert
            _mockCollectionMemeRepository.Verify(x => x.Delete(existingCollectionMeme), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task RemoveMemeFromCollection_ValidParameters_CallsDeleteAndSave()
        {
            // Arrange
            const int collectionId = 1;
            const int memeId = 1;
            var existingCollectionMeme = new CollectionMeme { CollectionMemeId = 1, CollectionId = collectionId, MemeId = memeId };
            _mockCollectionMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<CollectionMeme, bool>>>()))
                .ReturnsAsync(new List<CollectionMeme> { existingCollectionMeme });

            // Act
            await _collectionMemeService.RemoveMemeFromCollection(collectionId, memeId);

            // Assert
            _mockCollectionMemeRepository.Verify(x => x.Delete(existingCollectionMeme), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task ClearCollection_WithMemes_DeletesAllAndSaves()
        {
            // Arrange
            const int collectionId = 1;
            var collectionMemes = new List<CollectionMeme>
            {
                new CollectionMeme { CollectionMemeId = 1, CollectionId = collectionId, MemeId = 1 },
                new CollectionMeme { CollectionMemeId = 2, CollectionId = collectionId, MemeId = 2 }
            };
            _mockCollectionMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<CollectionMeme, bool>>>()))
                .ReturnsAsync(collectionMemes);

            // Act
            await _collectionMemeService.ClearCollection(collectionId);

            // Assert
            _mockCollectionMemeRepository.Verify(x => x.Delete(It.IsAny<CollectionMeme>()), Times.Exactly(2));
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task ClearCollection_NoMemes_DoesNotSave()
        {
            // Arrange
            const int collectionId = 1;
            _mockCollectionMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<CollectionMeme, bool>>>()))
                .ReturnsAsync(new List<CollectionMeme>());

            // Act
            await _collectionMemeService.ClearCollection(collectionId);

            // Assert
            _mockCollectionMemeRepository.Verify(x => x.Delete(It.IsAny<CollectionMeme>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }
        #endregion

        #region GET IDS TESTS
        [Fact]
        public async Task GetMemeIdsInCollection_ReturnsCorrectIds()
        {
            // Arrange
            const int collectionId = 1;
            var collectionMemes = new List<CollectionMeme>
            {
                new CollectionMeme { CollectionMemeId = 1, CollectionId = collectionId, MemeId = 1 },
                new CollectionMeme { CollectionMemeId = 2, CollectionId = collectionId, MemeId = 2 },
                new CollectionMeme { CollectionMemeId = 3, CollectionId = collectionId, MemeId = 3 }
            };
            _mockCollectionMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<CollectionMeme, bool>>>()))
                .ReturnsAsync(collectionMemes);

            // Act
            var result = await _collectionMemeService.GetMemeIdsInCollection(collectionId);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains(1, result);
            Assert.Contains(2, result);
            Assert.Contains(3, result);
        }

        [Fact]
        public async Task GetCollectionIdsForMeme_ReturnsCorrectIds()
        {
            // Arrange
            const int memeId = 1;
            var collectionMemes = new List<CollectionMeme>
            {
                new CollectionMeme { CollectionMemeId = 1, CollectionId = 1, MemeId = memeId },
                new CollectionMeme { CollectionMemeId = 2, CollectionId = 2, MemeId = memeId },
                new CollectionMeme { CollectionMemeId = 3, CollectionId = 3, MemeId = memeId }
            };
            _mockCollectionMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<CollectionMeme, bool>>>()))
                .ReturnsAsync(collectionMemes);

            // Act
            var result = await _collectionMemeService.GetCollectionIdsForMeme(memeId);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains(1, result);
            Assert.Contains(2, result);
            Assert.Contains(3, result);
        }
        #endregion

        public static IEnumerable<object[]> GetIncorrectCollectionMemes()
        {
            return new List<object[]>
            {
                new object[] { new CollectionMeme() { CollectionId = 0, MemeId = 1 } },
                new object[] { new CollectionMeme() { CollectionId = -1, MemeId = 1 } },
                new object[] { new CollectionMeme() { CollectionId = 1, MemeId = 0 } },
                new object[] { new CollectionMeme() { CollectionId = 1, MemeId = -1 } }
            };
        }
    }
}
using BusinessLogic.Services;
using Domain.Interfaces;
using Domain.Interfaces.ICollection;
using Domain.Interfaces.ICollectionMeme;
using Domain.Models;
using Moq;
using System.Linq.Expressions;

namespace BusinessLogic.Tests
{
    public class CollectionServiceTest
    {
        private readonly CollectionService _collectionService;
        private readonly Mock<ICollectionRepository> _mockCollectionRepository;
        private readonly Mock<ICollectionMemeRepository> _mockCollectionMemeRepository;
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;

        public CollectionServiceTest()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockCollectionRepository = new Mock<ICollectionRepository>();
            _mockCollectionMemeRepository = new Mock<ICollectionMemeRepository>();

            _mockRepositoryWrapper.Setup(x => x.Collection).Returns(_mockCollectionRepository.Object);
            _mockRepositoryWrapper.Setup(x => x.CollectionMeme).Returns(_mockCollectionMemeRepository.Object);
            _mockRepositoryWrapper.Setup(x => x.Save()).Returns(Task.CompletedTask);

            _collectionService = new CollectionService(_mockRepositoryWrapper.Object);
        }

        #region GET ALL COLLECTIONS TESTS

        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoCollections()
        {
            // Arrange
            var emptyList = new List<Collection>();
            _mockCollectionRepository.Setup(x => x.FindAll()).ReturnsAsync(emptyList);

            // Act
            var result = await _collectionService.GetAll();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAll_ReturnsListCollections_WhenCollectionsExist()
        {
            // Arrange
            var collections = new List<Collection>()
            {
                new Collection() { CollectionId = 1, UserId = 1, CollectionName = "Test Collection 1", Description = "Desc 1", IsPublic = true },
                new Collection() { CollectionId = 2, UserId = 2, CollectionName = "Test Collection 2", Description = "Desc 2", IsPublic = false }
            };
            _mockCollectionRepository.Setup(x => x.FindAll()).ReturnsAsync(collections);

            // Act
            var result = await _collectionService.GetAll();

            // Assert
            Assert.Equal(collections, result);
        }

        [Fact]
        public async Task GetAll_ThrowsException_WhenRepositoryThrows()
        {
            // Arrange
            _mockCollectionRepository.Setup(x => x.FindAll()).ThrowsAsync(new InvalidOperationException("DB Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _collectionService.GetAll());
        }
        #endregion

        #region GET PUBLIC COLLECTIONS TESTS
        [Fact]
        public async Task GetPublicCollections_ReturnsOnlyPublicCollections()
        {
            // Arrange
            var collections = new List<Collection>
            {
                new Collection { CollectionId = 1, UserId = 1, CollectionName = "Public 1", IsPublic = true },
                new Collection { CollectionId = 2, UserId = 2, CollectionName = "Private", IsPublic = false },
                new Collection { CollectionId = 3, UserId = 3, CollectionName = "Public 2", IsPublic = true }
            };
            _mockCollectionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Collection, bool>>>()))
                .ReturnsAsync((Expression<Func<Collection, bool>> predicate) =>
                    collections.Where(predicate.Compile()).ToList());

            // Act
            var result = await _collectionService.GetPublicCollections();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, c => Assert.True(c.IsPublic));
        }
        #endregion

        #region GET BY USER ID TESTS
        [Fact]
        public async Task GetByUserId_ReturnsUserCollections()
        {
            // Arrange
            const int userId = 1;
            var collections = new List<Collection>
            {
                new Collection { CollectionId = 1, UserId = userId, CollectionName = "Collection 1" },
                new Collection { CollectionId = 2, UserId = userId, CollectionName = "Collection 2" },
                new Collection { CollectionId = 3, UserId = 2, CollectionName = "Other User Collection" }
            };
            _mockCollectionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Collection, bool>>>()))
                .ReturnsAsync((Expression<Func<Collection, bool>> predicate) =>
                    collections.Where(predicate.Compile()).ToList());

            // Act
            var result = await _collectionService.GetByUserId(userId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, c => Assert.Equal(userId, c.UserId));
        }
        #endregion

        #region GET BY ID TESTS
        [Fact]
        public async Task GetById_CollectionExists_ReturnsCorrectCollection()
        {
            // Arrange
            var existingCollection = new Collection { CollectionId = 1, UserId = 1, CollectionName = "Test Collection", Description = "Test Desc", IsPublic = true };
            _mockCollectionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Collection, bool>>>()))
                .ReturnsAsync(new List<Collection> { existingCollection });

            // Act
            var result = await _collectionService.GetById(existingCollection.CollectionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingCollection.CollectionId, result.CollectionId);
            Assert.Equal(existingCollection.CollectionName, result.CollectionName);
        }

        [Fact]
        public async Task GetById_CollectionDoesntExist_ThrowsInvalidOperationException()
        {
            // Arrange
            const int nonexistentCollectionId = 999;
            _mockCollectionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Collection, bool>>>()))
                .ReturnsAsync(new List<Collection>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _collectionService.GetById(nonexistentCollectionId));
        }

        [Fact]
        public async Task GetUserCollectionById_ValidUserAndCollection_ReturnsCollection()
        {
            // Arrange
            const int userId = 1;
            const int collectionId = 1;
            var existingCollection = new Collection { CollectionId = collectionId, UserId = userId, CollectionName = "User Collection" };
            _mockCollectionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Collection, bool>>>()))
                .ReturnsAsync(new List<Collection> { existingCollection });

            // Act
            var result = await _collectionService.GetUserCollectionById(userId, collectionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(collectionId, result.CollectionId);
            Assert.Equal(userId, result.UserId);
        }

        [Fact]
        public async Task GetUserCollectionById_WrongUser_ThrowsInvalidOperationException()
        {
            // Arrange
            const int userId = 1;
            const int collectionId = 1;
            var existingCollection = new Collection { CollectionId = collectionId, UserId = 2, CollectionName = "Other User Collection" };
            _mockCollectionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Collection, bool>>>()))
                .ReturnsAsync(new List<Collection> { existingCollection });

            _mockCollectionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Collection, bool>>>()))
                .ReturnsAsync((Expression<Func<Collection, bool>> predicate) =>
                {
                    var func = predicate.Compile();
                    return new List<Collection> { existingCollection }.Where(func).ToList();
                });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _collectionService.GetUserCollectionById(userId, collectionId));
        }
        #endregion

        #region CREATE COLLECTION TESTS
        [Fact]
        public async Task Create_NullCollection_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _collectionService.Create(null));
            _mockCollectionRepository.Verify(x => x.Create(It.IsAny<Collection>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectCollections))]
        public async Task Create_IncorrectCollection_ThrowsArgumentException(Collection collection)
        {
            // Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _collectionService.Create(collection));

            // Assert
            _mockCollectionRepository.Verify(x => x.Create(It.IsAny<Collection>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Theory]
        [MemberData(nameof(GetWhitespaceCollections))]
        public async Task Create_WhitespaceCollection_ThrowsArgumentException(Collection collection)
        {
            // Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _collectionService.Create(collection));

            // Assert
            _mockCollectionRepository.Verify(x => x.Create(It.IsAny<Collection>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task Create_DuplicateCollectionName_ThrowsInvalidOperationException()
        {
            // Arrange
            var existingCollection = new Collection { CollectionId = 1, UserId = 1, CollectionName = "Existing Collection" };
            var newCollection = new Collection { UserId = 1, CollectionName = "Existing Collection" };

            _mockCollectionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Collection, bool>>>()))
                .ReturnsAsync(new List<Collection> { existingCollection });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _collectionService.Create(newCollection));
            _mockCollectionRepository.Verify(x => x.Create(It.IsAny<Collection>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_CorrectCollection_SetsDefaultsAndCallsCreateAndSave()
        {
            // Arrange
            var validCollection = new Collection { UserId = 1, CollectionName = "New Collection" }; // Description and IsPublic not set
            _mockCollectionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Collection, bool>>>()))
                .ReturnsAsync(new List<Collection>());

            // Act
            await _collectionService.Create(validCollection);

            // Assert
            Assert.Equal(string.Empty, validCollection.Description);
            Assert.False(validCollection.IsPublic);
            _mockCollectionRepository.Verify(x => x.Create(validCollection), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Create_CorrectCollectionWithValues_CallsCreateAndSave()
        {
            // Arrange
            var validCollection = new Collection { UserId = 1, CollectionName = "New Collection", Description = "Test Desc", IsPublic = true };
            _mockCollectionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Collection, bool>>>()))
                .ReturnsAsync(new List<Collection>());

            // Act
            await _collectionService.Create(validCollection);

            // Assert
            _mockCollectionRepository.Verify(x => x.Create(validCollection), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }
        #endregion

        #region UPDATE COLLECTION TESTS
        [Fact]
        public async Task Update_NullCollection_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _collectionService.Update(null));
            _mockCollectionRepository.Verify(x => x.Update(It.IsAny<Collection>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Update_NonExistentCollection_ThrowsInvalidOperationException()
        {
            // Arrange
            var collection = new Collection { CollectionId = 1, UserId = 1, CollectionName = "Test Collection" };
            _mockCollectionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Collection, bool>>>()))
                .ReturnsAsync(new List<Collection>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _collectionService.Update(collection));
            _mockCollectionRepository.Verify(x => x.Update(It.IsAny<Collection>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Update_DuplicateCollectionName_ThrowsInvalidOperationException()
        {
            // Arrange
            var existingCollection = new Collection { CollectionId = 1, UserId = 1, CollectionName = "Existing Collection" };
            var updatedCollection = new Collection { CollectionId = 2, UserId = 1, CollectionName = "Existing Collection" };

            _mockCollectionRepository.Setup(x => x.FindByCondition(It.Is<Expression<Func<Collection, bool>>>(e =>
                e.Compile()(new Collection { CollectionId = updatedCollection.CollectionId }))))
                .ReturnsAsync(new List<Collection> { updatedCollection });
            _mockCollectionRepository.Setup(x => x.FindByCondition(It.Is<Expression<Func<Collection, bool>>>(e =>
                e.Compile()(new Collection { UserId = updatedCollection.UserId, CollectionName = updatedCollection.CollectionName }))))
                .ReturnsAsync(new List<Collection> { existingCollection });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _collectionService.Update(updatedCollection));
            _mockCollectionRepository.Verify(x => x.Update(It.IsAny<Collection>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task UpdateCollectionName_ValidCollection_UpdatesName()
        {
            // Arrange
            const int collectionId = 1;
            const string newName = "Updated Name";
            var existingCollection = new Collection { CollectionId = collectionId, UserId = 1, CollectionName = "Old Name" };
            _mockCollectionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Collection, bool>>>()))
                .ReturnsAsync(new List<Collection> { existingCollection });

            // Act
            await _collectionService.UpdateCollectionName(collectionId, newName);

            // Assert
            Assert.Equal(newName, existingCollection.CollectionName);
            _mockCollectionRepository.Verify(x => x.Update(existingCollection), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task UpdateCollectionVisibility_ValidCollection_UpdatesVisibility()
        {
            // Arrange
            const int collectionId = 1;
            const bool newVisibility = true;
            var existingCollection = new Collection { CollectionId = collectionId, UserId = 1, CollectionName = "Test", IsPublic = false };
            _mockCollectionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Collection, bool>>>()))
                .ReturnsAsync(new List<Collection> { existingCollection });

            // Act
            await _collectionService.UpdateCollectionVisibility(collectionId, newVisibility);

            // Assert
            Assert.Equal(newVisibility, existingCollection.IsPublic);
            _mockCollectionRepository.Verify(x => x.Update(existingCollection), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }
        #endregion

        #region DELETE COLLECTION TESTS
        [Fact]
        public async Task Delete_CollectionExistsWithoutMemes_CallsDeleteAndSave()
        {
            // Arrange
            int deleteCollectionId = 1;
            var existingCollection = new Collection { CollectionId = deleteCollectionId, UserId = 1, CollectionName = "Delete Me" };
            _mockCollectionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Collection, bool>>>()))
                .ReturnsAsync(new List<Collection> { existingCollection });
            _mockCollectionMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<CollectionMeme, bool>>>()))
                .ReturnsAsync(new List<CollectionMeme>());

            // Act
            await _collectionService.Delete(deleteCollectionId);

            // Assert
            _mockCollectionRepository.Verify(x => x.Delete(existingCollection), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_CollectionWithMemes_ThrowsInvalidOperationException()
        {
            // Arrange
            int deleteCollectionId = 1;
            var existingCollection = new Collection { CollectionId = deleteCollectionId, UserId = 1, CollectionName = "Collection with Memes" };
            var collectionMemes = new List<CollectionMeme>
            {
                new CollectionMeme { CollectionMemeId = 1, CollectionId = deleteCollectionId, MemeId = 1 }
            };

            _mockCollectionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Collection, bool>>>()))
                .ReturnsAsync(new List<Collection> { existingCollection });
            _mockCollectionMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<CollectionMeme, bool>>>()))
                .ReturnsAsync(collectionMemes);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _collectionService.Delete(deleteCollectionId));
            _mockCollectionRepository.Verify(x => x.Delete(It.IsAny<Collection>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }
        #endregion

        #region HELPER METHODS TESTS
        [Fact]
        public async Task IsCollectionOwner_ValidOwner_ReturnsTrue()
        {
            // Arrange
            const int collectionId = 1;
            const int userId = 1;
            var collection = new Collection { CollectionId = collectionId, UserId = userId };
            _mockCollectionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Collection, bool>>>()))
                .ReturnsAsync(new List<Collection> { collection });

            // Act
            bool result = await _collectionService.IsCollectionOwner(collectionId, userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsCollectionOwner_InvalidOwner_ReturnsFalse()
        {
            // Arrange
            const int collectionId = 1;
            const int userId = 1;
            var collection = new Collection { CollectionId = collectionId, UserId = 2 }; // Different user

            _mockCollectionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Collection, bool>>>()))
                .ReturnsAsync((Expression<Func<Collection, bool>> predicate) =>
                {
                    var func = predicate.Compile();
                    return new List<Collection> { collection }.Where(func).ToList();
                });

            // Act
            bool result = await _collectionService.IsCollectionOwner(collectionId, userId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetCollectionMemeCount_ReturnsCorrectCount()
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
            int result = await _collectionService.GetCollectionMemeCount(collectionId);

            // Assert
            Assert.Equal(3, result);
        }
        #endregion

        public static IEnumerable<object[]> GetIncorrectCollections()
        {
            return new List<object[]>
            {
                new object[] { new Collection() { UserId = 0, CollectionName = "Test Collection" } },
                new object[] { new Collection() { UserId = -1, CollectionName = "Test Collection" } },
                new object[] { new Collection() { UserId = 1, CollectionName = "" } },
                new object[] { new Collection() { UserId = 1, CollectionName = null } }
            };
        }

        public static IEnumerable<object[]> GetWhitespaceCollections()
        {
            return new List<object[]>
            {
                new object[] { new Collection() { UserId = 1, CollectionName = " " } },
                new object[] { new Collection() { UserId = 1, CollectionName = "   " } }
            };
        }
    }
}
using BusinessLogic.Services;
using Domain.Interfaces;
using Domain.Interfaces.IReaction;
using Domain.Models;
using Moq;
using System.Linq.Expressions;

namespace BusinessLogic.Tests
{
    public class ReactionServiceTest
    {
        private readonly ReactionService _reactionService;
        private readonly Mock<IReactionRepository> _mockReactionRepository;
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;

        public ReactionServiceTest()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockReactionRepository = new Mock<IReactionRepository>();
            
            _mockRepositoryWrapper.Setup(x => x.Reaction).Returns(_mockReactionRepository.Object);
            _mockRepositoryWrapper.Setup(x => x.Save()).Returns(Task.CompletedTask);
            
            _reactionService = new ReactionService(_mockRepositoryWrapper.Object);
        }

        #region GET ALL REACTIONS TESTS

        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoReactions()
        {
            // Arrange
            var emptyList = new List<Reaction>();
            _mockReactionRepository.Setup(x => x.FindAll()).ReturnsAsync(emptyList);
            
            // Act
            var result = await _reactionService.GetAll();
            
            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAll_ReturnsListReactions_WhenReactionsExist()
        {
            // Arrange
            var reactions = new List<Reaction>()
            {
                new Reaction() { ReactionId = 1, MemeId = 1, UserId = 1, ReactionType = "like" },
                new Reaction() { ReactionId = 2, MemeId = 1, UserId = 2, ReactionType = "dislike" }
            };
            _mockReactionRepository.Setup(x => x.FindAll()).ReturnsAsync(reactions);
            
            // Act
            var result = await _reactionService.GetAll();
            
            // Assert
            Assert.Equal(reactions, result);
        }

        [Fact]
        public async Task GetAll_ThrowsException_WhenRepositoryThrows()
        {
            // Arrange
            _mockReactionRepository.Setup(x => x.FindAll()).ThrowsAsync(new InvalidOperationException("DB Error"));
            
            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _reactionService.GetAll());
        }
        #endregion

        #region GET BY ID TESTS
        [Fact]
        public async Task GetById_ReactionExists_ReturnsCorrectReaction()
        {
            // Arrange
            var existingReaction = new Reaction { ReactionId = 1, MemeId = 1, UserId = 1, ReactionType = "like" };
            _mockReactionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Reaction, bool>>>()))
                .ReturnsAsync(new List<Reaction> { existingReaction });

            // Act
            var result = await _reactionService.GetById(existingReaction.ReactionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingReaction.ReactionId, result.ReactionId);
            Assert.Equal(existingReaction.ReactionType, result.ReactionType);
        }

        [Fact]
        public async Task GetById_ReactionDoesntExist_ThrowsInvalidOperationException()
        {
            // Arrange
            const int nonexistentReactionId = 999;
            _mockReactionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Reaction, bool>>>()))
                .ReturnsAsync(new List<Reaction>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _reactionService.GetById(nonexistentReactionId));
        }

        [Fact]
        public async Task GetById_MultipleReactionsWithSameId_ThrowsInvalidOperationException()
        {
            // Arrange
            const int reactionId = 1;
            var reactions = new List<Reaction>
            {
                new Reaction { ReactionId = reactionId, MemeId = 1, UserId = 1, ReactionType = "like" },
                new Reaction { ReactionId = reactionId, MemeId = 2, UserId = 2, ReactionType = "dislike" }
            };
            _mockReactionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Reaction, bool>>>()))
                .ReturnsAsync(reactions);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _reactionService.GetById(reactionId));
        }

        [Fact]
        public async Task GetById_ThrowsException_WhenRepositoryThrows()
        {
            // Arrange
            const int reactionId = 1;
            _mockReactionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Reaction, bool>>>()))
                .ThrowsAsync(new InvalidOperationException("DB Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _reactionService.GetById(reactionId));
        }
        #endregion

        #region GET BY MEME ID TESTS
        [Fact]
        public async Task GetByMemeId_ReturnsReactions_WhenReactionsExist()
        {
            // Arrange
            const int memeId = 1;
            var reactions = new List<Reaction>
            {
                new Reaction { ReactionId = 1, MemeId = memeId, UserId = 1, ReactionType = "like" },
                new Reaction { ReactionId = 2, MemeId = memeId, UserId = 2, ReactionType = "dislike" }
            };
            _mockReactionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Reaction, bool>>>()))
                .ReturnsAsync(reactions);

            // Act
            var result = await _reactionService.GetByMemeId(memeId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, r => Assert.Equal(memeId, r.MemeId));
        }

        [Fact]
        public async Task GetByMemeId_ReturnsEmptyList_WhenNoReactions()
        {
            // Arrange
            const int memeId = 999;
            _mockReactionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Reaction, bool>>>()))
                .ReturnsAsync(new List<Reaction>());

            // Act
            var result = await _reactionService.GetByMemeId(memeId);

            // Assert
            Assert.Empty(result);
        }
        #endregion

        #region GET BY USER ID TESTS
        [Fact]
        public async Task GetByUserId_ReturnsReactions_WhenReactionsExist()
        {
            // Arrange
            const int userId = 1;
            var reactions = new List<Reaction>
            {
                new Reaction { ReactionId = 1, MemeId = 1, UserId = userId, ReactionType = "like" },
                new Reaction { ReactionId = 2, MemeId = 2, UserId = userId, ReactionType = "dislike" }
            };
            _mockReactionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Reaction, bool>>>()))
                .ReturnsAsync(reactions);

            // Act
            var result = await _reactionService.GetByUserId(userId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, r => Assert.Equal(userId, r.UserId));
        }
        #endregion

        #region GET BY MEME AND USER TESTS
        [Fact]
        public async Task GetByMemeAndUser_ReactionExists_ReturnsCorrectReaction()
        {
            // Arrange
            const int memeId = 1;
            const int userId = 1;
            var existingReaction = new Reaction { ReactionId = 1, MemeId = memeId, UserId = userId, ReactionType = "like" };
            _mockReactionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Reaction, bool>>>()))
                .ReturnsAsync(new List<Reaction> { existingReaction });

            // Act
            var result = await _reactionService.GetByMemeAndUser(memeId, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(memeId, result.MemeId);
            Assert.Equal(userId, result.UserId);
        }

        [Fact]
        public async Task GetByMemeAndUser_ReactionDoesntExist_ThrowsInvalidOperationException()
        {
            // Arrange
            const int memeId = 999;
            const int userId = 999;
            _mockReactionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Reaction, bool>>>()))
                .ReturnsAsync(new List<Reaction>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _reactionService.GetByMemeAndUser(memeId, userId));
        }
        #endregion

        #region GET REACTION COUNTS TESTS
        [Fact]
        public async Task GetReactionCounts_ReturnsCorrectCounts()
        {
            // Arrange
            const int memeId = 1;
            var reactions = new List<Reaction>
            {
                new Reaction { ReactionId = 1, MemeId = memeId, UserId = 1, ReactionType = "like" },
                new Reaction { ReactionId = 2, MemeId = memeId, UserId = 2, ReactionType = "like" },
                new Reaction { ReactionId = 3, MemeId = memeId, UserId = 3, ReactionType = "dislike" },
                new Reaction { ReactionId = 4, MemeId = memeId, UserId = 4, ReactionType = "love" }
            };
            _mockReactionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Reaction, bool>>>()))
                .ReturnsAsync(reactions);

            // Act
            var result = await _reactionService.GetReactionCounts(memeId);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(2, result["like"]);
            Assert.Equal(1, result["dislike"]);
            Assert.Equal(1, result["love"]);
        }

        [Fact]
        public async Task GetReactionCounts_ReturnsEmptyDictionary_WhenNoReactions()
        {
            // Arrange
            const int memeId = 999;
            _mockReactionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Reaction, bool>>>()))
                .ReturnsAsync(new List<Reaction>());

            // Act
            var result = await _reactionService.GetReactionCounts(memeId);

            // Assert
            Assert.Empty(result);
        }
        #endregion

        #region CREATE REACTION TESTS
        [Fact]
        public async Task Create_NullReaction_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _reactionService.Create(null));
            _mockReactionRepository.Verify(x => x.Create(It.IsAny<Reaction>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectReactions))]
        public async Task Create_IncorrectReaction_ThrowsArgumentException(Reaction reaction)
        {
            // Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _reactionService.Create(reaction));

            // Assert
            _mockReactionRepository.Verify(x => x.Create(It.IsAny<Reaction>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Theory]
        [MemberData(nameof(GetWhitespaceReactions))]
        public async Task Create_WhitespaceReaction_ThrowsArgumentException(Reaction reaction)
        {
            // Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _reactionService.Create(reaction));

            // Assert
            _mockReactionRepository.Verify(x => x.Create(It.IsAny<Reaction>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task Create_DuplicateReaction_ThrowsInvalidOperationException()
        {
            // Arrange
            var existingReaction = new Reaction { ReactionId = 1, MemeId = 1, UserId = 1, ReactionType = "like" };
            var newReaction = new Reaction { MemeId = 1, UserId = 1, ReactionType = "dislike" };
            
            _mockReactionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Reaction, bool>>>()))
                .ReturnsAsync(new List<Reaction> { existingReaction });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _reactionService.Create(newReaction));
            _mockReactionRepository.Verify(x => x.Create(It.IsAny<Reaction>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_CorrectReaction_CallsCreateAndSave()
        {
            // Arrange
            var validReaction = new Reaction { MemeId = 1, UserId = 1, ReactionType = "like" };
            _mockReactionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Reaction, bool>>>()))
                .ReturnsAsync(new List<Reaction>());

            // Act
            await _reactionService.Create(validReaction);

            // Assert
            _mockReactionRepository.Verify(x => x.Create(validReaction), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Create_ThrowsException_WhenRepositoryCreateThrows()
        {
            // Arrange
            var validReaction = new Reaction { MemeId = 1, UserId = 1, ReactionType = "like" };
            _mockReactionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Reaction, bool>>>()))
                .ReturnsAsync(new List<Reaction>());
            _mockReactionRepository.Setup(x => x.Create(validReaction)).ThrowsAsync(new InvalidOperationException("DB Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _reactionService.Create(validReaction));
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_ThrowsException_WhenSaveThrows()
        {
            // Arrange
            var validReaction = new Reaction { MemeId = 1, UserId = 1, ReactionType = "like" };
            _mockReactionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Reaction, bool>>>()))
                .ReturnsAsync(new List<Reaction>());
            _mockRepositoryWrapper.Setup(x => x.Save()).ThrowsAsync(new InvalidOperationException("Save Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _reactionService.Create(validReaction));
        }
        #endregion

        #region UPDATE REACTION TESTS
        [Fact]
        public async Task Update_NullReaction_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _reactionService.Update(null));
            _mockReactionRepository.Verify(x => x.Update(It.IsAny<Reaction>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectReactions))]
        public async Task Update_IncorrectReaction_ThrowsArgumentException(Reaction reaction)
        {
            // Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _reactionService.Update(reaction));

            // Assert
            _mockReactionRepository.Verify(x => x.Update(It.IsAny<Reaction>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task Update_NonExistentReaction_ThrowsInvalidOperationException()
        {
            // Arrange
            var reaction = new Reaction { ReactionId = 1, MemeId = 1, UserId = 1, ReactionType = "like" };
            _mockReactionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Reaction, bool>>>()))
                .ReturnsAsync(new List<Reaction>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _reactionService.Update(reaction));
            _mockReactionRepository.Verify(x => x.Update(It.IsAny<Reaction>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Update_ValidReaction_CallsUpdateAndSave()
        {
            // Arrange
            var updatedReaction = new Reaction { ReactionId = 1, MemeId = 1, UserId = 1, ReactionType = "dislike" };
            _mockReactionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Reaction, bool>>>()))
                .ReturnsAsync(new List<Reaction> { updatedReaction });

            // Act
            await _reactionService.Update(updatedReaction);

            // Assert
            _mockReactionRepository.Verify(x => x.Update(updatedReaction), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task UpdateReaction_ValidParameters_UpdatesReactionType()
        {
            // Arrange
            const int memeId = 1;
            const int userId = 1;
            const string newReactionType = "love";
            var existingReaction = new Reaction { ReactionId = 1, MemeId = memeId, UserId = userId, ReactionType = "like" };
            _mockReactionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Reaction, bool>>>()))
                .ReturnsAsync(new List<Reaction> { existingReaction });

            // Act
            await _reactionService.UpdateReaction(memeId, userId, newReactionType);

            // Assert
            Assert.Equal(newReactionType, existingReaction.ReactionType);
            _mockReactionRepository.Verify(x => x.Update(existingReaction), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task UpdateReaction_WhitespaceReactionType_ThrowsArgumentException()
        {
            // Arrange
            const int memeId = 1;
            const int userId = 1;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _reactionService.UpdateReaction(memeId, userId, " "));
            _mockReactionRepository.Verify(x => x.Update(It.IsAny<Reaction>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }
        #endregion

        #region DELETE REACTION TESTS
        [Fact]
        public async Task Delete_ReactionExists_CallsDeleteAndSave()
        {
            // Arrange
            int deleteReactionId = 1;
            var existingReaction = new Reaction { ReactionId = deleteReactionId, MemeId = 1, UserId = 1, ReactionType = "like" };
            _mockReactionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Reaction, bool>>>()))
                .ReturnsAsync(new List<Reaction> { existingReaction });

            // Act
            await _reactionService.Delete(deleteReactionId);

            // Assert
            _mockReactionRepository.Verify(x => x.Delete(existingReaction), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task DeleteByMemeAndUser_ReactionExists_CallsDeleteAndSave()
        {
            // Arrange
            const int memeId = 1;
            const int userId = 1;
            var existingReaction = new Reaction { ReactionId = 1, MemeId = memeId, UserId = userId, ReactionType = "like" };
            _mockReactionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Reaction, bool>>>()))
                .ReturnsAsync(new List<Reaction> { existingReaction });

            // Act
            await _reactionService.DeleteByMemeAndUser(memeId, userId);

            // Assert
            _mockReactionRepository.Verify(x => x.Delete(existingReaction), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_ReactionDoesntExist_ThrowsInvalidOperationException()
        {
            // Arrange
            const int nonexistentReactionId = 999;
            _mockReactionRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Reaction, bool>>>()))
                .ReturnsAsync(new List<Reaction>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _reactionService.Delete(nonexistentReactionId));
            _mockReactionRepository.Verify(x => x.Delete(It.IsAny<Reaction>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }
        #endregion

        public static IEnumerable<object[]> GetIncorrectReactions()
        {
            return new List<object[]>
            {
                new object[] { new Reaction() { MemeId = 0, UserId = 1, ReactionType = "like" } },
                new object[] { new Reaction() { MemeId = -1, UserId = 1, ReactionType = "like" } },
                new object[] { new Reaction() { MemeId = 1, UserId = 0, ReactionType = "like" } },
                new object[] { new Reaction() { MemeId = 1, UserId = -1, ReactionType = "like" } },
                new object[] { new Reaction() { MemeId = 1, UserId = 1, ReactionType = "" } },
                new object[] { new Reaction() { MemeId = 1, UserId = 1, ReactionType = null } }
            };
        }

        public static IEnumerable<object[]> GetWhitespaceReactions()
        {
            return new List<object[]>
            {
                new object[] { new Reaction() { MemeId = 1, UserId = 1, ReactionType = " " } },
                new object[] { new Reaction() { MemeId = 1, UserId = 1, ReactionType = "   " } }
            };
        }
    }
}
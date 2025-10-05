using BusinessLogic.Services;
using Domain.Interfaces;
using Domain.Interfaces.IComment;
using Domain.Models;
using Moq;
using System.Linq.Expressions;

namespace BusinessLogic.Tests
{
    public class CommentServiceTest
    {
        private readonly CommentService _commentService;
        private readonly Mock<ICommentRepository> _mockCommentRepository;
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;

        public CommentServiceTest()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockCommentRepository = new Mock<ICommentRepository>();

            _mockRepositoryWrapper.Setup(x => x.Comment).Returns(_mockCommentRepository.Object);
            _mockRepositoryWrapper.Setup(x => x.Save()).Returns(Task.CompletedTask);

            _commentService = new CommentService(_mockRepositoryWrapper.Object);
        }

        #region GET ALL COMMENTS TESTS

        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoComments()
        {
            // Arrange
            var emptyList = new List<Comment>();
            _mockCommentRepository.Setup(x => x.FindAll()).ReturnsAsync(emptyList);

            // Act
            var result = await _commentService.GetAll();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAll_ReturnsListComments_WhenCommentsExist()
        {
            // Arrange
            var comments = new List<Comment>()
            {
                new Comment() { CommentId = 1, CommentText = "Test Comment 1", UserId = 1, MemeId = 1 },
                new Comment() { CommentId = 2, CommentText = "Test Comment 2", UserId = 2, MemeId = 1 }
            };
            _mockCommentRepository.Setup(x => x.FindAll()).ReturnsAsync(comments);

            // Act
            var result = await _commentService.GetAll();

            // Assert
            Assert.Equal(comments, result);
        }

        [Fact]
        public async Task GetAll_ThrowsException_WhenRepositoryThrows()
        {
            // Arrange
            _mockCommentRepository.Setup(x => x.FindAll()).ThrowsAsync(new InvalidOperationException("DB Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _commentService.GetAll());
        }
        #endregion

        #region GET BY ID TESTS
        [Fact]
        public async Task GetById_CommentExists_ReturnsCorrectComment()
        {
            // Arrange
            var existingComment = new Comment { CommentId = 1, CommentText = "Existing Comment", UserId = 1, MemeId = 1 };
            _mockCommentRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Comment, bool>>>()))
                .ReturnsAsync(new List<Comment> { existingComment });

            // Act
            var result = await _commentService.GetById(existingComment.CommentId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingComment.CommentId, result.CommentId);
            Assert.Equal(existingComment.CommentText, result.CommentText);
        }

        [Fact]
        public async Task GetById_CommentDoesntExist_ThrowsInvalidOperationException()
        {
            // Arrange
            const int nonexistentCommentId = 999;
            _mockCommentRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Comment, bool>>>()))
                .ReturnsAsync(new List<Comment>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _commentService.GetById(nonexistentCommentId));
        }

        [Fact]
        public async Task GetById_MultipleCommentsWithSameId_ThrowsInvalidOperationException()
        {
            // Arrange
            const int commentId = 1;
            var comments = new List<Comment>
            {
                new Comment { CommentId = commentId, CommentText = "Dup1", UserId = 1, MemeId = 1 },
                new Comment { CommentId = commentId, CommentText = "Dup2", UserId = 2, MemeId = 1 }
            };
            _mockCommentRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Comment, bool>>>()))
                .ReturnsAsync(comments);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _commentService.GetById(commentId));
        }

        [Fact]
        public async Task GetById_ThrowsException_WhenRepositoryThrows()
        {
            // Arrange
            const int commentId = 1;
            _mockCommentRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Comment, bool>>>()))
                .ThrowsAsync(new InvalidOperationException("DB Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _commentService.GetById(commentId));
        }
        #endregion

        #region GET BY MEME ID TESTS
        [Fact]
        public async Task GetByMemeId_ReturnsComments_WhenCommentsExist()
        {
            // Arrange
            const int memeId = 1;
            var comments = new List<Comment>
            {
                new Comment { CommentId = 1, CommentText = "Comment 1", UserId = 1, MemeId = memeId },
                new Comment { CommentId = 2, CommentText = "Comment 2", UserId = 2, MemeId = memeId }
            };
            _mockCommentRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Comment, bool>>>()))
                .ReturnsAsync(comments);

            // Act
            var result = await _commentService.GetByMemeId(memeId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, c => Assert.Equal(memeId, c.MemeId));
        }

        [Fact]
        public async Task GetByMemeId_ReturnsEmptyList_WhenNoComments()
        {
            // Arrange
            const int memeId = 999;
            _mockCommentRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Comment, bool>>>()))
                .ReturnsAsync(new List<Comment>());

            // Act
            var result = await _commentService.GetByMemeId(memeId);

            // Assert
            Assert.Empty(result);
        }
        #endregion

        #region GET REPLIES TESTS
        [Fact]
        public async Task GetReplies_ReturnsReplies_WhenRepliesExist()
        {
            // Arrange
            const int parentCommentId = 1;
            var replies = new List<Comment>
            {
                new Comment { CommentId = 2, CommentText = "Reply 1", UserId = 1, MemeId = 1, ParentCommentId = parentCommentId },
                new Comment { CommentId = 3, CommentText = "Reply 2", UserId = 2, MemeId = 1, ParentCommentId = parentCommentId }
            };
            _mockCommentRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Comment, bool>>>()))
                .ReturnsAsync(replies);

            // Act
            var result = await _commentService.GetReplies(parentCommentId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, c => Assert.Equal(parentCommentId, c.ParentCommentId));
        }
        #endregion

        #region CREATE COMMENT TESTS
        [Fact]
        public async Task Create_NullComment_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _commentService.Create(null));
            _mockCommentRepository.Verify(x => x.Create(It.IsAny<Comment>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectComments))]
        public async Task Create_IncorrectComment_ThrowsArgumentException(Comment comment)
        {
            // Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _commentService.Create(comment));

            // Assert
            _mockCommentRepository.Verify(x => x.Create(It.IsAny<Comment>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Theory]
        [MemberData(nameof(GetWhitespaceComments))]
        public async Task Create_WhitespaceComment_ThrowsArgumentException(Comment comment)
        {
            // Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _commentService.Create(comment));

            // Assert
            _mockCommentRepository.Verify(x => x.Create(It.IsAny<Comment>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task Create_WithNonExistentParentComment_ThrowsInvalidOperationException()
        {
            // Arrange
            var commentWithInvalidParent = new Comment
            {
                CommentText = "Valid Comment",
                UserId = 1,
                MemeId = 1,
                ParentCommentId = 999
            };
            _mockCommentRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Comment, bool>>>()))
                .ReturnsAsync(new List<Comment>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _commentService.Create(commentWithInvalidParent));
        }

        [Fact]
        public async Task Create_CorrectComment_CallsCreateAndSave()
        {
            // Arrange
            var validComment = new Comment { CommentText = "Valid Comment", UserId = 1, MemeId = 1 };

            // Act
            await _commentService.Create(validComment);

            // Assert
            _mockCommentRepository.Verify(x => x.Create(validComment), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Create_CommentWithValidParent_CallsCreateAndSave()
        {
            // Arrange
            var parentComment = new Comment { CommentId = 1, CommentText = "Parent", UserId = 1, MemeId = 1 };
            var replyComment = new Comment { CommentText = "Reply", UserId = 2, MemeId = 1, ParentCommentId = 1 };

            _mockCommentRepository.Setup(x => x.FindByCondition(It.Is<Expression<Func<Comment, bool>>>(e =>
                e.Compile()(parentComment))))
                .ReturnsAsync(new List<Comment> { parentComment });

            // Act
            await _commentService.Create(replyComment);

            // Assert
            _mockCommentRepository.Verify(x => x.Create(replyComment), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Create_ThrowsException_WhenRepositoryCreateThrows()
        {
            // Arrange
            var validComment = new Comment { CommentText = "Valid Comment", UserId = 1, MemeId = 1 };
            _mockCommentRepository.Setup(x => x.Create(validComment)).ThrowsAsync(new InvalidOperationException("DB Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _commentService.Create(validComment));
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_ThrowsException_WhenSaveThrows()
        {
            // Arrange
            var validComment = new Comment { CommentText = "Valid Comment", UserId = 1, MemeId = 1 };
            _mockRepositoryWrapper.Setup(x => x.Save()).ThrowsAsync(new InvalidOperationException("Save Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _commentService.Create(validComment));
        }
        #endregion

        #region UPDATE COMMENT TESTS
        [Fact]
        public async Task Update_NullComment_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _commentService.Update(null));
            _mockCommentRepository.Verify(x => x.Update(It.IsAny<Comment>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectComments))]
        public async Task Update_IncorrectComment_ThrowsArgumentException(Comment comment)
        {
            // Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _commentService.Update(comment));

            // Assert
            _mockCommentRepository.Verify(x => x.Update(It.IsAny<Comment>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task Update_ValidComment_CallsUpdateAndSave()
        {
            // Arrange
            var updatedComment = new Comment { CommentId = 1, CommentText = "Updated Comment", UserId = 1, MemeId = 1 };

            // Act
            await _commentService.Update(updatedComment);

            // Assert
            _mockCommentRepository.Verify(x => x.Update(updatedComment), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_ThrowsException_WhenRepositoryUpdateThrows()
        {
            // Arrange
            var updatedComment = new Comment { CommentId = 1, CommentText = "Updated Comment", UserId = 1, MemeId = 1 };
            _mockCommentRepository.Setup(x => x.Update(updatedComment)).ThrowsAsync(new InvalidOperationException("DB Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _commentService.Update(updatedComment));
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Update_ThrowsException_WhenSaveThrows()
        {
            // Arrange
            var updatedComment = new Comment { CommentId = 1, CommentText = "Updated Comment", UserId = 1, MemeId = 1 };
            _mockRepositoryWrapper.Setup(x => x.Save()).ThrowsAsync(new InvalidOperationException("Save Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _commentService.Update(updatedComment));
        }
        #endregion

        #region DELETE COMMENT TESTS
        [Fact]
        public async Task Delete_CommentExists_CallsDeleteAndSave()
        {
            // Arrange
            int deleteCommentId = 1;
            var existingComment = new Comment { CommentId = deleteCommentId, CommentText = "Delete Me", UserId = 1, MemeId = 1 };
            _mockCommentRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Comment, bool>>>()))
                .ReturnsAsync(new List<Comment> { existingComment });
            _mockCommentRepository.Setup(x => x.FindByCondition(It.Is<Expression<Func<Comment, bool>>>(e =>
                e.Compile()(new Comment { ParentCommentId = deleteCommentId }))))
                .ReturnsAsync(new List<Comment>());

            // Act
            await _commentService.Delete(deleteCommentId);

            // Assert
            _mockCommentRepository.Verify(x => x.Delete(existingComment), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_CommentWithReplies_ThrowsInvalidOperationException()
        {
            // Arrange
            int deleteCommentId = 1;
            var existingComment = new Comment { CommentId = deleteCommentId, CommentText = "Delete Me", UserId = 1, MemeId = 1 };
            var replies = new List<Comment>
            {
                new Comment { CommentId = 2, CommentText = "Reply", UserId = 2, MemeId = 1, ParentCommentId = deleteCommentId }
            };

            _mockCommentRepository.Setup(x => x.FindByCondition(It.Is<Expression<Func<Comment, bool>>>(e =>
                e.Compile()(existingComment))))
                .ReturnsAsync(new List<Comment> { existingComment });
            _mockCommentRepository.Setup(x => x.FindByCondition(It.Is<Expression<Func<Comment, bool>>>(e =>
                e.Compile()(new Comment { ParentCommentId = deleteCommentId }))))
                .ReturnsAsync(replies);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _commentService.Delete(deleteCommentId));
            _mockCommentRepository.Verify(x => x.Delete(It.IsAny<Comment>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_CommentDoesntExist_ThrowsInvalidOperationException()
        {
            // Arrange
            const int nonexistentCommentId = 999;
            _mockCommentRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Comment, bool>>>()))
                .ReturnsAsync(new List<Comment>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _commentService.Delete(nonexistentCommentId));
            _mockCommentRepository.Verify(x => x.Delete(It.IsAny<Comment>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_MultipleCommentsWithSameId_ThrowsInvalidOperationException()
        {
            // Arrange
            const int commentId = 1;
            var comments = new List<Comment>
            {
                new Comment { CommentId = commentId, CommentText = "Dup1", UserId = 1, MemeId = 1 },
                new Comment { CommentId = commentId, CommentText = "Dup2", UserId = 2, MemeId = 1 }
            };
            _mockCommentRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Comment, bool>>>()))
                .ReturnsAsync(comments);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _commentService.Delete(commentId));
            _mockCommentRepository.Verify(x => x.Delete(It.IsAny<Comment>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_ThrowsException_WhenFindByConditionThrows()
        {
            // Arrange
            const int commentId = 1;
            _mockCommentRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Comment, bool>>>()))
                .ThrowsAsync(new InvalidOperationException("DB Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _commentService.Delete(commentId));
            _mockCommentRepository.Verify(x => x.Delete(It.IsAny<Comment>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_ThrowsException_WhenDeleteThrows()
        {
            // Arrange
            int deleteCommentId = 1;
            var existingComment = new Comment { CommentId = deleteCommentId, CommentText = "Delete Me", UserId = 1, MemeId = 1 };
            _mockCommentRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Comment, bool>>>()))
                .ReturnsAsync(new List<Comment> { existingComment });
            _mockCommentRepository.Setup(x => x.FindByCondition(It.Is<Expression<Func<Comment, bool>>>(e =>
                e.Compile()(new Comment { ParentCommentId = deleteCommentId }))))
                .ReturnsAsync(new List<Comment>());
            _mockCommentRepository.Setup(x => x.Delete(existingComment)).ThrowsAsync(new InvalidOperationException("Delete Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _commentService.Delete(deleteCommentId));
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_ThrowsException_WhenSaveThrows()
        {
            // Arrange
            int deleteCommentId = 1;
            var existingComment = new Comment { CommentId = deleteCommentId, CommentText = "Delete Me", UserId = 1, MemeId = 1 };
            _mockCommentRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Comment, bool>>>()))
                .ReturnsAsync(new List<Comment> { existingComment });
            _mockCommentRepository.Setup(x => x.FindByCondition(It.Is<Expression<Func<Comment, bool>>>(e =>
                e.Compile()(new Comment { ParentCommentId = deleteCommentId }))))
                .ReturnsAsync(new List<Comment>());
            _mockRepositoryWrapper.Setup(x => x.Save()).ThrowsAsync(new InvalidOperationException("Save Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _commentService.Delete(deleteCommentId));
        }
        #endregion

        public static IEnumerable<object[]> GetIncorrectComments()
        {
            return new List<object[]>
            {
                new object[] { new Comment() { CommentText = "", UserId = 1, MemeId = 1 } },
                new object[] { new Comment() { CommentText = null, UserId = 1, MemeId = 1 } },
                new object[] { new Comment() { CommentText = "Valid Content", UserId = 0, MemeId = 1 } },
                new object[] { new Comment() { CommentText = "Valid Content", UserId = -1, MemeId = 1 } },
                new object[] { new Comment() { CommentText = "Valid Content", UserId = 1, MemeId = 0 } },
                new object[] { new Comment() { CommentText = "Valid Content", UserId = 1, MemeId = -1 } }
            };
        }

        public static IEnumerable<object[]> GetWhitespaceComments()
        {
            return new List<object[]>
            {
                new object[] { new Comment() { CommentText = " ", UserId = 1, MemeId = 1 } },
                new object[] { new Comment() { CommentText = "   ", UserId = 1, MemeId = 1 } }
            };
        }
    }
}
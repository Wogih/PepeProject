using BusinessLogic.Services;
using Domain.Interfaces;
using Domain.Interfaces.ITag;
using Domain.Models;
using Moq;
using System.Linq.Expressions;

namespace BusinessLogic.Tests
{
    public class TagServiceTest
    {
        private readonly TagService _tagService;
        private readonly Mock<ITagRepository> _mockTagRepository;
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;

        public TagServiceTest()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockTagRepository = new Mock<ITagRepository>();

            _mockRepositoryWrapper.Setup(x => x.Tag).Returns(_mockTagRepository.Object);
            _mockRepositoryWrapper.Setup(x => x.Save()).Returns(Task.CompletedTask);

            _tagService = new TagService(_mockRepositoryWrapper.Object);
        }

        #region GET ALL TAGS TESTS

        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoTags()
        {
            // Arrange
            var emptyList = new List<Tag>();
            _mockTagRepository.Setup(x => x.FindAll()).ReturnsAsync(emptyList);

            // Act
            var result = await _tagService.GetAll();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAll_ReturnsListTags_WhenTagsExist()
        {
            // Arrange
            var tags = new List<Tag>()
            {
                new Tag() { TagId = 1, TagName = "TestTag" },
                new Tag() { TagId = 2, TagName = "TestTag2" }
            };
            _mockTagRepository.Setup(x => x.FindAll()).ReturnsAsync(tags);

            // Act
            var result = await _tagService.GetAll();

            // Assert
            Assert.Equal(tags, result);
        }

        [Fact]
        public async Task GetAll_ThrowsException_WhenRepositoryThrows()
        {
            // Arrange
            _mockTagRepository.Setup(x => x.FindAll()).ThrowsAsync(new InvalidOperationException("DB Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _tagService.GetAll());
        }
        #endregion

        #region GET BY ID TESTS
        [Fact]
        public async Task GetById_TagExists_ReturnsCorrectTag()
        {
            // Arrange
            var existingTag = new Tag { TagId = 1, TagName = "ExistingTag" };
            _mockTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Tag, bool>>>()))
                .ReturnsAsync(new List<Tag> { existingTag });

            // Act
            var result = await _tagService.GetById(existingTag.TagId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingTag.TagId, result.TagId);
            Assert.Equal(existingTag.TagName, result.TagName);
        }

        [Fact]
        public async Task GetById_TagDoesntExist_ThrowsInvalidOperationException()
        {
            // Arrange
            const int nonexistentTagId = 999;
            _mockTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Tag, bool>>>()))
                .ReturnsAsync(new List<Tag>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _tagService.GetById(nonexistentTagId));
        }

        [Fact]
        public async Task GetById_MultipleTagsWithSameId_ThrowsInvalidOperationException()
        {
            // Arrange
            const int tagId = 1;
            var tags = new List<Tag>
            {
                new Tag { TagId = tagId, TagName = "Dup1" },
                new Tag { TagId = tagId, TagName = "Dup2" }
            };
            _mockTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Tag, bool>>>()))
                .ReturnsAsync(tags);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _tagService.GetById(tagId));
        }

        [Fact]
        public async Task GetById_ThrowsException_WhenRepositoryThrows()
        {
            // Arrange
            const int tagId = 1;
            _mockTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Tag, bool>>>()))
                .ThrowsAsync(new InvalidOperationException("DB Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _tagService.GetById(tagId));
        }
        #endregion

        #region CREATE TAG TESTS
        [Fact]
        public async Task Create_NullTag_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _tagService.Create(null));
            _mockTagRepository.Verify(x => x.Create(It.IsAny<Tag>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectTags))]
        public async Task Create_IncorrectTag_ThrowsArgumentException(Tag tag)
        {
            // Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _tagService.Create(tag));

            // Assert
            _mockTagRepository.Verify(x => x.Create(It.IsAny<Tag>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Theory]
        [MemberData(nameof(GetWhitespaceTags))]
        public async Task Create_WhitespaceTag_ThrowsArgumentException(Tag tag)
        {
            // Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _tagService.Create(tag));

            // Assert
            _mockTagRepository.Verify(x => x.Create(It.IsAny<Tag>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task Create_CorrectTag_CallsCreateAndSave()
        {
            // Arrange
            var validTag = new Tag { TagName = "ValidTag" };

            // Act
            await _tagService.Create(validTag);

            // Assert
            _mockTagRepository.Verify(x => x.Create(validTag), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Create_ThrowsException_WhenRepositoryCreateThrows()
        {
            // Arrange
            var validTag = new Tag { TagName = "ValidTag" };
            _mockTagRepository.Setup(x => x.Create(validTag)).ThrowsAsync(new InvalidOperationException("DB Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _tagService.Create(validTag));
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_ThrowsException_WhenSaveThrows()
        {
            // Arrange
            var validTag = new Tag { TagName = "ValidTag" };
            _mockRepositoryWrapper.Setup(x => x.Save()).ThrowsAsync(new InvalidOperationException("Save Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _tagService.Create(validTag));
        }
        #endregion

        #region UPDATE TAG TESTS
        [Fact]
        public async Task Update_NullTag_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _tagService.Update(null));
            _mockTagRepository.Verify(x => x.Update(It.IsAny<Tag>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectTags))]
        public async Task Update_IncorrectTag_ThrowsArgumentException(Tag tag)
        {
            // Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _tagService.Update(tag));

            // Assert
            _mockTagRepository.Verify(x => x.Update(It.IsAny<Tag>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task Update_ValidTag_CallsUpdateAndSave()
        {
            // Arrange
            var updatedTag = new Tag { TagId = 1, TagName = "UpdatedTagName" };

            // Act
            await _tagService.Update(updatedTag);

            // Assert
            _mockTagRepository.Verify(x => x.Update(updatedTag), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_ThrowsException_WhenRepositoryUpdateThrows()
        {
            // Arrange
            var updatedTag = new Tag { TagId = 1, TagName = "UpdatedTagName" };
            _mockTagRepository.Setup(x => x.Update(updatedTag)).ThrowsAsync(new InvalidOperationException("DB Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _tagService.Update(updatedTag));
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Update_ThrowsException_WhenSaveThrows()
        {
            // Arrange
            var updatedTag = new Tag { TagId = 1, TagName = "UpdatedTagName" };
            _mockRepositoryWrapper.Setup(x => x.Save()).ThrowsAsync(new InvalidOperationException("Save Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _tagService.Update(updatedTag));
        }
        #endregion

        #region DELETE TAG TESTS
        [Fact]
        public async Task Delete_TagExists_CallsDeleteAndSave()
        {
            // Arrange
            int deleteTagId = 1;
            var existingTag = new Tag { TagId = deleteTagId, TagName = "DeleteMe" };
            _mockTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Tag, bool>>>()))
                .ReturnsAsync(new List<Tag> { existingTag });

            // Act
            await _tagService.Delete(deleteTagId);

            // Assert
            _mockTagRepository.Verify(x => x.Delete(existingTag), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_TagDoesntExist_ThrowsInvalidOperationException()
        {
            // Arrange
            const int nonexistentTagId = 999;
            _mockTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Tag, bool>>>()))
                .ReturnsAsync(new List<Tag>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _tagService.Delete(nonexistentTagId));
            _mockTagRepository.Verify(x => x.Delete(It.IsAny<Tag>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_MultipleTagsWithSameId_ThrowsInvalidOperationException()
        {
            // Arrange
            const int tagId = 1;
            var tags = new List<Tag>
            {
                new Tag { TagId = tagId, TagName = "Dup1" },
                new Tag { TagId = tagId, TagName = "Dup2" }
            };
            _mockTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Tag, bool>>>()))
                .ReturnsAsync(tags);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _tagService.Delete(tagId));
            _mockTagRepository.Verify(x => x.Delete(It.IsAny<Tag>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_ThrowsException_WhenFindByConditionThrows()
        {
            // Arrange
            const int tagId = 1;
            _mockTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Tag, bool>>>()))
                .ThrowsAsync(new InvalidOperationException("DB Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _tagService.Delete(tagId));
            _mockTagRepository.Verify(x => x.Delete(It.IsAny<Tag>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_ThrowsException_WhenDeleteThrows()
        {
            // Arrange
            int deleteTagId = 1;
            var existingTag = new Tag { TagId = deleteTagId, TagName = "DeleteMe" };
            _mockTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Tag, bool>>>()))
                .ReturnsAsync(new List<Tag> { existingTag });
            _mockTagRepository.Setup(x => x.Delete(existingTag)).ThrowsAsync(new InvalidOperationException("Delete Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _tagService.Delete(deleteTagId));
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_ThrowsException_WhenSaveThrows()
        {
            // Arrange
            int deleteTagId = 1;
            var existingTag = new Tag { TagId = deleteTagId, TagName = "DeleteMe" };
            _mockTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Tag, bool>>>()))
                .ReturnsAsync(new List<Tag> { existingTag });
            _mockRepositoryWrapper.Setup(x => x.Save()).ThrowsAsync(new InvalidOperationException("Save Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _tagService.Delete(deleteTagId));
        }
        #endregion

        public static IEnumerable<object[]> GetIncorrectTags()
        {
            return new List<object[]>
            {
                new object[] { new Tag() { TagName = "" } },
                new object[] { new Tag() { TagName = null } }
            };
        }

        public static IEnumerable<object[]> GetWhitespaceTags()
        {
            return new List<object[]>
            {
                new object[] { new Tag() { TagName = " " } },
                new object[] { new Tag() { TagName = "   " } }
            };
        }
    }
}
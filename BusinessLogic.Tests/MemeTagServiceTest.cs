using BusinessLogic.Services;
using Domain.Interfaces;
using Domain.Interfaces.IMeme;
using Domain.Interfaces.IMemeTag;
using Domain.Interfaces.ITag;
using Domain.Models;
using Moq;
using System.Linq.Expressions;

namespace BusinessLogic.Tests
{
    public class MemeTagServiceTest
    {
        private readonly MemeTagService _memeTagService;
        private readonly Mock<IMemeTagRepository> _mockMemeTagRepository;
        private readonly Mock<IMemeRepository> _mockMemeRepository;
        private readonly Mock<ITagRepository> _mockTagRepository;
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;

        public MemeTagServiceTest()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockMemeTagRepository = new Mock<IMemeTagRepository>();
            _mockMemeRepository = new Mock<IMemeRepository>();
            _mockTagRepository = new Mock<ITagRepository>();

            _mockRepositoryWrapper.Setup(x => x.MemeTag).Returns(_mockMemeTagRepository.Object);
            _mockRepositoryWrapper.Setup(x => x.Meme).Returns(_mockMemeRepository.Object);
            _mockRepositoryWrapper.Setup(x => x.Tag).Returns(_mockTagRepository.Object);
            _mockRepositoryWrapper.Setup(x => x.Save()).Returns(Task.CompletedTask);

            _memeTagService = new MemeTagService(_mockRepositoryWrapper.Object);
        }

        #region GET ALL MEME TAGS TESTS

        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoMemeTags()
        {
            // Arrange
            var emptyList = new List<MemeTag>();
            _mockMemeTagRepository.Setup(x => x.FindAll()).ReturnsAsync(emptyList);

            // Act
            var result = await _memeTagService.GetAll();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAll_ReturnsListMemeTags_WhenMemeTagsExist()
        {
            // Arrange
            var memeTags = new List<MemeTag>()
            {
                new MemeTag() { MemeTagId = 1, MemeId = 1, TagId = 1 },
                new MemeTag() { MemeTagId = 2, MemeId = 1, TagId = 2 }
            };
            _mockMemeTagRepository.Setup(x => x.FindAll()).ReturnsAsync(memeTags);

            // Act
            var result = await _memeTagService.GetAll();

            // Assert
            Assert.Equal(memeTags, result);
        }

        [Fact]
        public async Task GetAll_ThrowsException_WhenRepositoryThrows()
        {
            // Arrange
            _mockMemeTagRepository.Setup(x => x.FindAll()).ThrowsAsync(new InvalidOperationException("DB Error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeTagService.GetAll());
        }
        #endregion

        #region GET BY ID TESTS
        [Fact]
        public async Task GetById_MemeTagExists_ReturnsCorrectMemeTag()
        {
            // Arrange
            var existingMemeTag = new MemeTag { MemeTagId = 1, MemeId = 1, TagId = 1 };
            _mockMemeTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeTag, bool>>>()))
                .ReturnsAsync(new List<MemeTag> { existingMemeTag });

            // Act
            var result = await _memeTagService.GetById(existingMemeTag.MemeTagId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingMemeTag.MemeTagId, result.MemeTagId);
            Assert.Equal(existingMemeTag.MemeId, result.MemeId);
            Assert.Equal(existingMemeTag.TagId, result.TagId);
        }

        [Fact]
        public async Task GetById_MemeTagDoesntExist_ThrowsInvalidOperationException()
        {
            // Arrange
            const int nonexistentMemeTagId = 999;
            _mockMemeTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeTag, bool>>>()))
                .ReturnsAsync(new List<MemeTag>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeTagService.GetById(nonexistentMemeTagId));
        }
        #endregion

        #region GET BY MEME ID AND TAG ID TESTS
        [Fact]
        public async Task GetByMemeId_ReturnsMemeTags_WhenTheyExist()
        {
            // Arrange
            const int memeId = 1;
            var memeTags = new List<MemeTag>
            {
                new MemeTag { MemeTagId = 1, MemeId = memeId, TagId = 1 },
                new MemeTag { MemeTagId = 2, MemeId = memeId, TagId = 2 }
            };
            _mockMemeTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeTag, bool>>>()))
                .ReturnsAsync(memeTags);

            // Act
            var result = await _memeTagService.GetByMemeId(memeId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, mt => Assert.Equal(memeId, mt.MemeId));
        }

        [Fact]
        public async Task GetByTagId_ReturnsMemeTags_WhenTheyExist()
        {
            // Arrange
            const int tagId = 1;
            var memeTags = new List<MemeTag>
            {
                new MemeTag { MemeTagId = 1, MemeId = 1, TagId = tagId },
                new MemeTag { MemeTagId = 2, MemeId = 2, TagId = tagId }
            };
            _mockMemeTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeTag, bool>>>()))
                .ReturnsAsync(memeTags);

            // Act
            var result = await _memeTagService.GetByTagId(tagId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, mt => Assert.Equal(tagId, mt.TagId));
        }

        [Fact]
        public async Task GetByMemeAndTag_Exists_ReturnsMemeTag()
        {
            // Arrange
            const int memeId = 1;
            const int tagId = 1;
            var existingMemeTag = new MemeTag { MemeTagId = 1, MemeId = memeId, TagId = tagId };
            _mockMemeTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeTag, bool>>>()))
                .ReturnsAsync(new List<MemeTag> { existingMemeTag });

            // Act
            var result = await _memeTagService.GetByMemeAndTag(memeId, tagId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(memeId, result.MemeId);
            Assert.Equal(tagId, result.TagId);
        }
        #endregion

        #region EXISTS AND COUNT TESTS
        [Fact]
        public async Task ExistsForMemeAndTag_Exists_ReturnsTrue()
        {
            // Arrange
            const int memeId = 1;
            const int tagId = 1;
            var existingMemeTag = new MemeTag { MemeTagId = 1, MemeId = memeId, TagId = tagId };
            _mockMemeTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeTag, bool>>>()))
                .ReturnsAsync(new List<MemeTag> { existingMemeTag });

            // Act
            bool result = await _memeTagService.ExistsForMemeAndTag(memeId, tagId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetTagCountForMeme_ReturnsCorrectCount()
        {
            // Arrange
            const int memeId = 1;
            var memeTags = new List<MemeTag>
            {
                new MemeTag { MemeTagId = 1, MemeId = memeId, TagId = 1 },
                new MemeTag { MemeTagId = 2, MemeId = memeId, TagId = 2 },
                new MemeTag { MemeTagId = 3, MemeId = memeId, TagId = 3 }
            };
            _mockMemeTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeTag, bool>>>()))
                .ReturnsAsync(memeTags);

            // Act
            int result = await _memeTagService.GetTagCountForMeme(memeId);

            // Assert
            Assert.Equal(3, result);
        }

        [Fact]
        public async Task GetMemeCountForTag_ReturnsCorrectCount()
        {
            // Arrange
            const int tagId = 1;
            var memeTags = new List<MemeTag>
            {
                new MemeTag { MemeTagId = 1, MemeId = 1, TagId = tagId },
                new MemeTag { MemeTagId = 2, MemeId = 2, TagId = tagId },
                new MemeTag { MemeTagId = 3, MemeId = 3, TagId = tagId }
            };
            _mockMemeTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeTag, bool>>>()))
                .ReturnsAsync(memeTags);

            // Act
            int result = await _memeTagService.GetMemeCountForTag(tagId);

            // Assert
            Assert.Equal(3, result);
        }
        #endregion

        #region GET IDS TESTS
        [Fact]
        public async Task GetTagIdsForMeme_ReturnsCorrectIds()
        {
            // Arrange
            const int memeId = 1;
            var memeTags = new List<MemeTag>
            {
                new MemeTag { MemeTagId = 1, MemeId = memeId, TagId = 1 },
                new MemeTag { MemeTagId = 2, MemeId = memeId, TagId = 2 },
                new MemeTag { MemeTagId = 3, MemeId = memeId, TagId = 3 }
            };
            _mockMemeTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeTag, bool>>>()))
                .ReturnsAsync(memeTags);

            // Act
            var result = await _memeTagService.GetTagIdsForMeme(memeId);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains(1, result);
            Assert.Contains(2, result);
            Assert.Contains(3, result);
        }

        [Fact]
        public async Task GetMemeIdsForTag_ReturnsCorrectIds()
        {
            // Arrange
            const int tagId = 1;
            var memeTags = new List<MemeTag>
            {
                new MemeTag { MemeTagId = 1, MemeId = 1, TagId = tagId },
                new MemeTag { MemeTagId = 2, MemeId = 2, TagId = tagId },
                new MemeTag { MemeTagId = 3, MemeId = 3, TagId = tagId }
            };
            _mockMemeTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeTag, bool>>>()))
                .ReturnsAsync(memeTags);

            // Act
            var result = await _memeTagService.GetMemeIdsForTag(tagId);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains(1, result);
            Assert.Contains(2, result);
            Assert.Contains(3, result);
        }
        #endregion

        #region CREATE MEME TAG TESTS
        [Fact]
        public async Task Create_NullMemeTag_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _memeTagService.Create(null));
            _mockMemeTagRepository.Verify(x => x.Create(It.IsAny<MemeTag>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectMemeTags))]
        public async Task Create_IncorrectMemeTag_ThrowsArgumentException(MemeTag memeTag)
        {
            // Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _memeTagService.Create(memeTag));

            // Assert
            _mockMemeTagRepository.Verify(x => x.Create(It.IsAny<MemeTag>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task Create_NonExistentMeme_ThrowsInvalidOperationException()
        {
            // Arrange
            var memeTag = new MemeTag { MemeId = 999, TagId = 1 };
            _mockMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>()))
                .ReturnsAsync(new List<Meme>());
            _mockTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Tag, bool>>>()))
                .ReturnsAsync(new List<Tag> { new Tag() });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeTagService.Create(memeTag));
            _mockMemeTagRepository.Verify(x => x.Create(It.IsAny<MemeTag>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_NonExistentTag_ThrowsInvalidOperationException()
        {
            // Arrange
            var memeTag = new MemeTag { MemeId = 1, TagId = 999 };
            _mockMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>()))
                .ReturnsAsync(new List<Meme> { new Meme() });
            _mockTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Tag, bool>>>()))
                .ReturnsAsync(new List<Tag>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeTagService.Create(memeTag));
            _mockMemeTagRepository.Verify(x => x.Create(It.IsAny<MemeTag>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_DuplicateMemeTag_ThrowsInvalidOperationException()
        {
            // Arrange
            var existingMemeTag = new MemeTag { MemeTagId = 1, MemeId = 1, TagId = 1 };
            var newMemeTag = new MemeTag { MemeId = 1, TagId = 1 };

            _mockMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>()))
                .ReturnsAsync(new List<Meme> { new Meme() });
            _mockTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Tag, bool>>>()))
                .ReturnsAsync(new List<Tag> { new Tag() });
            _mockMemeTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeTag, bool>>>()))
                .ReturnsAsync(new List<MemeTag> { existingMemeTag });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeTagService.Create(newMemeTag));
            _mockMemeTagRepository.Verify(x => x.Create(It.IsAny<MemeTag>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_CorrectMemeTag_CallsCreateAndSave()
        {
            // Arrange
            var validMemeTag = new MemeTag { MemeId = 1, TagId = 1 };
            _mockMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>()))
                .ReturnsAsync(new List<Meme> { new Meme() });
            _mockTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Tag, bool>>>()))
                .ReturnsAsync(new List<Tag> { new Tag() });
            _mockMemeTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeTag, bool>>>()))
                .ReturnsAsync(new List<MemeTag>());

            // Act
            await _memeTagService.Create(validMemeTag);

            // Assert
            _mockMemeTagRepository.Verify(x => x.Create(validMemeTag), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task AddTagToMeme_ValidParameters_CallsCreate()
        {
            // Arrange
            const int memeId = 1;
            const int tagId = 1;
            _mockMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>()))
                .ReturnsAsync(new List<Meme> { new Meme() });
            _mockTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Tag, bool>>>()))
                .ReturnsAsync(new List<Tag> { new Tag() });
            _mockMemeTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeTag, bool>>>()))
                .ReturnsAsync(new List<MemeTag>());

            // Act
            await _memeTagService.AddTagToMeme(memeId, tagId);

            // Assert
            _mockMemeTagRepository.Verify(x => x.Create(It.Is<MemeTag>(mt =>
                mt.MemeId == memeId && mt.TagId == tagId)), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task AddTagsToMeme_ValidParameters_CallsCreateForEachTag()
        {
            // Arrange
            const int memeId = 1;
            var tagIds = new List<int> { 1, 2, 3 };
            _mockMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>()))
                .ReturnsAsync(new List<Meme> { new Meme() });
            _mockTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Tag, bool>>>()))
                .ReturnsAsync(new List<Tag> { new Tag() });
            _mockMemeTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeTag, bool>>>()))
                .ReturnsAsync(new List<MemeTag>());

            // Act
            await _memeTagService.AddTagsToMeme(memeId, tagIds);

            // Assert
            _mockMemeTagRepository.Verify(x => x.Create(It.IsAny<MemeTag>()), Times.Exactly(3));
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Exactly(3));
        }
        #endregion

        #region DELETE MEME TAG TESTS
        [Fact]
        public async Task Delete_MemeTagExists_CallsDeleteAndSave()
        {
            // Arrange
            int deleteMemeTagId = 1;
            var existingMemeTag = new MemeTag { MemeTagId = deleteMemeTagId, MemeId = 1, TagId = 1 };
            _mockMemeTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeTag, bool>>>()))
                .ReturnsAsync(new List<MemeTag> { existingMemeTag });

            // Act
            await _memeTagService.Delete(deleteMemeTagId);

            // Assert
            _mockMemeTagRepository.Verify(x => x.Delete(existingMemeTag), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task RemoveTagFromMeme_ValidParameters_CallsDeleteAndSave()
        {
            // Arrange
            const int memeId = 1;
            const int tagId = 1;
            var existingMemeTag = new MemeTag { MemeTagId = 1, MemeId = memeId, TagId = tagId };
            _mockMemeTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeTag, bool>>>()))
                .ReturnsAsync(new List<MemeTag> { existingMemeTag });

            // Act
            await _memeTagService.RemoveTagFromMeme(memeId, tagId);

            // Assert
            _mockMemeTagRepository.Verify(x => x.Delete(existingMemeTag), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task ClearTagsFromMeme_WithTags_DeletesAllAndSaves()
        {
            // Arrange
            const int memeId = 1;
            var memeTags = new List<MemeTag>
            {
                new MemeTag { MemeTagId = 1, MemeId = memeId, TagId = 1 },
                new MemeTag { MemeTagId = 2, MemeId = memeId, TagId = 2 }
            };
            _mockMemeTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeTag, bool>>>()))
                .ReturnsAsync(memeTags);

            // Act
            await _memeTagService.ClearTagsFromMeme(memeId);

            // Assert
            _mockMemeTagRepository.Verify(x => x.Delete(It.IsAny<MemeTag>()), Times.Exactly(2));
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task ClearMemesFromTag_WithMemes_DeletesAllAndSaves()
        {
            // Arrange
            const int tagId = 1;
            var memeTags = new List<MemeTag>
            {
                new MemeTag { MemeTagId = 1, MemeId = 1, TagId = tagId },
                new MemeTag { MemeTagId = 2, MemeId = 2, TagId = tagId }
            };
            _mockMemeTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeTag, bool>>>()))
                .ReturnsAsync(memeTags);

            // Act
            await _memeTagService.ClearMemesFromTag(tagId);

            // Assert
            _mockMemeTagRepository.Verify(x => x.Delete(It.IsAny<MemeTag>()), Times.Exactly(2));
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }
        #endregion

        #region GET RELATED ENTITIES TESTS
        [Fact]
        public async Task GetMemesByTag_ReturnsMemes()
        {
            // Arrange
            const int tagId = 1;
            var memeTags = new List<MemeTag>
            {
                new MemeTag { MemeTagId = 1, MemeId = 1, TagId = tagId },
                new MemeTag { MemeTagId = 2, MemeId = 2, TagId = tagId }
            };
            var memes = new List<Meme>
            {
                new Meme { MemeId = 1, Title = "Meme 1" },
                new Meme { MemeId = 2, Title = "Meme 2" }
            };

            _mockMemeTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeTag, bool>>>()))
                .ReturnsAsync(memeTags);
            _mockMemeRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>()))
                .ReturnsAsync(memes);

            // Act
            var result = await _memeTagService.GetMemesByTag(tagId);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetTagsByMeme_ReturnsTags()
        {
            // Arrange
            const int memeId = 1;
            var memeTags = new List<MemeTag>
            {
                new MemeTag { MemeTagId = 1, MemeId = memeId, TagId = 1 },
                new MemeTag { MemeTagId = 2, MemeId = memeId, TagId = 2 }
            };
            var tags = new List<Tag>
            {
                new Tag { TagId = 1, TagName = "Tag 1" },
                new Tag { TagId = 2, TagName = "Tag 2" }
            };

            _mockMemeTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<MemeTag, bool>>>()))
                .ReturnsAsync(memeTags);
            _mockTagRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Tag, bool>>>()))
                .ReturnsAsync(tags);

            // Act
            var result = await _memeTagService.GetTagsByMeme(memeId);

            // Assert
            Assert.Equal(2, result.Count);
        }
        #endregion

        public static IEnumerable<object[]> GetIncorrectMemeTags()
        {
            return new List<object[]>
            {
                new object[] { new MemeTag() { MemeId = 0, TagId = 1 } },
                new object[] { new MemeTag() { MemeId = -1, TagId = 1 } },
                new object[] { new MemeTag() { MemeId = 1, TagId = 0 } },
                new object[] { new MemeTag() { MemeId = 1, TagId = -1 } }
            };
        }
    }
}
using BusinessLogic.Services;
using Domain.Interfaces;
using Domain.Interfaces.IMeme;
using Domain.Models;
using Moq;
using System.Linq.Expressions;

namespace BusinessLogic.Tests
{
    public class MemeServiceTest
    {
        private readonly MemeService _memeService;
        private readonly Mock<IMemeRepository> _memeRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> _repositoryWrapperMoq;

        public MemeServiceTest()
        {
            _repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            _memeRepositoryMoq = new Mock<IMemeRepository>();

            _repositoryWrapperMoq.Setup(x => x.Meme).Returns(_memeRepositoryMoq.Object);
            _repositoryWrapperMoq.Setup(x => x.Save()).Returns(Task.CompletedTask);

            _memeService = new MemeService(_repositoryWrapperMoq.Object);
        }

        #region GET ALL MEMES TESTS
        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoMemes()
        {
            // arrange
            var emptyList = new List<Meme>();
            _memeRepositoryMoq.Setup(x => x.FindAll()).ReturnsAsync(emptyList);

            // act
            var result = await _memeService.GetAll();

            // assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAll_ReturnsListOfMemes_WhenMemesExist()
        {
            // arrange
            var memes = new List<Meme>
            {
                new Meme { MemeId = 1, Title = "Meme1", ImageUrl = "http://meme1.com" },
                new Meme { MemeId = 2, Title = "Meme2", ImageUrl = "http://meme2.com" }
            };

            _memeRepositoryMoq.Setup(x => x.FindAll()).ReturnsAsync(memes);

            // act
            var result = await _memeService.GetAll();

            // assert
            Assert.Equal(memes.Count, result.Count);
            foreach (var expected in memes)
            {
                Assert.Contains(result, actual => actual.MemeId == expected.MemeId && actual.Title == expected.Title);
            }
        }

        [Fact]
        public async Task GetAll_ThrowsException_WhenRepositoryThrows()
        {
            // arrange
            _memeRepositoryMoq.Setup(x => x.FindAll()).ThrowsAsync(new InvalidOperationException("DB Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeService.GetAll());
        }
        #endregion

        #region GET BY ID TESTS
        [Fact]
        public async Task GetById_MemeExists_ReturnsCorrectMeme()
        {
            // arrange
            var existingMeme = new Meme { MemeId = 1, Title = "ExistingMeme", ImageUrl = "http://existing.com" };
            _memeRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>())).ReturnsAsync(new List<Meme> { existingMeme });

            // act
            var result = await _memeService.GetById(existingMeme.MemeId);

            // assert
            Assert.NotNull(result);
            Assert.Equal(existingMeme.MemeId, result.MemeId);
            Assert.Equal(existingMeme.Title, result.Title);
        }

        [Fact]
        public async Task GetById_InvalidId_ThrowsArgumentException()
        {
            // arrange
            const int invalidId = 0;

            // act & assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await _memeService.GetById(invalidId));
            Assert.Equal("Invalid id.", exception.Message);

            // Verify
            _memeRepositoryMoq.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>()), Times.Never);
        }

        [Fact]
        public async Task GetById_MemeDoesntExist_ThrowsInvalidOperationException()
        {
            // arrange
            const int nonexistentMemeId = 999;
            _memeRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>())).ReturnsAsync(new List<Meme>());

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeService.GetById(nonexistentMemeId));
        }

        [Fact]
        public async Task GetById_MultipleMemesWithSameId_ThrowsInvalidOperationException()
        {
            // arrange
            const int memeId = 1;
            var memes = new List<Meme>
            {
                new Meme { MemeId = memeId, Title = "Dup1", ImageUrl = "http://dup1.com" },
                new Meme { MemeId = memeId, Title = "Dup2", ImageUrl = "http://dup2.com" }
            };
            _memeRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>())).ReturnsAsync(memes);

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeService.GetById(memeId));
        }

        [Fact]
        public async Task GetById_ThrowsException_WhenRepositoryThrows()
        {
            // arrange
            const int memeId = 1;
            _memeRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>())).ThrowsAsync(new InvalidOperationException("DB Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeService.GetById(memeId));
        }
        #endregion

        #region CREATE MEME TESTS
        [Fact]
        public async Task Create_NullMeme_ThrowsArgumentNullException()
        {
            // act
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _memeService.Create(null));

            // assert
            Assert.IsType<ArgumentNullException>(ex);
            _memeRepositoryMoq.Verify(x => x.Create(It.IsAny<Meme>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectMemes))]
        public async Task Create_IncorrectMeme_ThrowsArgumentException(Meme meme)
        {
            // arrange
            var newMeme = meme;

            // act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _memeService.Create(newMeme));

            // assert
            _memeRepositoryMoq.Verify(x => x.Create(It.IsAny<Meme>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Theory]
        [MemberData(nameof(GetWhitespaceMemes))]
        public async Task Create_WhitespaceMeme_ThrowsArgumentException(Meme meme)
        {
            // arrange
            var newMeme = meme;

            // act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _memeService.Create(newMeme));

            // assert
            _memeRepositoryMoq.Verify(x => x.Create(It.IsAny<Meme>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task Create_ValidMeme_CallsCreateAndSave()
        {
            // arrange
            var validMeme = new Meme { MemeId = 1, Title = "ValidMeme", ImageUrl = "http://valid.com" };

            // act
            await _memeService.Create(validMeme);

            // assert
            _memeRepositoryMoq.Verify(x => x.Create(validMeme), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Create_ThrowsException_WhenRepositoryCreateThrows()
        {
            // arrange
            var validMeme = new Meme { MemeId = 1, Title = "ValidMeme", ImageUrl = "http://valid.com" };
            _memeRepositoryMoq.Setup(x => x.Create(validMeme)).ThrowsAsync(new InvalidOperationException("DB Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeService.Create(validMeme));
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_ThrowsException_WhenSaveThrows()
        {
            // arrange
            var validMeme = new Meme { MemeId = 1, Title = "ValidMeme", ImageUrl = "http://valid.com" };
            _repositoryWrapperMoq.Setup(x => x.Save()).ThrowsAsync(new InvalidOperationException("Save Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeService.Create(validMeme));
        }
        #endregion

        #region UPDATE MEME TESTS
        [Fact]
        public async Task Update_NullMeme_ThrowsArgumentNullException()
        {
            // act & assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _memeService.Update(null));
            _memeRepositoryMoq.Verify(x => x.Update(It.IsAny<Meme>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectMemesForUpdate))]
        public async Task Update_IncorrectMeme_ThrowsArgumentException(Meme meme)
        {
            // arrange
            var updatedMeme = meme;

            // act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _memeService.Update(updatedMeme));

            // assert
            _memeRepositoryMoq.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>()), Times.Never);
            _memeRepositoryMoq.Verify(x => x.Update(It.IsAny<Meme>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Theory]
        [MemberData(nameof(GetWhitespaceMemesForUpdate))]
        public async Task Update_WhitespaceMeme_ThrowsArgumentException(Meme meme)
        {
            // arrange
            var updatedMeme = meme;

            // act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _memeService.Update(updatedMeme));

            // assert
            _memeRepositoryMoq.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>()), Times.Never);
            _memeRepositoryMoq.Verify(x => x.Update(It.IsAny<Meme>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task Update_MemeDoesntExist_ThrowsInvalidOperationException()
        {
            // arrange
            var nonExistingMeme = new Meme { MemeId = 999, Title = "NonExisting", ImageUrl = "http://nonexisting.com" };
            _memeRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>())).ReturnsAsync(new List<Meme>());

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeService.Update(nonExistingMeme));

            // Verify
            _memeRepositoryMoq.Verify(x => x.Update(It.IsAny<Meme>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Update_MultipleMemesWithSameId_ThrowsInvalidOperationException()
        {
            // arrange
            const int memeId = 1;
            var updatedMeme = new Meme { MemeId = memeId, Title = "UpdatedMeme", ImageUrl = "http://updated.com" };
            var memes = new List<Meme>
            {
                new Meme { MemeId = memeId, Title = "Dup1", ImageUrl = "http://dup1.com" },
                new Meme { MemeId = memeId, Title = "Dup2", ImageUrl = "http://dup2.com" }
            };
            _memeRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>())).ReturnsAsync(memes);

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeService.Update(updatedMeme));

            // Verify
            _memeRepositoryMoq.Verify(x => x.Update(It.IsAny<Meme>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Update_ValidMeme_CallsUpdateAndSave()
        {
            // arrange
            var existingMeme = new Meme { MemeId = 1, Title = "OldTitle", ImageUrl = "http://old.com" };
            var updatedMeme = new Meme { MemeId = 1, Title = "UpdatedMeme", ImageUrl = "http://updated.com" };
            _memeRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>())).ReturnsAsync(new List<Meme> { existingMeme });

            // act
            await _memeService.Update(updatedMeme);

            // assert
            _memeRepositoryMoq.Verify(x => x.Update(updatedMeme), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_ThrowsException_WhenRepositoryUpdateThrows()
        {
            // arrange
            var existingMeme = new Meme { MemeId = 1, Title = "OldTitle", ImageUrl = "http://old.com" };
            var updatedMeme = new Meme { MemeId = 1, Title = "UpdatedMeme", ImageUrl = "http://updated.com" };
            _memeRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>())).ReturnsAsync(new List<Meme> { existingMeme });
            _memeRepositoryMoq.Setup(x => x.Update(updatedMeme)).ThrowsAsync(new InvalidOperationException("DB Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeService.Update(updatedMeme));
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Update_ThrowsException_WhenSaveThrows()
        {
            // arrange
            var existingMeme = new Meme { MemeId = 1, Title = "OldTitle", ImageUrl = "http://old.com" };
            var updatedMeme = new Meme { MemeId = 1, Title = "UpdatedMeme", ImageUrl = "http://updated.com" };
            _memeRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>())).ReturnsAsync(new List<Meme> { existingMeme });
            _repositoryWrapperMoq.Setup(x => x.Save()).ThrowsAsync(new InvalidOperationException("Save Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeService.Update(updatedMeme));
        }

        [Fact]
        public async Task Update_ThrowsException_WhenFindByConditionThrows()
        {
            // arrange
            var updatedMeme = new Meme { MemeId = 1, Title = "UpdatedMeme", ImageUrl = "http://updated.com" };
            _memeRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>())).ThrowsAsync(new InvalidOperationException("DB Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeService.Update(updatedMeme));
            _memeRepositoryMoq.Verify(x => x.Update(It.IsAny<Meme>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }
        #endregion

        #region DELETE MEME TESTS
        [Fact]
        public async Task Delete_InvalidId_ThrowsArgumentException()
        {
            // arrange
            const int invalidId = 0;

            // act & assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await _memeService.Delete(invalidId));
            Assert.Equal("Invalid id.", exception.Message);

            // Verify
            _memeRepositoryMoq.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>()), Times.Never);
            _memeRepositoryMoq.Verify(x => x.Delete(It.IsAny<Meme>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_MemeExists_CallsDeleteAndSave()
        {
            // arrange
            int deleteMemeId = 1;
            var existingMeme = new Meme { MemeId = deleteMemeId, Title = "DeleteMe", ImageUrl = "http://delete.com" };
            _memeRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>())).ReturnsAsync(new List<Meme> { existingMeme });

            // act
            await _memeService.Delete(deleteMemeId);

            // assert
            _memeRepositoryMoq.Verify(x => x.Delete(existingMeme), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_MemeDoesntExist_ThrowsInvalidOperationException()
        {
            // arrange
            const int nonexistentMemeId = 999;
            _memeRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>())).ReturnsAsync(new List<Meme>());

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeService.Delete(nonexistentMemeId));
            _memeRepositoryMoq.Verify(x => x.Delete(It.IsAny<Meme>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_MultipleMemesWithSameId_ThrowsInvalidOperationException()
        {
            // arrange
            const int memeId = 1;
            var memes = new List<Meme>
            {
                new Meme { MemeId = memeId, Title = "Dup1", ImageUrl = "http://dup1.com" },
                new Meme { MemeId = memeId, Title = "Dup2", ImageUrl = "http://dup2.com" }
            };
            _memeRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>())).ReturnsAsync(memes);

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeService.Delete(memeId));
            _memeRepositoryMoq.Verify(x => x.Delete(It.IsAny<Meme>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_ThrowsException_WhenFindByConditionThrows()
        {
            // arrange
            const int memeId = 1;
            _memeRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>())).ThrowsAsync(new InvalidOperationException("DB Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeService.Delete(memeId));
            _memeRepositoryMoq.Verify(x => x.Delete(It.IsAny<Meme>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_ThrowsException_WhenDeleteThrows()
        {
            // arrange
            int deleteMemeId = 1;
            var existingMeme = new Meme { MemeId = deleteMemeId, Title = "DeleteMe", ImageUrl = "http://delete.com" };
            _memeRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>())).ReturnsAsync(new List<Meme> { existingMeme });
            _memeRepositoryMoq.Setup(x => x.Delete(existingMeme)).ThrowsAsync(new InvalidOperationException("Delete Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeService.Delete(deleteMemeId));
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_ThrowsException_WhenSaveThrows()
        {
            // arrange
            int deleteMemeId = 1;
            var existingMeme = new Meme { MemeId = deleteMemeId, Title = "DeleteMe", ImageUrl = "http://delete.com" };
            _memeRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Meme, bool>>>())).ReturnsAsync(new List<Meme> { existingMeme });
            _repositoryWrapperMoq.Setup(x => x.Save()).ThrowsAsync(new InvalidOperationException("Save Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memeService.Delete(deleteMemeId));
        }
        #endregion

        public static IEnumerable<object[]> GetIncorrectMemes()
        {
            return new List<object[]>
            {
                new object[] { new Meme { MemeId = 0, Title = "", ImageUrl = "" } },
                new object[] { new Meme { MemeId = 0, Title = "Test", ImageUrl = "" } },
                new object[] { new Meme { MemeId = 0, Title = "Test", ImageUrl = "http://test.com" } },
                new object[] { new Meme { MemeId = 1, Title = "", ImageUrl = "http://test.com" } },
                new object[] { new Meme { MemeId = 1, Title = "", ImageUrl = "" } },
                new object[] { new Meme { MemeId = 1, Title = "Test", ImageUrl = "" } }
            };
        }

        public static IEnumerable<object[]> GetWhitespaceMemes()
        {
            return new List<object[]>
            {
                new object[] { new Meme { MemeId = 1, Title = " ", ImageUrl = "http://test.com" } },
                new object[] { new Meme { MemeId = 1, Title = "test", ImageUrl = " " } }
            };
        }

        public static IEnumerable<object[]> GetIncorrectMemesForUpdate()
        {
            return new List<object[]>
            {
                new object[] { new Meme { MemeId = 0, Title = "", ImageUrl = "" } },
                new object[] { new Meme { MemeId = 0, Title = "Test", ImageUrl = "" } },
                new object[] { new Meme { MemeId = 0, Title = "Test", ImageUrl = "http://test.com" } },
                new object[] { new Meme { MemeId = 1, Title = "", ImageUrl = "http://test.com" } },
                new object[] { new Meme { MemeId = 1, Title = "", ImageUrl = "" } },
                new object[] { new Meme { MemeId = 1, Title = "Test", ImageUrl = "" } }
            };
        }

        public static IEnumerable<object[]> GetWhitespaceMemesForUpdate()
        {
            return new List<object[]>
            {
                new object[] { new Meme { MemeId = 1, Title = " ", ImageUrl = "http://test.com" } },
                new object[] { new Meme { MemeId = 1, Title = "test", ImageUrl = " " } }
            };
        }
    }
}
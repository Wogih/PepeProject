using BusinessLogic.Services;
using Domain.Interfaces;
using Domain.Interfaces.IUser;
using Domain.Models;
using Moq;
using System.Linq.Expressions;

namespace BusinessLogic.Tests
{
    public class UserServiceTest
    {
        private readonly UserService _userService;
        private readonly Mock<IUserRepository> _userRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> _repositoryWrapperMoq;

        public UserServiceTest()
        {
            _repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            _userRepositoryMoq = new Mock<IUserRepository>();

            _repositoryWrapperMoq.Setup(x => x.User).Returns(_userRepositoryMoq.Object);
            _repositoryWrapperMoq.Setup(x => x.Save()).Returns(Task.CompletedTask);

            _userService = new UserService(_repositoryWrapperMoq.Object);
        }

        #region GET ALL USERS TESTS
        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoUsers()
        {
            // arrange
            var emptyList = new List<User>();
            _userRepositoryMoq.Setup(x => x.FindAll()).ReturnsAsync(emptyList);

            // act
            var result = await _userService.GetAll();

            // assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAll_ReturnsListOfUsers_WhenUsersExist()
        {
            // arrange
            var users = new List<User>
            {
                new User { UserId = 1, Username = "User1", Email = "email1@example.com" },
                new User { UserId = 2, Username = "User2", Email = "email2@example.com" }
            };

            _userRepositoryMoq.Setup(x => x.FindAll()).ReturnsAsync(users);

            // act
            var result = await _userService.GetAll();

            // assert
            Assert.Equal(users.Count, result.Count);
            foreach (var expected in users)
            {
                Assert.Contains(result, actual => actual.UserId == expected.UserId && actual.Username == expected.Username);
            }
        }

        [Fact]
        public async Task GetAll_ThrowsException_WhenRepositoryThrows()
        {
            // arrange
            _userRepositoryMoq.Setup(x => x.FindAll()).ThrowsAsync(new InvalidOperationException("DB Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userService.GetAll());
        }
        #endregion

        #region GET BY ID TESTS
        [Fact]
        public async Task GetById_UserExists_ReturnsCorrectUser()
        {
            // arrange
            var existingUser = new User { UserId = 1, Username = "ExistingUser", Email = "existing@example.com" };
            _userRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(new List<User> { existingUser });

            // act
            var result = await _userService.GetById(existingUser.UserId);

            // assert
            Assert.NotNull(result);
            Assert.Equal(existingUser.UserId, result.UserId);
            Assert.Equal(existingUser.Username, result.Username);
        }

        [Fact]
        public async Task GetById_UserDoesntExist_ThrowsInvalidOperationException()
        {
            // arrange
            const int nonexistentUserId = 999;
            _userRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(new List<User>());

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userService.GetById(nonexistentUserId));
        }

        [Fact]
        public async Task GetById_MultipleUsersWithSameId_ThrowsInvalidOperationException()
        {
            // arrange
            const int userId = 1;
            var users = new List<User>
            {
                new User { UserId = userId, Username = "Dup1", Email = "dup1@example.com" },
                new User { UserId = userId, Username = "Dup2", Email = "dup2@example.com" }
            };
            _userRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(users);

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userService.GetById(userId));
        }

        [Fact]
        public async Task GetById_ThrowsException_WhenRepositoryThrows()
        {
            // arrange
            const int userId = 1;
            _userRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).ThrowsAsync(new InvalidOperationException("DB Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userService.GetById(userId));
        }
        #endregion

        #region CREATE USER TESTS
        [Fact]
        public async Task Create_NullUser_ThrowsArgumentNullException()
        {
            // act
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _userService.Create(null));

            // assert
            Assert.IsType<ArgumentNullException>(ex);
            _userRepositoryMoq.Verify(x => x.Create(It.IsAny<User>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectUsers))]
        public async Task Create_IncorrectUser_ThrowsArgumentException(User user)
        {
            // arrange
            var newUser = user;

            // act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.Create(newUser));

            // assert
            _userRepositoryMoq.Verify(x => x.Create(It.IsAny<User>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Theory]
        [MemberData(nameof(GetWhitespaceUsers))]
        public async Task Create_WhitespaceUser_ThrowsArgumentException(User user)
        {
            // arrange
            var newUser = user;

            // act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.Create(newUser));

            // assert
            _userRepositoryMoq.Verify(x => x.Create(It.IsAny<User>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task Create_CorrectUser_CallsCreateAndSave()
        {
            // arrange
            var validUser = new User { Username = "ValidUser", Email = "valid@example.com", PasswordHash = "passwordhash" };

            // act
            await _userService.Create(validUser);

            // assert
            _userRepositoryMoq.Verify(x => x.Create(validUser), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Create_ThrowsException_WhenRepositoryCreateThrows()
        {
            // arrange
            var validUser = new User { Username = "ValidUser", Email = "valid@example.com", PasswordHash = "passwordhash" };
            _userRepositoryMoq.Setup(x => x.Create(validUser)).ThrowsAsync(new InvalidOperationException("DB Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userService.Create(validUser));
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_ThrowsException_WhenSaveThrows()
        {
            // arrange
            var validUser = new User { Username = "ValidUser", Email = "valid@example.com", PasswordHash = "passwordhash" };
            _repositoryWrapperMoq.Setup(x => x.Save()).ThrowsAsync(new InvalidOperationException("Save Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userService.Create(validUser));
        }
        #endregion

        #region UPDATE USER TESTS
        [Fact]
        public async Task Update_NullUser_ThrowsArgumentNullException()
        {
            // act & assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _userService.Update(null));
            _userRepositoryMoq.Verify(x => x.Update(It.IsAny<User>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Update_ValidUser_CallsUpdateAndSave()
        {
            // arrange
            var updatedUser = new User { UserId = 1, Username = "UpdatedUsername", Email = "updated@example.com" };

            // act
            await _userService.Update(updatedUser);

            // assert
            _userRepositoryMoq.Verify(x => x.Update(updatedUser), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_ThrowsException_WhenRepositoryUpdateThrows()
        {
            // arrange
            var updatedUser = new User { UserId = 1, Username = "UpdatedUsername", Email = "updated@example.com" };
            _userRepositoryMoq.Setup(x => x.Update(updatedUser)).ThrowsAsync(new InvalidOperationException("DB Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userService.Update(updatedUser));
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Update_ThrowsException_WhenSaveThrows()
        {
            // arrange
            var updatedUser = new User { UserId = 1, Username = "UpdatedUsername", Email = "updated@example.com" };
            _repositoryWrapperMoq.Setup(x => x.Save()).ThrowsAsync(new InvalidOperationException("Save Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userService.Update(updatedUser));
        }
        #endregion

        #region DELETE USER TESTS
        [Fact]
        public async Task Delete_UserExists_CallsDeleteAndSave()
        {
            // arrange
            int deleteUserId = 1;
            var existingUser = new User { UserId = deleteUserId, Username = "DeleteMe", Email = "delete@example.com" };
            _userRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(new List<User> { existingUser });

            // act
            await _userService.Delete(deleteUserId);

            // assert
            _userRepositoryMoq.Verify(x => x.Delete(existingUser), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_UserDoesntExist_ThrowsInvalidOperationException()
        {
            // arrange
            const int nonexistentUserId = 999;
            _userRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(new List<User>());

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userService.Delete(nonexistentUserId));
            _userRepositoryMoq.Verify(x => x.Delete(It.IsAny<User>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_MultipleUsersWithSameId_ThrowsInvalidOperationException()
        {
            // arrange
            const int userId = 1;
            var users = new List<User>
            {
                new User { UserId = userId, Username = "Dup1", Email = "dup1@example.com" },
                new User { UserId = userId, Username = "Dup2", Email = "dup2@example.com" }
            };
            _userRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(users);

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userService.Delete(userId));
            _userRepositoryMoq.Verify(x => x.Delete(It.IsAny<User>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_ThrowsException_WhenFindByConditionThrows()
        {
            // arrange
            const int userId = 1;
            _userRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).ThrowsAsync(new InvalidOperationException("DB Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userService.Delete(userId));
            _userRepositoryMoq.Verify(x => x.Delete(It.IsAny<User>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_ThrowsException_WhenDeleteThrows()
        {
            // arrange
            int deleteUserId = 1;
            var existingUser = new User { UserId = deleteUserId, Username = "DeleteMe", Email = "delete@example.com" };
            _userRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(new List<User> { existingUser });
            _userRepositoryMoq.Setup(x => x.Delete(existingUser)).ThrowsAsync(new InvalidOperationException("Delete Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userService.Delete(deleteUserId));
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_ThrowsException_WhenSaveThrows()
        {
            // arrange
            int deleteUserId = 1;
            var existingUser = new User { UserId = deleteUserId, Username = "DeleteMe", Email = "delete@example.com" };
            _userRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(new List<User> { existingUser });
            _repositoryWrapperMoq.Setup(x => x.Save()).ThrowsAsync(new InvalidOperationException("Save Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userService.Delete(deleteUserId));
        }
        #endregion

        public static IEnumerable<object[]> GetIncorrectUsers()
        {
            return new List<object[]>
            {
                new object[] { new User() { Username = "", Email = "", PasswordHash = "" } },
                new object[] { new User() { Username = "Test", Email = "", PasswordHash = "" } },
                new object[] { new User() { Username = "Test", Email = "test@test.com", PasswordHash = "" } },
                new object[] { new User() { Username = "", Email = "test@test.com", PasswordHash = "hash" } },
                new object[] { new User() { Username = "", Email = "", PasswordHash = "hash" } }
            };
        }

        public static IEnumerable<object[]> GetWhitespaceUsers()
        {
            return new List<object[]>
            {
                new object[] { new User() { Username = " ", Email = "test@test.com", PasswordHash = "hash" } },
                new object[] { new User() { Username = "test", Email = " ", PasswordHash = "hash" } },
                new object[] { new User() { Username = "test", Email = "test@test.com", PasswordHash = "   " } }
            };
        }
    }
}
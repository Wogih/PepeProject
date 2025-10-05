using BusinessLogic.Services;
using Domain.Interfaces;
using Domain.Interfaces.IUserRole;
using Domain.Models;
using Moq;
using System.Linq.Expressions;

namespace BusinessLogic.Tests
{
    public class UserRoleServiceTest
    {
        private readonly UserRoleService _userRoleService;
        private readonly Mock<IUserRoleRepository> _userRoleRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> _repositoryWrapperMoq;

        public UserRoleServiceTest()
        {
            _repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            _userRoleRepositoryMoq = new Mock<IUserRoleRepository>();

            _repositoryWrapperMoq.Setup(x => x.UserRole).Returns(_userRoleRepositoryMoq.Object);
            _repositoryWrapperMoq.Setup(x => x.Save()).Returns(Task.CompletedTask);

            _userRoleService = new UserRoleService(_repositoryWrapperMoq.Object);
        }

        #region GET ALL USER ROLES TESTS
        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoUserRoles()
        {
            // arrange
            var emptyList = new List<UserRole>();
            _userRoleRepositoryMoq.Setup(x => x.FindAll()).ReturnsAsync(emptyList);

            // act
            var result = await _userRoleService.GetAll();

            // assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAll_ReturnsListOfUserRoles_WhenUserRolesExist()
        {
            // arrange
            var userRoles = new List<UserRole>
            {
                new UserRole { UserRoleId = 1, UserId = 1, RoleId = 1 },
                new UserRole { UserRoleId = 2, UserId = 2, RoleId = 2 }
            };

            _userRoleRepositoryMoq.Setup(x => x.FindAll()).ReturnsAsync(userRoles);

            // act
            var result = await _userRoleService.GetAll();

            // assert
            Assert.Equal(userRoles.Count, result.Count);
            foreach (var expected in userRoles)
            {
                Assert.Contains(result, actual => 
                    actual.UserRoleId == expected.UserRoleId && 
                    actual.UserId == expected.UserId && 
                    actual.RoleId == expected.RoleId);
            }
        }

        [Fact]
        public async Task GetAll_ThrowsException_WhenRepositoryThrows()
        {
            // arrange
            _userRoleRepositoryMoq.Setup(x => x.FindAll()).ThrowsAsync(new InvalidOperationException("DB Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userRoleService.GetAll());
        }
        #endregion

        #region GET BY ID TESTS
        [Fact]
        public async Task GetById_UserRoleExists_ReturnsCorrectUserRole()
        {
            // arrange
            var existingUserRole = new UserRole { UserRoleId = 1, UserId = 1, RoleId = 1 };
            _userRoleRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UserRole, bool>>>()))
                .ReturnsAsync(new List<UserRole> { existingUserRole });

            // act
            var result = await _userRoleService.GetById(existingUserRole.UserRoleId);

            // assert
            Assert.NotNull(result);
            Assert.Equal(existingUserRole.UserRoleId, result.UserRoleId);
            Assert.Equal(existingUserRole.UserId, result.UserId);
            Assert.Equal(existingUserRole.RoleId, result.RoleId);
        }

        [Fact]
        public async Task GetById_UserRoleDoesntExist_ThrowsInvalidOperationException()
        {
            // arrange
            const int nonexistentUserRoleId = 999;
            _userRoleRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UserRole, bool>>>()))
                .ReturnsAsync(new List<UserRole>());

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userRoleService.GetById(nonexistentUserRoleId));
        }

        [Fact]
        public async Task GetById_MultipleUserRolesWithSameId_ThrowsInvalidOperationException()
        {
            // arrange
            const int userRoleId = 1;
            var userRoles = new List<UserRole>
            {
                new UserRole { UserRoleId = userRoleId, UserId = 1, RoleId = 1 },
                new UserRole { UserRoleId = userRoleId, UserId = 2, RoleId = 2 }
            };
            _userRoleRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UserRole, bool>>>()))
                .ReturnsAsync(userRoles);

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userRoleService.GetById(userRoleId));
        }

        [Fact]
        public async Task GetById_ThrowsException_WhenRepositoryThrows()
        {
            // arrange
            const int userRoleId = 1;
            _userRoleRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UserRole, bool>>>()))
                .ThrowsAsync(new InvalidOperationException("DB Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userRoleService.GetById(userRoleId));
        }
        #endregion

        #region CREATE USER ROLE TESTS
        [Fact]
        public async Task Create_NullUserRole_ThrowsArgumentNullException()
        {
            // act & assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _userRoleService.Create(null));
            _userRoleRepositoryMoq.Verify(x => x.Create(It.IsAny<UserRole>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectUserRoles))]
        public async Task Create_IncorrectUserRole_ThrowsArgumentException(UserRole userRole)
        {
            // act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _userRoleService.Create(userRole));

            // assert
            _userRoleRepositoryMoq.Verify(x => x.Create(It.IsAny<UserRole>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task Create_CorrectUserRole_CallsCreateAndSave()
        {
            // arrange
            var validUserRole = new UserRole { UserId = 1, RoleId = 1 };

            // act
            await _userRoleService.Create(validUserRole);

            // assert
            _userRoleRepositoryMoq.Verify(x => x.Create(validUserRole), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Create_ThrowsException_WhenRepositoryCreateThrows()
        {
            // arrange
            var validUserRole = new UserRole { UserId = 1, RoleId = 1 };
            _userRoleRepositoryMoq.Setup(x => x.Create(validUserRole)).ThrowsAsync(new InvalidOperationException("DB Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userRoleService.Create(validUserRole));
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_ThrowsException_WhenSaveThrows()
        {
            // arrange
            var validUserRole = new UserRole { UserId = 1, RoleId = 1 };
            _repositoryWrapperMoq.Setup(x => x.Save()).ThrowsAsync(new InvalidOperationException("Save Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userRoleService.Create(validUserRole));
        }
        #endregion

        #region UPDATE USER ROLE TESTS
        [Fact]
        public async Task Update_NullUserRole_ThrowsArgumentNullException()
        {
            // act & assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _userRoleService.Update(null));
            _userRoleRepositoryMoq.Verify(x => x.Update(It.IsAny<UserRole>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectUserRoles))]
        public async Task Update_IncorrectUserRole_ThrowsArgumentException(UserRole userRole)
        {
            // act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _userRoleService.Update(userRole));

            // assert
            _userRoleRepositoryMoq.Verify(x => x.Update(It.IsAny<UserRole>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task Update_ValidUserRole_CallsUpdateAndSave()
        {
            // arrange
            var updatedUserRole = new UserRole { UserRoleId = 1, UserId = 1, RoleId = 2 };

            // act
            await _userRoleService.Update(updatedUserRole);

            // assert
            _userRoleRepositoryMoq.Verify(x => x.Update(updatedUserRole), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_ThrowsException_WhenRepositoryUpdateThrows()
        {
            // arrange
            var updatedUserRole = new UserRole { UserRoleId = 1, UserId = 1, RoleId = 2 };
            _userRoleRepositoryMoq.Setup(x => x.Update(updatedUserRole)).ThrowsAsync(new InvalidOperationException("DB Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userRoleService.Update(updatedUserRole));
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Update_ThrowsException_WhenSaveThrows()
        {
            // arrange
            var updatedUserRole = new UserRole { UserRoleId = 1, UserId = 1, RoleId = 2 };
            _repositoryWrapperMoq.Setup(x => x.Save()).ThrowsAsync(new InvalidOperationException("Save Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userRoleService.Update(updatedUserRole));
        }
        #endregion

        #region DELETE USER ROLE TESTS
        [Fact]
        public async Task Delete_UserRoleExists_CallsDeleteAndSave()
        {
            // arrange
            int deleteUserRoleId = 1;
            var existingUserRole = new UserRole { UserRoleId = deleteUserRoleId, UserId = 1, RoleId = 1 };
            _userRoleRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UserRole, bool>>>()))
                .ReturnsAsync(new List<UserRole> { existingUserRole });

            // act
            await _userRoleService.Delete(deleteUserRoleId);

            // assert
            _userRoleRepositoryMoq.Verify(x => x.Delete(existingUserRole), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_UserRoleDoesntExist_ThrowsInvalidOperationException()
        {
            // arrange
            const int nonexistentUserRoleId = 999;
            _userRoleRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UserRole, bool>>>()))
                .ReturnsAsync(new List<UserRole>());

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userRoleService.Delete(nonexistentUserRoleId));
            _userRoleRepositoryMoq.Verify(x => x.Delete(It.IsAny<UserRole>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_MultipleUserRolesWithSameId_ThrowsInvalidOperationException()
        {
            // arrange
            const int userRoleId = 1;
            var userRoles = new List<UserRole>
            {
                new UserRole { UserRoleId = userRoleId, UserId = 1, RoleId = 1 },
                new UserRole { UserRoleId = userRoleId, UserId = 2, RoleId = 2 }
            };
            _userRoleRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UserRole, bool>>>()))
                .ReturnsAsync(userRoles);

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userRoleService.Delete(userRoleId));
            _userRoleRepositoryMoq.Verify(x => x.Delete(It.IsAny<UserRole>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_ThrowsException_WhenFindByConditionThrows()
        {
            // arrange
            const int userRoleId = 1;
            _userRoleRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UserRole, bool>>>()))
                .ThrowsAsync(new InvalidOperationException("DB Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userRoleService.Delete(userRoleId));
            _userRoleRepositoryMoq.Verify(x => x.Delete(It.IsAny<UserRole>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_ThrowsException_WhenDeleteThrows()
        {
            // arrange
            int deleteUserRoleId = 1;
            var existingUserRole = new UserRole { UserRoleId = deleteUserRoleId, UserId = 1, RoleId = 1 };
            _userRoleRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UserRole, bool>>>()))
                .ReturnsAsync(new List<UserRole> { existingUserRole });
            _userRoleRepositoryMoq.Setup(x => x.Delete(existingUserRole)).ThrowsAsync(new InvalidOperationException("Delete Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userRoleService.Delete(deleteUserRoleId));
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_ThrowsException_WhenSaveThrows()
        {
            // arrange
            int deleteUserRoleId = 1;
            var existingUserRole = new UserRole { UserRoleId = deleteUserRoleId, UserId = 1, RoleId = 1 };
            _userRoleRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UserRole, bool>>>()))
                .ReturnsAsync(new List<UserRole> { existingUserRole });
            _repositoryWrapperMoq.Setup(x => x.Save()).ThrowsAsync(new InvalidOperationException("Save Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userRoleService.Delete(deleteUserRoleId));
        }
        #endregion

        #region ADDITIONAL METHODS TESTS
        [Fact]
        public async Task GetByUserId_ValidUserId_ReturnsUserRoles()
        {
            // arrange
            const int userId = 1;
            var userRoles = new List<UserRole>
            {
                new UserRole { UserRoleId = 1, UserId = userId, RoleId = 1 },
                new UserRole { UserRoleId = 2, UserId = userId, RoleId = 2 }
            };
            _userRoleRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UserRole, bool>>>()))
                .ReturnsAsync(userRoles);

            // act
            var result = await _userRoleService.GetByUserId(userId);

            // assert
            Assert.Equal(userRoles.Count, result.Count);
            Assert.All(result, ur => Assert.Equal(userId, ur.UserId));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetByUserId_InvalidUserId_ThrowsArgumentException(int invalidUserId)
        {
            // act & assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userRoleService.GetByUserId(invalidUserId));
        }

        [Fact]
        public async Task GetByRoleId_ValidRoleId_ReturnsUserRoles()
        {
            // arrange
            const int roleId = 1;
            var userRoles = new List<UserRole>
            {
                new UserRole { UserRoleId = 1, UserId = 1, RoleId = roleId },
                new UserRole { UserRoleId = 2, UserId = 2, RoleId = roleId }
            };
            _userRoleRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UserRole, bool>>>()))
                .ReturnsAsync(userRoles);

            // act
            var result = await _userRoleService.GetByRoleId(roleId);

            // assert
            Assert.Equal(userRoles.Count, result.Count);
            Assert.All(result, ur => Assert.Equal(roleId, ur.RoleId));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetByRoleId_InvalidRoleId_ThrowsArgumentException(int invalidRoleId)
        {
            // act & assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userRoleService.GetByRoleId(invalidRoleId));
        }
        #endregion

        public static IEnumerable<object[]> GetIncorrectUserRoles()
        {
            return new List<object[]>
            {
                new object[] { new UserRole() { UserId = 0, RoleId = 1 } },
                new object[] { new UserRole() { UserId = -1, RoleId = 1 } },
                new object[] { new UserRole() { UserId = 1, RoleId = 0 } },
                new object[] { new UserRole() { UserId = 1, RoleId = -1 } },
                new object[] { new UserRole() { UserId = 0, RoleId = 0 } },
                new object[] { new UserRole() { UserId = -1, RoleId = -1 } }
            };
        }
    }
}
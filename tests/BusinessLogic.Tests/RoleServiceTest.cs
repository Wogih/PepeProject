using BusinessLogic.Services;
using Domain.Interfaces;
using Domain.Interfaces.IRole;
using Domain.Models;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace BusinessLogic.Tests
{
    public class RoleServiceTest
    {
        private readonly RoleService _roleService;
        private readonly Mock<IRoleRepository> _roleRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> _repositoryWrapperMoq;

        public RoleServiceTest()
        {
            _repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            _roleRepositoryMoq = new Mock<IRoleRepository>();

            _repositoryWrapperMoq.Setup(x => x.Role).Returns(_roleRepositoryMoq.Object);
            _repositoryWrapperMoq.Setup(x => x.Save()).Returns(Task.CompletedTask);

            _roleService = new RoleService(_repositoryWrapperMoq.Object);
        }

        #region GET ALL ROLES TESTS
        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoRoles()
        {
            // arrange
            var emptyList = new List<Role>();
            _roleRepositoryMoq.Setup(x => x.FindAll()).ReturnsAsync(emptyList);

            // act
            var result = await _roleService.GetAll();

            // assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAll_ReturnsListOfRoles_WhenRolesExist()
        {
            // arrange
            var roles = new List<Role>
            {
                new Role { RoleId = 1, RoleName = "Role1" }, new Role { RoleId = 2, RoleName = "Role2" }
            };

            _roleRepositoryMoq.Setup(x => x.FindAll()).ReturnsAsync(roles);

            // act
            var result = await _roleService.GetAll();

            // assert
            Assert.Equal(roles.Count, result.Count);
            foreach (var expected in roles)
            {
                Assert.Contains(result, actual => actual.RoleId == expected.RoleId && actual.RoleName == expected.RoleName);
            }
        }

        [Fact]
        public async Task GetAll_ThrowsException_WhenRepositoryThrows()
        {
            // arrange
            _roleRepositoryMoq.Setup(x => x.FindAll()).ThrowsAsync(new InvalidOperationException("DB Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _roleService.GetAll());
        }
        #endregion

        #region GET BY ID TESTS
        [Fact]
        public async Task GetById_RoleExists_ReturnsCorrectRole()
        {
            // arrange
            var existingRole = new Role { RoleId = 1, RoleName = "ExistingRole" };
            _roleRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Role, bool>>>())).ReturnsAsync(new List<Role> { existingRole });

            // act
            var result = await _roleService.GetById(existingRole.RoleId);

            // assert
            Assert.NotNull(result);
            Assert.Equal(existingRole.RoleId, result.RoleId);
            Assert.Equal(existingRole.RoleName, result.RoleName);
        }

        [Fact]
        public async Task GetById_RoleDoesntExist_ThrowsInvalidOperationException()
        {
            // arrange
            const int nonexistentRoleId = 999;
            _roleRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Role, bool>>>())).ReturnsAsync(new List<Role>());

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _roleService.GetById(nonexistentRoleId));
        }

        [Fact]
        public async Task GetById_MultipleRolesWithSameId_ThrowsInvalidOperationException()
        {
            // arrange
            const int roleId = 1;
            var roles = new List<Role>
            {
                new Role { RoleId = roleId, RoleName = "Dup1" },
                new Role { RoleId = roleId, RoleName = "Dup2" }
            };
            _roleRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Role, bool>>>())).ReturnsAsync(roles);

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _roleService.GetById(roleId));
        }

        [Fact]
        public async Task GetById_ThrowsException_WhenRepositoryThrows()
        {
            // arrange
            const int roleId = 1;
            _roleRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Role, bool>>>())).ThrowsAsync(new InvalidOperationException("DB Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _roleService.GetById(roleId));
        }
        #endregion

        #region CREATE ROLE TESTS
        [Fact]
        public async Task Create_NullRole_ThrowsArgumentNullException()
        {
            // act
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _roleService.Create(null));

            // assert
            Assert.IsType<ArgumentNullException>(ex);
            _roleRepositoryMoq.Verify(x => x.Create(It.IsAny<Role>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectRoles))]
        public async Task Create_IncorrectRole_ThrowsArgumentException(Role role)
        {
            // arrange
            var newRole = role;

            // act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _roleService.Create(newRole));

            // assert
            _roleRepositoryMoq.Verify(x => x.Create(It.IsAny<Role>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Theory]
        [MemberData(nameof(GetWhitespaceRoles))]
        public async Task Create_WhitespaceRole_ThrowsArgumentException(Role role)
        {
            // arrange
            var newRole = role;

            // act
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _roleService.Create(newRole));

            // assert
            _roleRepositoryMoq.Verify(x => x.Create(It.IsAny<Role>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task Create_ValidRole_CallsCreateAndSave()
        {
            // arrange
            var validRole = new Role { RoleName = "ValidRole" };

            // act
            await _roleService.Create(validRole);

            // assert
            _roleRepositoryMoq.Verify(x => x.Create(validRole), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Create_ThrowsException_WhenRepositoryCreateThrows()
        {
            // arrange
            var validRole = new Role { RoleName = "ValidRole" };
            _roleRepositoryMoq.Setup(x => x.Create(validRole)).ThrowsAsync(new InvalidOperationException("DB Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _roleService.Create(validRole));
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_ThrowsException_WhenSaveThrows()
        {
            // arrange
            var validRole = new Role { RoleName = "ValidRole" };
            _repositoryWrapperMoq.Setup(x => x.Save()).ThrowsAsync(new InvalidOperationException("Save Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _roleService.Create(validRole));
        }
        #endregion

        #region UPDATE ROLE TESTS
        [Fact]
        public async Task Update_NullRole_ThrowsArgumentNullException()
        {
            // act & assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _roleService.Update(null));
            _roleRepositoryMoq.Verify(x => x.Update(It.IsAny<Role>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Update_ValidRole_CallsUpdateAndSave()
        {
            // arrange
            var updatedRole = new Role { RoleId = 1, RoleName = "UpdatedRole" };

            // act
            await _roleService.Update(updatedRole);

            // assert
            _roleRepositoryMoq.Verify(x => x.Update(updatedRole), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_ThrowsException_WhenRepositoryUpdateThrows()
        {
            // arrange
            var updatedRole = new Role { RoleId = 1, RoleName = "UpdatedRole" };
            _roleRepositoryMoq.Setup(x => x.Update(updatedRole)).ThrowsAsync(new InvalidOperationException("DB Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _roleService.Update(updatedRole));
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Update_ThrowsException_WhenSaveThrows()
        {
            // arrange
            var updatedRole = new Role { RoleId = 1, RoleName = "UpdatedRole" };
            _repositoryWrapperMoq.Setup(x => x.Save()).ThrowsAsync(new InvalidOperationException("Save Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _roleService.Update(updatedRole));
        }
        #endregion

        #region DELETE ROLE TESTS
        [Fact]
        public async Task Delete_RoleExists_CallsDeleteAndSave()
        {
            // arrange
            int deleteRoleId = 1;
            var existingRole = new Role { RoleId = deleteRoleId, RoleName = "DeleteMe" };
            _roleRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Role, bool>>>())).ReturnsAsync(new List<Role> { existingRole });

            // act
            await _roleService.Delete(deleteRoleId);

            // assert
            _roleRepositoryMoq.Verify(x => x.Delete(existingRole), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_RoleDoesntExist_ThrowsInvalidOperationException()
        {
            // arrange
            const int nonexistentRoleId = 999;
            _roleRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Role, bool>>>())).ReturnsAsync(new List<Role>());

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _roleService.Delete(nonexistentRoleId));
            _roleRepositoryMoq.Verify(x => x.Delete(It.IsAny<Role>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_MultipleRolesWithSameId_ThrowsInvalidOperationException()
        {
            // arrange
            const int roleId = 1;
            var roles = new List<Role>
            {
                new Role { RoleId = roleId, RoleName = "Dup1" },
                new Role { RoleId = roleId, RoleName = "Dup2" }
            };
            _roleRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Role, bool>>>())).ReturnsAsync(roles);

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _roleService.Delete(roleId));
            _roleRepositoryMoq.Verify(x => x.Delete(It.IsAny<Role>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_ThrowsException_WhenFindByConditionThrows()
        {
            // arrange
            const int roleId = 1;
            _roleRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Role, bool>>>())).ThrowsAsync(new InvalidOperationException("DB Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _roleService.Delete(roleId));
            _roleRepositoryMoq.Verify(x => x.Delete(It.IsAny<Role>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_ThrowsException_WhenDeleteThrows()
        {
            // arrange
            int deleteRoleId = 1;
            var existingRole = new Role { RoleId = deleteRoleId, RoleName = "DeleteMe" };
            _roleRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Role, bool>>>())).ReturnsAsync(new List<Role> { existingRole });
            _roleRepositoryMoq.Setup(x => x.Delete(existingRole)).ThrowsAsync(new InvalidOperationException("Delete Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _roleService.Delete(deleteRoleId));
            _repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_ThrowsException_WhenSaveThrows()
        {
            // arrange
            int deleteRoleId = 1;
            var existingRole = new Role { RoleId = deleteRoleId, RoleName = "DeleteMe" };
            _roleRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Role, bool>>>())).ReturnsAsync(new List<Role> { existingRole });
            _repositoryWrapperMoq.Setup(x => x.Save()).ThrowsAsync(new InvalidOperationException("Save Error"));

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _roleService.Delete(deleteRoleId));
        }
        #endregion

        public static IEnumerable<object[]> GetIncorrectRoles()
        {
            return new List<object[]>
            {
                new object[] { new Role { RoleName = "" } }
            };
        }

        public static IEnumerable<object[]> GetWhitespaceRoles()
        {
            return new List<object[]>
            {
                new object[] { new Role { RoleName = " " } }
            };
        }
    }
}
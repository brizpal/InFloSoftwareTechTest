using FluentAssertions;
using Moq;
using UserManagement.Core.DTOs;
using UserManagement.Core.Interfaces;
using UserManagement.Services;

namespace Tests.UserManagement.Api.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _mockRepo = null!;
        private UserService _service = null!;

        private List<User> sampleUsers = new()
        {
            new User { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", IsActive = true },
            new User { Id = 2, Forename = "Benjamin", Surname = "Gates", Email = "bfgates@example.com", IsActive = true }
        };

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IUserRepository>();
            _service = new UserService(_mockRepo.Object);
        }

        [TestMethod]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(sampleUsers);

            var result = await _service.GetAllUsersAsync();

            result.Should().HaveCount(2);
            result.Select(u => u.Email).Should().Contain("ploew@example.com");
        }

        [TestMethod]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenExists()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(sampleUsers[0]);

            var user = await _service.GetUserByIdAsync(1);

            user.Should().NotBeNull();
            user!.Forename.Should().Be("Peter");
        }

        [TestMethod]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(100)).ReturnsAsync((User?)null);

            var user = await _service.GetUserByIdAsync(100);

            user.Should().BeNull();
        }

        [TestMethod]
        public async Task CreateUserAsync_ShouldFail_WhenEmailExists()
        {
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(sampleUsers);

            var dto = new UserDto { Id = 3, Forename = "Test", Surname = "User", Email = "ploew@example.com" };

            var (success, message, user) = await _service.CreateUserAsync(dto);

            success.Should().BeFalse();
            message.Should().Be("A user with this email already exists.");
            user.Should().BeNull();
        }

        [TestMethod]
        public async Task CreateUserAsync_ShouldSucceed_WhenEmailIsUnique()
        {
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(sampleUsers);
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var dto = new UserDto { Id = 3, Forename = "Test", Surname = "User", Email = "test@example.com" };

            var (success, message, user) = await _service.CreateUserAsync(dto);

            success.Should().BeTrue();
            message.Should().Be("User created successfully.");
            user.Should().NotBeNull();
            user!.Email.Should().Be("test@example.com");
        }

        [TestMethod]
        public async Task UpdateUserAsync_ShouldSucceed_WhenIdMatches()
        {
            var dto = new UserDto { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com" };
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var (success, message) = await _service.UpdateUserAsync(1, dto);

            success.Should().BeTrue();
            message.Should().Be("User updated successfully.");
        }

        [TestMethod]
        public async Task UpdateUserAsync_ShouldFail_WhenIdMismatch()
        {
            var dto = new UserDto { Id = 2, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com" };

            var (success, message) = await _service.UpdateUserAsync(1, dto);

            success.Should().BeFalse();
            message.Should().Be("User ID mismatch.");
        }

        [TestMethod]
        public async Task DeleteUserAsync_ShouldSucceed_WhenUserExists()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(sampleUsers[0]);
            _mockRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            var (success, message) = await _service.DeleteUserAsync(1);

            success.Should().BeTrue();
            message.Should().Be("User deleted successfully.");
        }

        [TestMethod]
        public async Task DeleteUserAsync_ShouldFail_WhenUserNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(100)).ReturnsAsync((User?)null);

            var (success, message) = await _service.DeleteUserAsync(100);

            success.Should().BeFalse();
            message.Should().Be("User not found.");
        }
    }
}

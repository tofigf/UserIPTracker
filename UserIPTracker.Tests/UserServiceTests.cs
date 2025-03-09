using Moq;
using Microsoft.Extensions.Caching.Memory;
using Application.Services;
using Domain.Interfaces;

namespace UserIPTracker.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly UserService _userService;
        private readonly IMemoryCache _cache;

        public UserServiceTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _cache = new MemoryCache(new MemoryCacheOptions());
            _userService = new UserService(_mockRepo.Object, _cache);
        }

        [Fact]
        public async Task AddConnectionAsync_ShouldCallRepositoryOnce()
        {
            // Arrange
            long userId = 1;
            string ipAddress = "192.168.1.1";

            // Act
            await _userService.AddConnectionAsync(userId, ipAddress);

            // Assert
            _mockRepo.Verify(repo => repo.AddConnectionAsync(userId, ipAddress), Times.Once);
        }

        [Fact]
        public async Task SearchUsersByIpAsync_ShouldReturnCorrectUsers()
        {
            // Arrange
            string ipPart = "31.214";
            var expectedUsers = new List<long> { 1, 2 };

            _mockRepo.Setup(repo => repo.SearchUsersByIpAsync(ipPart))
                     .ReturnsAsync(expectedUsers);

            // Act
            var result = await _userService.SearchUsersByIpAsync(ipPart);

            // Assert
            Assert.Equal(expectedUsers, result);
        }
    }

}

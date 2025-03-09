using Moq;
using Infrastructure.Cache.UserIPTracker.Infrastructure.Cache;
using Infrastructure.Kafka.UserIPTracker.Infrastructure.Kafka;
using Domain.Interfaces;
using Application.Services;
using Infrastructure;

namespace UserIPTracker.Tests
{
        public class UserServiceTests
        {
            private readonly Mock<IUserRepository> _mockRepo;
            private readonly Mock<KafkaProducer> _mockKafkaProducer;
            private readonly Mock<RedisCacheService> _mockCache;
            private readonly UserService _userService;

            public UserServiceTests()
            {
                _mockRepo = new Mock<IUserRepository>();
                _mockKafkaProducer = new Mock<KafkaProducer>();
                _mockCache = new Mock<RedisCacheService>();

                _userService = new UserService(_mockRepo.Object, _mockKafkaProducer.Object, _mockCache.Object);
            }

            [Fact]
            public async Task AddConnectionAsync_ShouldPublishToKafkaAndClearCache()
            {
                var request = new ConnectUserRequest { UserId = 1, IpAddress = "192.168.1.1" };

                await _userService.AddConnectionAsync(request);

                _mockKafkaProducer.Verify(kafka => kafka.PublishUserConnectionAsync(request.UserId, request.IpAddress), Times.Once);
                _mockCache.Verify(cache => cache.RemoveCacheAsync($"UserIps_{request.UserId}"), Times.Once);
            }

            [Fact]
            public async Task SearchUsersByIpAsync_ShouldReturnCorrectUsers_FromCache()
            {
                string ipPart = "31.214";
                var expectedUsers = new List<long> { 1, 2 };

                _mockCache.Setup(cache => cache.GetCacheAsync($"search:{ipPart}"))
                          .ReturnsAsync(Newtonsoft.Json.JsonConvert.SerializeObject(expectedUsers));

                var result = await _userService.SearchUsersByIpAsync(ipPart);

                Assert.Equal(expectedUsers, result);
                _mockRepo.Verify(repo => repo.SearchUsersByIpAsync(ipPart), Times.Never);
            }

            [Fact]
            public async Task SearchUsersByIpAsync_ShouldReturnCorrectUsers_FromDatabaseIfCacheMiss()
            {
                string ipPart = "31.214";
                var expectedUsers = new List<long> { 1, 2 };

                _mockCache.Setup(cache => cache.GetCacheAsync($"search:{ipPart}"))
                          .ReturnsAsync((string)null); 

                _mockRepo.Setup(repo => repo.SearchUsersByIpAsync(ipPart))
                         .ReturnsAsync(expectedUsers);

                var result = await _userService.SearchUsersByIpAsync(ipPart);

                Assert.Equal(expectedUsers, result);
                _mockRepo.Verify(repo => repo.SearchUsersByIpAsync(ipPart), Times.Once);
                _mockCache.Verify(cache => cache.SetCacheAsync($"search:{ipPart}", Newtonsoft.Json.JsonConvert.SerializeObject(expectedUsers), It.IsAny<System.TimeSpan>()), Times.Once);
            }
        }
    }

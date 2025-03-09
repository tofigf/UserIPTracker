using Application.Services.Interfaces;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Cache.UserIPTracker.Infrastructure.Cache;
using Infrastructure.DTOs.Response;
using Infrastructure.Kafka.UserIPTracker.Infrastructure.Kafka;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
        public class UserService : IUserService
        {
            private readonly IUserRepository _userRepository;
            private readonly KafkaProducer _kafkaProducer;
            private readonly RedisCacheService _cache;

        public UserService(IUserRepository userRepository, KafkaProducer kafkaProducer, RedisCacheService cache)
            {
                _userRepository = userRepository;
                _kafkaProducer = kafkaProducer;
                _cache = cache;
        }

            public async Task AddConnectionAsync(ConnectUserRequest request)
            {
                await _kafkaProducer.PublishUserConnectionAsync(request.UserId, request.IpAddress);

                await _cache.RemoveCacheAsync($"UserIps_{request.UserId}");
            }

        public async Task<List<long>> SearchUsersByIpAsync(string ipPart)
        {
            var cacheKey = $"search:{ipPart}";
            var cachedUsers = await _cache.GetCacheAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedUsers))
            {
                var users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<long>>(cachedUsers);
                return users ?? new List<long>();
            }

            var usersFromRepo = await _userRepository.SearchUsersByIpAsync(ipPart);

            await _cache.SetCacheAsync(cacheKey, Newtonsoft.Json.JsonConvert.SerializeObject(usersFromRepo), TimeSpan.FromMinutes(10));

            return usersFromRepo;
        }

            public async Task<List<string>> GetUserIpAddressesAsync(long userId)
            {
        
                var cacheKey = $"UserIps_{userId}";
                var cachedIps = await _cache.GetCacheAsync(cacheKey);


            if (!string.IsNullOrEmpty(cachedIps))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(cachedIps) ?? new List<string>();
            }

                var ips = await _userRepository.GetUserIpAddressesAsync(userId);

                await _cache.SetCacheAsync(cacheKey, Newtonsoft.Json.JsonConvert.SerializeObject(ips), TimeSpan.FromMinutes(10));

                return ips;
            }

            public async Task<UserConnectionResponse?> GetLastConnectionAsync(long userId)
            {
                var cacheKey = $"last_connection:{userId}";
                var cachedLastConnection = await _cache.GetCacheAsync(cacheKey);

                if (!string.IsNullOrEmpty(cachedLastConnection))
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<UserConnectionResponse>(cachedLastConnection);
                }

                var lastConnection = await _userRepository.GetLastConnectionAsync(userId);

                if (lastConnection == null) return null;

                var response = new UserConnectionResponse
                {
                    LastTime = lastConnection.Value.lastTime,
                    IpAddress = lastConnection.Value.ipAddress
                };

                await _cache.SetCacheAsync(cacheKey, Newtonsoft.Json.JsonConvert.SerializeObject(response), TimeSpan.FromMinutes(10));

                return response;
            }
        }
    }

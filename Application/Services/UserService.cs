using Application.Services.Interfaces;
using Domain.Interfaces;
using Infrastructure.DTOs.Response;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _cache;

        public UserService(IUserRepository userRepository, IMemoryCache cache)
        {
            _userRepository = userRepository;
            _cache = cache;
        }

        public async Task AddConnectionAsync(long userId, string ipAddress)
        {
            await _userRepository.AddConnectionAsync(userId, ipAddress);
            _cache.Remove($"UserIps_{userId}");
        }

        public async Task<List<long>> SearchUsersByIpAsync(string ipPart)
        {
            return await _userRepository.SearchUsersByIpAsync(ipPart);
        }

        public async Task<List<string>> GetUserIpAddressesAsync(long userId)
        {
            return await _cache.GetOrCreateAsync($"UserIps_{userId}", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                var result = await _userRepository.GetUserIpAddressesAsync(userId);
                return result ?? new List<string>();
            }) ?? new List<string>();
        }

        public async Task<UserConnectionResponse?> GetLastConnectionAsync(long userId)
        {
            var lastConnection = await _userRepository.GetLastConnectionAsync(userId);

            if (lastConnection == null) return null;

            return new UserConnectionResponse
            {
                LastTime = lastConnection.Value.lastTime,
                IpAddress = lastConnection.Value.ipAddress
            };
        }
    }
}

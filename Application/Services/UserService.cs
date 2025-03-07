using Application.Services.Interfaces;
using Domain.Interfaces;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task AddConnectionAsync(long userId, string ipAddress)
        {
            await _userRepository.AddConnectionAsync(userId, ipAddress);
        }
    }
}

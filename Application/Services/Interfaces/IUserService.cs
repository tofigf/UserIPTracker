using Infrastructure;
using Infrastructure.DTOs.Response;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        Task AddConnectionAsync(ConnectUserRequest request);
        Task<List<long>> SearchUsersByIpAsync(string ipPart);
        Task<List<string>> GetUserIpAddressesAsync(long userId);
        Task<UserConnectionResponse?> GetLastConnectionAsync(long userId);
    }
}

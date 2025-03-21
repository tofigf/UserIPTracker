﻿namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserConnectionAsync(long userId, string ipAddress);
        Task<List<long>> SearchUsersByIpAsync(string ipPart);
        Task<List<string>> GetUserIpAddressesAsync(long userId);
        Task<(DateTime lastTime, string ipAddress)?> GetLastConnectionAsync(long userId);
    }
}

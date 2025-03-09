using Domain.Interfaces;
using Infrastructure.Persistence;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddUserConnectionAsync(long userId, string ipAddress)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.UserConnections.Add(new UserConnection
                {
                    UserId = userId,
                    IpAddress = ipAddress,
                    ConnectedAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
            }
        }


        public async Task<List<long>> SearchUsersByIpAsync(string ipPart)
        {
            return await _context.UserConnections
                .Where(uc => EF.Functions.Like(uc.IpAddress, ipPart + "%"))
                .Select(uc => uc.UserId)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<string>> GetUserIpAddressesAsync(long userId)
        {
            return await _context.UserConnections
                .Where(uc => uc.UserId == userId)
                .Select(uc => uc.IpAddress)
                .Distinct()
                .ToListAsync();
        }

        public async Task<(DateTime lastTime, string ipAddress)?> GetLastConnectionAsync(long userId)
        {
            var lastConnection = await _context.UserConnections
                .AsNoTracking() 
                .Where(uc => uc.UserId == userId)
                .OrderByDescending(uc => uc.ConnectedAt)
                .FirstOrDefaultAsync();

            return lastConnection == null ? null : (lastConnection.ConnectedAt, lastConnection.IpAddress);
        }

    }
}

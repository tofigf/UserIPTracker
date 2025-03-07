using Domain.Interfaces;
using Infrastructure.Persistence;
using Domain.Models;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddConnectionAsync(long userId, string ipAddress)
        {
            _context.UserConnections.Add(new UserConnection
            {
                UserId = userId,
                IpAddress = ipAddress
            });

            await _context.SaveChangesAsync();
        }
    }
}

using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserConnection> UserConnections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define primary keys
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<UserConnection>().HasKey(uc => uc.Id);

            // Define indexes for efficient querying
            modelBuilder.Entity<UserConnection>().HasIndex(uc => uc.IpAddress);
            modelBuilder.Entity<UserConnection>().HasIndex(uc => uc.UserId);
        }
    }

}

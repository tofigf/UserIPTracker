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
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<UserConnection>().HasKey(uc => uc.Id);

            modelBuilder.Entity<UserConnection>()
                .HasIndex(uc => uc.UserId)
                .HasDatabaseName("idx_userconnections_userid");
        
            modelBuilder.Entity<UserConnection>()
                .HasIndex(uc => uc.IpAddress)
                .HasDatabaseName("idx_userconnections_ip")
                .HasMethod("gist"); 

            modelBuilder.Entity<UserConnection>()
                .Property(uc => uc.IpAddress)
                .HasColumnType("inet"); 
        }
    }
}

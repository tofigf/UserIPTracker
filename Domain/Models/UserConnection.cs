using System.Net;

namespace Domain.Models
{
    public class UserConnection
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public required IPAddress IpAddress { get; set; }
        public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;
    }
}

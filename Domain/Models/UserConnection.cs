namespace Domain.Models
{
    public class UserConnection
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string IpAddress { get; set; } = null!;
        public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;
    }
}

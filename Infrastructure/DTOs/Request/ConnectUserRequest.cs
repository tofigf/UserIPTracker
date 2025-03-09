namespace Infrastructure
{
    public class ConnectUserRequest
    {
        public long UserId { get; set; }
        public string IpAddress { get; set; } = null!;
    }
}

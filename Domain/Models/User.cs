namespace Domain.Models
{
    public class User
    {
        public long Id { get; set; }
        public List<UserConnection> Connections { get; set; } = new();
    }

}

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task AddConnectionAsync(long userId, string ipAddress);
    }
}

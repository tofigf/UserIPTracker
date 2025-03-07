namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        Task AddConnectionAsync(long userId, string ipAddress);
    }
}

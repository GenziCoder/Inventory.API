using Inventory.API.Entities;

namespace Inventory.API.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByEmailAsync(string email);

        Task AddUserAsync(User user);

        Task SaveChangesAsync();

        Task<RefreshToken?> GetRefreshTokenAsync(string token);

        Task AddRefreshTokenAsync(RefreshToken token);
    }
}
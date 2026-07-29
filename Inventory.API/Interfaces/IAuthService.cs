using Inventory.API.DTOs.Auth;

namespace Inventory.API.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterDto dto);

        Task<LoginResponseDto?> LoginAsync(LoginDto dto);

        Task<RefreshTokenResponseDto?> RefreshTokenAsync(string refreshToken);

        Task LogoutAsync(string refreshToken);
    }
}
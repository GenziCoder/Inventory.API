using Inventory.API.DTOs.Auth;
using Inventory.API.Helpers;

namespace Inventory.API.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<string>> RegisterAsync(RegisterDto dto);

        Task<LoginResponseDto?> LoginAsync(LoginDto dto);

        Task<RefreshTokenResponseDto?> RefreshTokenAsync(string refreshToken);

        Task LogoutAsync(string refreshToken);
    }
}
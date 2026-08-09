using Inventory.API.DTOs.Auth;
using Inventory.API.Entities;
using Inventory.API.Helpers;
using Inventory.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Security.Cryptography;

namespace Inventory.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repository;
        private readonly ITokenService _tokenService;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthService(
            IAuthRepository repository,
            ITokenService tokenService)
        {
            _repository = repository;
            _tokenService = tokenService;
            _passwordHasher = new PasswordHasher<User>();
        }

        // Add this method here
        private static string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randomBytes);
        }
        public async Task<ApiResponse<string>> RegisterAsync(RegisterDto dto)
        {
            var existingEmail = await _repository.GetUserByEmailAsync(dto.Email);

            if (existingEmail != null)
                return ApiResponse<string>.FailureResponse("Email already exists.");
            // Check Username
            var existingUsername = await _repository.GetUserByUsernameAsync(dto.Username);

            if (existingUsername != null)
                return ApiResponse<string>.FailureResponse("Username already exists.");

            var user = new User
            {
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                Username = dto.Username,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Role = dto.Role,
                IsActive = true,

            };

            user.PasswordHash =
                _passwordHasher.HashPassword(user, dto.Password);

            await _repository.AddUserAsync(user);
            await _repository.SaveChangesAsync();

            return ApiResponse<string>.SuccessResponse("Registration completed successfully.");
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _repository.GetUserByEmailAsync(dto.Email);

            if (user == null)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.Password);

            if (result == PasswordVerificationResult.Failed)
                return null;

            var accessToken = _tokenService.GenerateToken(user);

            var refreshToken = GenerateRefreshToken();

            await _repository.AddRefreshTokenAsync(new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            });

            await _repository.SaveChangesAsync();

            return new LoginResponseDto
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(60)
            };


        }

        public async Task<RefreshTokenResponseDto?> RefreshTokenAsync(string refreshToken)
        {
            var storedToken = await _repository.GetRefreshTokenAsync(refreshToken);

            if (storedToken == null ||
                storedToken.IsRevoked ||
                storedToken.ExpiryDate <= DateTime.UtcNow)
            {
                return null;
            }

            var newAccessToken = _tokenService.GenerateToken(storedToken.User);

            var newRefreshToken = GenerateRefreshToken();

            storedToken.IsRevoked = true;

            await _repository.AddRefreshTokenAsync(new RefreshToken
            {
                Token = newRefreshToken,
                UserId = storedToken.UserId,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            });

            await _repository.SaveChangesAsync();

            return new RefreshTokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(60)
            };
        }
        public async Task LogoutAsync(string refreshToken)
        {
            var token = await _repository.GetRefreshTokenAsync(refreshToken);

            if (token == null)
                return;
            if (token.IsRevoked)
                return;
            if (token.ExpiryDate <= DateTime.UtcNow)
                return;
            token.IsRevoked = true;

            await _repository.SaveChangesAsync();
        }
    }
}
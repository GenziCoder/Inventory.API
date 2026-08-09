using Inventory.API.DTOs.Auth;
using Inventory.API.Helpers;
using Inventory.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if (result==null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Email already exists.",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "User registered successfully.",
                Data = null
            });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        [EnableRateLimiting("LoginPolicy")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (result == null)
            {
                return Unauthorized(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid email or password.",
                    Data = null
                });
            }

            return Ok(new ApiResponse<LoginResponseDto>
            {
                Success = true,
                Message = "Login successful.",
                Data = result
            });
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh(RefreshTokenRequestDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto.RefreshToken);

            if (result == null)
            {
                return Unauthorized(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid or expired refresh token.",
                    Data = null
                });
            }

            return Ok(new ApiResponse<RefreshTokenResponseDto>
            {
                Success = true,
                Message = "Token refreshed successfully.",
                Data = result
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout(RefreshTokenRequestDto dto)
        {
            await _authService.LogoutAsync(dto.RefreshToken);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Logged out successfully.",
                Data = null
            });
        }
    }
} 
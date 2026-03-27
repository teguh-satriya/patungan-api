using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patungan.Services.Interfaces;
using Patungan.Shared.Requests.User;
using Patungan.Shared.Responses;
using Patungan.Shared.Responses.User;
using System.Security.Claims;

namespace Patungan.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        protected readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<UserResponse>>> Register([FromBody] RegistrationRequest request)
        {
            var response = await _authService.RegisterAsync(request);
            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            if (response.Success)
                return Ok(response);

            return Unauthorized(response);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<ApiResponse<RefreshTokenResponse>>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var response = await _authService.RefreshTokenAsync(request);
            if (response.Success)
                return Ok(response);

            return Unauthorized(response);
        }

        [Authorize]
        [HttpPost("revoke-token")]
        public async Task<ActionResult<ApiResponse<object>>> RevokeToken([FromBody] RefreshTokenRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(ApiResponse<object>.Fail("Invalid user"));
            }

            var response = await _authService.RevokeTokenAsync(request.RefreshToken, userId);
            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }
    }
}

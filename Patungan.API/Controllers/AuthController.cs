using Microsoft.AspNetCore.Mvc;
using Patungan.Services.Interfaces;
using Patungan.Shared.Requests.User;
using Patungan.Shared.Responses;
using Patungan.Shared.Responses.User;

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
    }
}

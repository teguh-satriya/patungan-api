using Patungan.Shared.Requests.User;
using Patungan.Shared.Responses;
using Patungan.Shared.Responses.User;

namespace Patungan.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<UserResponse>> RegisterAsync(RegistrationRequest request);
        Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);
        Task<ApiResponse<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);
        Task<ApiResponse<object>> RevokeTokenAsync(string refreshToken, int userId);
    }
}

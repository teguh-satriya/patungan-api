using Patungan.Shared.Requests.User;
using Patungan.Shared.Responses;
using Patungan.Shared.Responses.User;

namespace Patungan.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<UserResponse>> RegisterAsync(RegistrationRequest request);
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Patungan.DataAccess.Entities;
using Patungan.DataAccess.Interfaces;
using Patungan.Services.Interfaces;
using Patungan.Shared.Constants;
using Patungan.Shared.Requests.User;
using Patungan.Shared.Responses;
using Patungan.Shared.Responses.User;
using Patungan.Shared.Settings;

namespace Patungan.Services.Services
{
    public class AuthServices : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITransactionTypeRepository _transactionTypeRepository;
        private readonly ITransactionTypeTemplateRepository _transactionTypeTemplateRepository;
        private readonly IPasswordHasher<UserModel> _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly JwtSettings _jwtSettings;

        public AuthServices(
            IUserRepository userRepository, 
            IPasswordHasher<UserModel> passwordHasher,
            ITransactionTypeRepository transactionTypeRepository,
            ITransactionTypeTemplateRepository transactionTypeTemplateRepository,
            IJwtTokenService jwtTokenService,
            IRefreshTokenRepository refreshTokenRepository,
            IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _transactionTypeRepository = transactionTypeRepository;
            _transactionTypeTemplateRepository = transactionTypeTemplateRepository;
            _jwtTokenService = jwtTokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<ApiResponse<UserResponse>> RegisterAsync(RegistrationRequest request)
        {
            try
            {
                if (await _userRepository.EmailExistsAsync(request.Email))
                {
                    return ApiResponse<UserResponse>.Fail(AuthConstant.MESSAGE_ERROR_EMAIL_EXIST);
                }
                var user = new UserModel
                {
                    UserName = request.UserName,
                    Email = request.Email,
                };

                if (!string.IsNullOrEmpty(request.Password))
                {
                    user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
                }
                else if (!string.IsNullOrEmpty(request.GoogleId))
                {
                    user.GoogleId = request.GoogleId;
                }
                else
                {
                    return ApiResponse<UserResponse>.Fail(AuthConstant.MESSAGE_ERROR_PASSWORD_REQUIRED);
                }

                await _userRepository.AddUserAsync(user);

                IReadOnlyList<TransactionTypeTemplateModel> templates = await _transactionTypeTemplateRepository.GetDefaultTemplatesAsync();
                if (templates.Count > 0)
                {
                    List<TransactionTypeModel> copies = templates
                        .Select(template => new TransactionTypeModel { 
                            Name = template.Name,
                            Nature = template.Nature,
                            Description = template.Description,
                            UserId = user.Id
                        })
                        .ToList();
                    await _transactionTypeRepository.AddRangeAsync(copies);
                }

                UserResponse data = new UserResponse
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email
                };

                return ApiResponse<UserResponse>.Ok(AuthConstant.MESSAGE_SUCCESS, data);
            }
            catch (Exception ex) 
            {
                return ApiResponse<UserResponse>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(request.Email);
                if (user == null)
                {
                    return ApiResponse<LoginResponse>.Fail(AuthConstant.MESSAGE_ERROR_INVALID_CREDENTIALS);
                }

                if (string.IsNullOrEmpty(user.PasswordHash))
                {
                    return ApiResponse<LoginResponse>.Fail("User registered with Google. Please use Google Sign-In.");
                }

                var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
                if (verificationResult == PasswordVerificationResult.Failed)
                {
                    return ApiResponse<LoginResponse>.Fail(AuthConstant.MESSAGE_ERROR_INVALID_CREDENTIALS);
                }

                var token = _jwtTokenService.GenerateToken(user);
                var refreshToken = _jwtTokenService.GenerateRefreshToken();
                var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes);

                var refreshTokenModel = new RefreshTokenModel
                {
                    UserId = user.Id,
                    Token = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryInDays),
                    CreatedAt = DateTime.UtcNow
                };

                await _refreshTokenRepository.AddAsync(refreshTokenModel);

                var response = new LoginResponse
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = token,
                    RefreshToken = refreshToken,
                    ExpiresAt = expiresAt
                };

                return ApiResponse<LoginResponse>.Ok("Login successful", response);
            }
            catch (Exception ex)
            {
                return ApiResponse<LoginResponse>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            try
            {
                var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);

                if (refreshToken == null)
                {
                    return ApiResponse<RefreshTokenResponse>.Fail("Invalid refresh token");
                }

                if (!refreshToken.IsActive)
                {
                    return ApiResponse<RefreshTokenResponse>.Fail("Refresh token is expired or revoked");
                }

                var user = await _userRepository.GetByIdAsync(refreshToken.UserId);
                if (user == null)
                {
                    return ApiResponse<RefreshTokenResponse>.Fail("User not found");
                }

                var newAccessToken = _jwtTokenService.GenerateToken(user);
                var newRefreshToken = _jwtTokenService.GenerateRefreshToken();
                var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes);

                refreshToken.RevokedAt = DateTime.UtcNow;
                refreshToken.ReplacedByToken = newRefreshToken;
                await _refreshTokenRepository.UpdateAsync(refreshToken);

                var newRefreshTokenModel = new RefreshTokenModel
                {
                    UserId = user.Id,
                    Token = newRefreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryInDays),
                    CreatedAt = DateTime.UtcNow
                };

                await _refreshTokenRepository.AddAsync(newRefreshTokenModel);

                var response = new RefreshTokenResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    ExpiresAt = expiresAt
                };

                return ApiResponse<RefreshTokenResponse>.Ok("Token refreshed successfully", response);
            }
            catch (Exception ex)
            {
                return ApiResponse<RefreshTokenResponse>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<object>> RevokeTokenAsync(string refreshToken, int userId)
        {
            try
            {
                var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

                if (token == null)
                {
                    return ApiResponse<object>.Fail("Invalid refresh token");
                }

                if (token.UserId != userId)
                {
                    return ApiResponse<object>.Fail("Unauthorized");
                }

                if (!token.IsActive)
                {
                    return ApiResponse<object>.Fail("Token already revoked or expired");
                }

                token.RevokedAt = DateTime.UtcNow;
                await _refreshTokenRepository.UpdateAsync(token);

                return ApiResponse<object>.Ok("Token revoked successfully", null);
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.Fail(ex.Message);
            }
        }
    }
}

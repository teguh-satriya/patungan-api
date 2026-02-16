using Microsoft.AspNetCore.Identity;
using Patungan.DataAccess.Entities;
using Patungan.DataAccess.Interfaces;
using Patungan.Services.Interfaces;
using Patungan.Shared.Constants;
using Patungan.Shared.Requests.User;
using Patungan.Shared.Responses;
using Patungan.Shared.Responses.User;

namespace Patungan.Services.Services
{
    public class AuthServices : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITransactionTypeRepository _transactionTypeRepository;
        private readonly ITransactionTypeTemplateRepository _transactionTypeTemplateRepository;
        private readonly IPasswordHasher<UserModel> _passwordHasher;
        public AuthServices(
            IUserRepository userRepository, 
            IPasswordHasher<UserModel> passwordHasher,
            ITransactionTypeRepository transactionTypeRepository,
            ITransactionTypeTemplateRepository transactionTypeTemplateRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _transactionTypeRepository = transactionTypeRepository;
            _transactionTypeTemplateRepository = transactionTypeTemplateRepository;
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
    }
}

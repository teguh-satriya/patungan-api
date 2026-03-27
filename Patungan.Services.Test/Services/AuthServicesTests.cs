using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using Patungan.DataAccess.Entities;
using Patungan.DataAccess.Interfaces;
using Patungan.Services.Interfaces;
using Patungan.Services.Services;
using Patungan.Shared.Constants;
using Patungan.Shared.Requests.User;
using Patungan.Shared.Responses.User;
using Patungan.Shared.Settings;

namespace Patungan.Services.Test.Services
{
    public class AuthServiceTests
    {
        private Mock<IJwtTokenService> CreateMockJwtTokenService()
        {
            var mockJwtService = new Mock<IJwtTokenService>();
            mockJwtService.Setup(j => j.GenerateToken(It.IsAny<UserModel>()))
                .Returns("mock.jwt.token");
            mockJwtService.Setup(j => j.GenerateRefreshToken())
                .Returns("mock.refresh.token");
            return mockJwtService;
        }

        private Mock<IRefreshTokenRepository> CreateMockRefreshTokenRepository()
        {
            var mockRefreshTokenRepo = new Mock<IRefreshTokenRepository>();
            mockRefreshTokenRepo.Setup(r => r.AddAsync(It.IsAny<RefreshTokenModel>()))
                .Returns(Task.CompletedTask);
            return mockRefreshTokenRepo;
        }

        private IOptions<JwtSettings> CreateMockJwtSettings()
        {
            return Options.Create(new JwtSettings
            {
                ExpiryInMinutes = 60,
                RefreshTokenExpiryInDays = 7
            });
        }

        [Fact]
        public async Task RegisterAsync_Successful_WithPassword()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.EmailExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            mockRepo.Setup(r => r.AddUserAsync(It.IsAny<UserModel>())).Returns(Task.CompletedTask);

            var mockHasher = new Mock<IPasswordHasher<UserModel>>();
            mockHasher.Setup(h => h.HashPassword(It.IsAny<UserModel>(), It.IsAny<string>())).Returns("hashed");

            var mockTransactionTypeRepo = new Mock<ITransactionTypeRepository>();
            var mockTransactionTypeTemplateRepo = new Mock<ITransactionTypeTemplateRepository>();
            mockTransactionTypeTemplateRepo.Setup(r => r.GetDefaultTemplatesAsync())
                .ReturnsAsync(new List<TransactionTypeTemplateModel>());

            var mockJwtService = CreateMockJwtTokenService();

            var service = new AuthServices(
                mockRepo.Object, 
                mockHasher.Object, 
                mockTransactionTypeRepo.Object, 
                mockTransactionTypeTemplateRepo.Object,
                mockJwtService.Object,
                CreateMockRefreshTokenRepository().Object,
                CreateMockJwtSettings());

            var request = new RegistrationRequest
            {
                UserName = "testuser",
                Email = "test@example.com",
                Password = "password123"
            };

            // Act
            var response = await service.RegisterAsync(request);

            UserResponse expectedData = new UserResponse
            {
                UserName = request.UserName,
                Email = request.Email
            };

            // Assert
            Assert.True(response.Success);
            Assert.Equal(AuthConstant.MESSAGE_SUCCESS, response.Message);
            Assert.Equivalent(expectedData, response.Data);
            mockRepo.Verify(r => r.AddUserAsync(It.IsAny<UserModel>()), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_Successful_WithGoogleId()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.EmailExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            mockRepo.Setup(r => r.AddUserAsync(It.IsAny<UserModel>())).Returns(Task.CompletedTask);

            var mockHasher = new Mock<IPasswordHasher<UserModel>>();
            var mockTransactionTypeRepo = new Mock<ITransactionTypeRepository>();
            var mockTransactionTypeTemplateRepo = new Mock<ITransactionTypeTemplateRepository>();
            mockTransactionTypeTemplateRepo.Setup(r => r.GetDefaultTemplatesAsync())
                .ReturnsAsync(new List<TransactionTypeTemplateModel>());

            var mockJwtService = CreateMockJwtTokenService();

            var service = new AuthServices(
                mockRepo.Object, 
                mockHasher.Object, 
                mockTransactionTypeRepo.Object, 
                mockTransactionTypeTemplateRepo.Object,
                mockJwtService.Object,
                CreateMockRefreshTokenRepository().Object,
                CreateMockJwtSettings());

            var request = new RegistrationRequest
            {
                UserName = "testuser",
                Email = "test@example.com",
                GoogleId = "password123"
            };

            // Act
            var response = await service.RegisterAsync(request);

            UserResponse expectedData = new UserResponse
            {
                UserName = request.UserName,
                Email = request.Email
            };

            // Assert
            Assert.True(response.Success);
            Assert.Equal(AuthConstant.MESSAGE_SUCCESS, response.Message);
            Assert.Equivalent(expectedData, response.Data);
            mockRepo.Verify(r => r.AddUserAsync(It.IsAny<UserModel>()), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_Fails_Exception()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.EmailExistsAsync(It.IsAny<string>())).ThrowsAsync(new Exception("DB Error"));

            var mockHasher = new Mock<IPasswordHasher<UserModel>>();
            var mockTransactionTypeRepo = new Mock<ITransactionTypeRepository>();
            var mockTransactionTypeTemplateRepo = new Mock<ITransactionTypeTemplateRepository>();
            mockTransactionTypeTemplateRepo.Setup(r => r.GetDefaultTemplatesAsync())
                .ReturnsAsync(new List<TransactionTypeTemplateModel>());

            var mockJwtService = CreateMockJwtTokenService();

            var service = new AuthServices(
                mockRepo.Object, 
                mockHasher.Object, 
                mockTransactionTypeRepo.Object, 
                mockTransactionTypeTemplateRepo.Object,
                mockJwtService.Object,
                CreateMockRefreshTokenRepository().Object,
                CreateMockJwtSettings());

            var request = new RegistrationRequest
            {
                UserName = "testuser",
                Email = "test@example.com",
                Password = "password123"
            };

            // Act
            var response = await service.RegisterAsync(request);

            // Assert
            Assert.False(response.Success);
            Assert.Equal("DB Error", response.Message);
            Assert.Null(response.Data);
            mockRepo.Verify(r => r.AddUserAsync(It.IsAny<UserModel>()), Times.Never);
        }

        [Fact]
        public async Task RegisterAsync_Fails_DuplicateEmail()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.EmailExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            var mockHasher = new Mock<IPasswordHasher<UserModel>>();
            var mockTransactionTypeRepo = new Mock<ITransactionTypeRepository>();
            var mockTransactionTypeTemplateRepo = new Mock<ITransactionTypeTemplateRepository>();
            mockTransactionTypeTemplateRepo.Setup(r => r.GetDefaultTemplatesAsync())
                .ReturnsAsync(new List<TransactionTypeTemplateModel>());

            var mockJwtService = CreateMockJwtTokenService();

            var service = new AuthServices(
                mockRepo.Object, 
                mockHasher.Object, 
                mockTransactionTypeRepo.Object, 
                mockTransactionTypeTemplateRepo.Object,
                mockJwtService.Object,
                CreateMockRefreshTokenRepository().Object,
                CreateMockJwtSettings());

            var request = new RegistrationRequest
            {
                UserName = "testuser",
                Email = "test@example.com",
                Password = "password123"
            };

            // Act
            var response = await service.RegisterAsync(request);

            // Assert
            Assert.False(response.Success);
            Assert.Equal(AuthConstant.MESSAGE_ERROR_EMAIL_EXIST, response.Message);
            Assert.Null(response.Data);
            mockRepo.Verify(r => r.AddUserAsync(It.IsAny<UserModel>()), Times.Never);
        }

        [Fact]
        public async Task RegisterAsync_Fails_WhenPasswordAndGoogleIdMissing()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.EmailExistsAsync(It.IsAny<string>())).ReturnsAsync(false);

            var mockHasher = new Mock<IPasswordHasher<UserModel>>();
            var mockTransactionTypeRepo = new Mock<ITransactionTypeRepository>();
            var mockTransactionTypeTemplateRepo = new Mock<ITransactionTypeTemplateRepository>();
            mockTransactionTypeTemplateRepo.Setup(r => r.GetDefaultTemplatesAsync())
                .ReturnsAsync(new List<TransactionTypeTemplateModel>());

            var mockJwtService = CreateMockJwtTokenService();

            var service = new AuthServices(
                mockRepo.Object, 
                mockHasher.Object, 
                mockTransactionTypeRepo.Object, 
                mockTransactionTypeTemplateRepo.Object,
                mockJwtService.Object,
                CreateMockRefreshTokenRepository().Object,
                CreateMockJwtSettings());

            var request = new RegistrationRequest
            {
                UserName = "testuser",
                Email = "test@example.com"
                // Password and GoogleId are both missing
            };

            // Act
            var response = await service.RegisterAsync(request);

            // Assert
            Assert.False(response.Success);
            Assert.Equal(AuthConstant.MESSAGE_ERROR_PASSWORD_REQUIRED, response.Message);
            Assert.Null(response.Data);
            mockRepo.Verify(r => r.AddUserAsync(It.IsAny<UserModel>()), Times.Never);
        }

        [Fact]
        public async Task LoginAsync_Successful()
        {
            // Arrange
            var user = new UserModel
            {
                Id = 1,
                UserName = "testuser",
                Email = "test@example.com",
                PasswordHash = "hashedPassword"
            };

            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

            var mockHasher = new Mock<IPasswordHasher<UserModel>>();
            mockHasher.Setup(h => h.VerifyHashedPassword(It.IsAny<UserModel>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(PasswordVerificationResult.Success);

            var mockTransactionTypeRepo = new Mock<ITransactionTypeRepository>();
            var mockTransactionTypeTemplateRepo = new Mock<ITransactionTypeTemplateRepository>();
            var mockJwtService = CreateMockJwtTokenService();

            var service = new AuthServices(
                mockRepo.Object,
                mockHasher.Object,
                mockTransactionTypeRepo.Object,
                mockTransactionTypeTemplateRepo.Object,
                mockJwtService.Object,
                CreateMockRefreshTokenRepository().Object,
                CreateMockJwtSettings());

            var request = new LoginRequest
            {
                Email = "test@example.com",
                Password = "password123"
            };

            // Act
            var response = await service.LoginAsync(request);

            // Assert
            Assert.True(response.Success);
            Assert.Equal("Login successful", response.Message);
            Assert.NotNull(response.Data);
            Assert.Equal(user.Id, response.Data.UserId);
            Assert.Equal(user.Email, response.Data.Email);
            Assert.Equal("mock.jwt.token", response.Data.Token);
            Assert.Equal("mock.refresh.token", response.Data.RefreshToken);
        }

        [Fact]
        public async Task LoginAsync_Fails_UserNotFound()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((UserModel?)null);

            var mockHasher = new Mock<IPasswordHasher<UserModel>>();
            var mockTransactionTypeRepo = new Mock<ITransactionTypeRepository>();
            var mockTransactionTypeTemplateRepo = new Mock<ITransactionTypeTemplateRepository>();
            var mockJwtService = CreateMockJwtTokenService();

            var service = new AuthServices(
                mockRepo.Object,
                mockHasher.Object,
                mockTransactionTypeRepo.Object,
                mockTransactionTypeTemplateRepo.Object,
                mockJwtService.Object,
                CreateMockRefreshTokenRepository().Object,
                CreateMockJwtSettings());

            var request = new LoginRequest
            {
                Email = "test@example.com",
                Password = "password123"
            };

            // Act
            var response = await service.LoginAsync(request);

            // Assert
            Assert.False(response.Success);
            Assert.Equal(AuthConstant.MESSAGE_ERROR_INVALID_CREDENTIALS, response.Message);
            Assert.Null(response.Data);
        }

        [Fact]
        public async Task LoginAsync_Fails_InvalidPassword()
        {
            // Arrange
            var user = new UserModel
            {
                Id = 1,
                UserName = "testuser",
                Email = "test@example.com",
                PasswordHash = "hashedPassword"
            };

            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

            var mockHasher = new Mock<IPasswordHasher<UserModel>>();
            mockHasher.Setup(h => h.VerifyHashedPassword(It.IsAny<UserModel>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(PasswordVerificationResult.Failed);

            var mockTransactionTypeRepo = new Mock<ITransactionTypeRepository>();
            var mockTransactionTypeTemplateRepo = new Mock<ITransactionTypeTemplateRepository>();
            var mockJwtService = CreateMockJwtTokenService();

            var service = new AuthServices(
                mockRepo.Object,
                mockHasher.Object,
                mockTransactionTypeRepo.Object,
                mockTransactionTypeTemplateRepo.Object,
                mockJwtService.Object,
                CreateMockRefreshTokenRepository().Object,
                CreateMockJwtSettings());

            var request = new LoginRequest
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };

            // Act
            var response = await service.LoginAsync(request);

            // Assert
            Assert.False(response.Success);
            Assert.Equal(AuthConstant.MESSAGE_ERROR_INVALID_CREDENTIALS, response.Message);
            Assert.Null(response.Data);
        }

        [Fact]
        public async Task LoginAsync_Fails_GoogleUser()
        {
            // Arrange
            var user = new UserModel
            {
                Id = 1,
                UserName = "testuser",
                Email = "test@example.com",
                GoogleId = "google123",
                PasswordHash = null
            };

            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

            var mockHasher = new Mock<IPasswordHasher<UserModel>>();
            var mockTransactionTypeRepo = new Mock<ITransactionTypeRepository>();
            var mockTransactionTypeTemplateRepo = new Mock<ITransactionTypeTemplateRepository>();
            var mockJwtService = CreateMockJwtTokenService();

            var service = new AuthServices(
                mockRepo.Object,
                mockHasher.Object,
                mockTransactionTypeRepo.Object,
                mockTransactionTypeTemplateRepo.Object,
                mockJwtService.Object,
                CreateMockRefreshTokenRepository().Object,
                CreateMockJwtSettings());

            var request = new LoginRequest
            {
                Email = "test@example.com",
                Password = "password123"
            };

            // Act
            var response = await service.LoginAsync(request);

            // Assert
            Assert.False(response.Success);
            Assert.Contains("Google", response.Message);
            Assert.Null(response.Data);
        }
    }
}

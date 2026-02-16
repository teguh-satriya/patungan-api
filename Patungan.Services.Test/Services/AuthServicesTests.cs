using Microsoft.AspNetCore.Identity;
using Moq;
using Patungan.DataAccess.Entities;
using Patungan.DataAccess.Interfaces;
using Patungan.Services.Services;
using Patungan.Shared.Constants;
using Patungan.Shared.Requests.User;
using Patungan.Shared.Responses.User;

namespace Patungan.Services.Test.Services
{
    public class AuthServiceTests
    {
        [Fact]
        public async Task RegisterAsync_Successful_WithPassword()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.EmailExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            mockRepo.Setup(r => r.AddUserAsync(It.IsAny<UserModel>())).Returns(Task.CompletedTask);

            var mockHasher = new Mock<IPasswordHasher<UserModel>>();
            mockHasher.Setup(h => h.HashPassword(It.IsAny<UserModel>(), It.IsAny<string>())).Returns("hashed");

            var service = new AuthServices(mockRepo.Object, mockHasher.Object);

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

            var service = new AuthServices(mockRepo.Object, mockHasher.Object);

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
            var service = new AuthServices(mockRepo.Object, mockHasher.Object);

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
            var service = new AuthServices(mockRepo.Object, mockHasher.Object);

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
            var service = new AuthServices(mockRepo.Object, mockHasher.Object);

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
    }
}

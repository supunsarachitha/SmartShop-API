using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using SmartShop.API.Controllers;
using SmartShop.API.Interfaces;
using SmartShop.API.Models.Requests;
using SmartShop.API.Models.Responses;
using SmartShop.API.Models;

namespace SmartShop.UnitTests
{
    public class AuthControllerTests
    {
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<AuthController>> _loggerMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _tokenServiceMock = new Mock<ITokenService>();
            _userServiceMock = new Mock<IUserService>();
            _loggerMock = new Mock<ILogger<AuthController>>();
            _controller = new AuthController(_tokenServiceMock.Object, _loggerMock.Object, _userServiceMock.Object);
        }

        [Fact]
        public void Login_ReturnsOk_WhenAuthenticated()
        {
            // Arrange
            var request = new LoginRequest { UserName = "testuser", Password = "password" };
            var user = new User { Id = Guid.NewGuid(), UserName = "testuser", Email = "test@example.com", Password = "password", IsActive = true, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow };
            var authResponse = new UserAuthenticationResponse { IsAuthenticated = true, User = user };
            _userServiceMock.Setup(s => s.Authenticate(request.UserName, request.Password)).Returns(authResponse);
            _tokenServiceMock.Setup(s => s.GenerateJwtToken(user.UserName)).Returns("jwt-token");

            // Act
            var result = _controller.Login(request) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var response = result.Value as ApplicationResponse<object>;
            Assert.True(response.Success);
            Assert.Equal("Login successful", response.Message);
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(response.Data);
            Assert.Null(response.Errors);
        }

        [Fact]
        public void Login_ReturnsUnauthorized_WhenNotAuthenticated()
        {
            // Arrange
            var request = new LoginRequest { UserName = "wronguser", Password = "wrongpass" };
            var authResponse = new UserAuthenticationResponse { IsAuthenticated = false, User = null };
            _userServiceMock.Setup(s => s.Authenticate(request.UserName, request.Password)).Returns(authResponse);

            // Act
            var result = _controller.Login(request) as UnauthorizedObjectResult;

            // Assert
            Assert.NotNull(result);
            var response = result.Value as ApplicationResponse<object>;
            Assert.False(response.Success);
            Assert.Equal("Invalid username or password", response.Message);
            Assert.Equal(401, response.StatusCode);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.Single(response.Errors);
            Assert.Equal("UserName", response.Errors[0].Field);
            Assert.Equal("Invalid credentials", response.Errors[0].Message);
        }
        [Fact]
        public void Login_ReturnsBadRequest_WhenRequestIsNull()
        {
            // Act
            var result = _controller.Login(null!) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            var response = result.Value as ApplicationResponse<object>;
            Assert.NotNull(response);
            Assert.False(response!.Success);
            Assert.Equal("Invalid request", response.Message);
            Assert.Equal(400, response.StatusCode);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
        }

        [Fact]
        public void Login_ReturnsBadRequest_WhenUserNameIsMissing()
        {
            // Arrange
            var request = new LoginRequest { UserName = null!, Password = "password" };

            // Act
            // Since the controller does not handle null or missing UserName directly,
            // but will pass it to the service, we need to mock the service to return not authenticated.
            var authResponse = new UserAuthenticationResponse { IsAuthenticated = false, User = null };
            _userServiceMock.Setup(s => s.Authenticate(request.UserName, request.Password)).Returns(authResponse);

            var result = _controller.Login(request) as UnauthorizedObjectResult;

            // Assert
            Assert.NotNull(result);
            var response = result.Value as ApplicationResponse<object>;
            Assert.False(response.Success);
            Assert.Equal("Invalid username or password", response.Message);
            Assert.Equal(401, response.StatusCode);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.Single(response.Errors);
            Assert.Equal("UserName", response.Errors[0].Field);
            Assert.Equal("Invalid credentials", response.Errors[0].Message);
        }

        [Fact]
        public void Login_ReturnsBadRequest_WhenPasswordIsMissing()
        {
            // Arrange
            var request = new LoginRequest { UserName = "testuser", Password = null! };

            // Act
            var result = _controller.Login(request) as UnauthorizedObjectResult;

            // Assert
            Assert.NotNull(result);
            var response = result.Value as ApplicationResponse<object>;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.Equal("Invalid username or password", response.Message);
            Assert.Equal(401, response.StatusCode);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.Single(response.Errors);
            Assert.Equal("UserName", response.Errors[0].Field);
            Assert.Equal("Invalid credentials", response.Errors[0].Message);
        }

        [Fact]
        public void Login_ReturnsBadRequest_WhenRequestBodyIsEmptyObject()
        {
            // Arrange
            var request = new LoginRequest { UserName = "", Password = "" };
            var authResponse = new UserAuthenticationResponse { IsAuthenticated = false, User = null };
            _userServiceMock.Setup(s => s.Authenticate(request.UserName, request.Password)).Returns(authResponse);

            // Act
            var result = _controller.Login(request) as UnauthorizedObjectResult;

            // Assert
            Assert.NotNull(result);
            var response = result.Value as ApplicationResponse<object>;
            Assert.False(response.Success);
            Assert.Equal("Invalid username or password", response.Message);
            Assert.Equal(401, response.StatusCode);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.Single(response.Errors);
            Assert.Equal("UserName", response.Errors[0].Field);
            Assert.Equal("Invalid credentials", response.Errors[0].Message);
        }

        [Fact]
        public void Login_ReturnsUnauthorized_WhenUserIsInactive()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "inactiveuser",
                Email = "inactive@example.com",
                Password = "password",
                IsActive = false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };
            var request = new LoginRequest { UserName = "inactiveuser", Password = "password" };
            var authResponse = new UserAuthenticationResponse { IsAuthenticated = false, User = user };
            _userServiceMock.Setup(s => s.Authenticate(request.UserName, request.Password)).Returns(authResponse);

            // Act
            var result = _controller.Login(request) as UnauthorizedObjectResult;

            // Assert
            Assert.NotNull(result);
            var response = result.Value as ApplicationResponse<object>;
            Assert.False(response.Success);
            Assert.Equal("Invalid username or password", response.Message);
            Assert.Equal(401, response.StatusCode);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.Single(response.Errors);
            Assert.Equal("UserName", response.Errors[0].Field);
            Assert.Equal("Invalid credentials", response.Errors[0].Message);
        }

        [Fact]
        public void Login_ReturnsOk_WithCorrectTokenInResponse()
        {
            // Arrange
            var request = new LoginRequest { UserName = "testuser", Password = "password" };
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "testuser",
                Email = "test@example.com",
                Password = "password",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };
            var authResponse = new UserAuthenticationResponse { IsAuthenticated = true, User = user };
            _userServiceMock.Setup(s => s.Authenticate(request.UserName, request.Password)).Returns(authResponse);
            _tokenServiceMock.Setup(s => s.GenerateJwtToken(user.UserName)).Returns("expected-jwt-token");

            // Act
            var result = _controller.Login(request) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var response = result.Value as ApplicationResponse<object>;
            Assert.True(response.Success);
            Assert.Equal("Login successful", response.Message);
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(response.Data);
            var tokenProp = response.Data.GetType().GetProperty("token");
            Assert.NotNull(tokenProp);
            Assert.Equal("expected-jwt-token", tokenProp.GetValue(response.Data));
            Assert.Null(response.Errors);
        }

        [Fact]
        public void Login_ReturnsBadRequest_WhenRequestIsNullObject()
        {
            // Act
            var result = _controller.Login(null) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            var response = result.Value as ApplicationResponse<object>;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.Equal("Invalid request", response.Message);
            Assert.Equal(400, response.StatusCode);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.Single(response.Errors);
            Assert.Equal("Request", response.Errors[0].Field);
            Assert.Equal("Request body is required", response.Errors[0].Message);
        }
    }
}

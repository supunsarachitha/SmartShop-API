using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SmartShop.API.Controllers;
using SmartShop.API.Interfaces;
using SmartShop.API.Models;
using SmartShop.API.Models.Responses;
using Xunit;

namespace SmartShop.UnitTests
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<UsersController>> _loggerMock;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _loggerMock = new Mock<ILogger<UsersController>>();
            _controller = new UsersController(_loggerMock.Object, _userServiceMock.Object);
        }

        [Fact]
        public async Task GetUsers_ReturnsOkResult_WithUsers()
        {
            var users = new List<UserDto> { new UserDto { Id = Guid.NewGuid(), UserName = "test", Email = "test@test.com", IsActive = true, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow } };
            var response = new ApplicationResponse<List<UserDto>> { Success = true, Data = users, StatusCode = StatusCodes.Status200OK };
            _userServiceMock.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(response);

            var result = await _controller.GetUsers();

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }

        [Fact]
        public async Task GetUser_ReturnsOkResult_WithUser()
        {
            var userId = Guid.NewGuid();
            var user = new UserDto { Id = userId, UserName = "test", Email = "test@test.com", IsActive = true, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow };
            var response = new ApplicationResponse<UserDto> { Success = true, Data = user, StatusCode = StatusCodes.Status200OK };
            _userServiceMock.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(response);

            var result = await _controller.GetUser(userId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }

        [Fact]
        public async Task PutUser_ReturnsBadRequest_WhenUpdateFails()
        {
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, UserName = "test", Email = "test@test.com", Password = "pass", IsActive = true, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow };
            var response = new ApplicationResponse<UserDto> { Success = false, Data = null, StatusCode = StatusCodes.Status400BadRequest };
            _userServiceMock.Setup(s => s.UpdateUserAsync(userId, user)).ReturnsAsync(response);

            var result = await _controller.PutUser(userId, user);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }

        [Fact]
        public async Task PutUser_ReturnsOkResult_WhenUpdateSucceeds()
        {
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                UserName = "test",
                Email = "test@test.com",
                Password = "pass",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };
            var userDto = new UserDto
            {
                Id = userId,
                UserName = "test",
                Email = "test@test.com",
                IsActive = true,
                CreatedDate = user.CreatedDate,
                UpdatedDate = user.UpdatedDate
            };
            var response = new ApplicationResponse<UserDto>
            {
                Success = true,
                Data = userDto,
                StatusCode = StatusCodes.Status200OK
            };
            _userServiceMock.Setup(s => s.UpdateUserAsync(userId, user)).ReturnsAsync(response);

            var result = await _controller.PutUser(userId, user);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }

        [Fact]
        public async Task PostUser_ReturnsCreatedAtAction_WhenSuccess()
        {
            var user = new User { Id = Guid.NewGuid(), UserName = "test", Email = "test@test.com", Password = "pass", IsActive = true, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow };
            var response = new ApplicationResponse<UserDto> { Success = true, Data = new UserDto { Id = user.Id, UserName = user.UserName, Email = user.Email, IsActive = user.IsActive, CreatedDate = user.CreatedDate, UpdatedDate = user.UpdatedDate }, StatusCode = StatusCodes.Status201Created };
            _userServiceMock.Setup(s => s.CreateUserAsync(user)).ReturnsAsync(response);

            var result = await _controller.PostUser(user);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetUser), createdResult.ActionName);
            Assert.NotNull(createdResult.RouteValues);
            Assert.True(createdResult.RouteValues.ContainsKey("id"));
            Assert.Equal(user.Id, createdResult.RouteValues["id"]);
            Assert.Equal(response, createdResult.Value);
        }

        [Fact]
        public async Task PostUser_ReturnsBadRequest_WhenCreateFails()
        {
            var user = new User { Id = Guid.NewGuid(), UserName = "test", Email = "test@test.com", Password = "pass", IsActive = true, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow };
            var response = new ApplicationResponse<UserDto> { Success = false, Data = null, StatusCode = StatusCodes.Status400BadRequest };
            _userServiceMock.Setup(s => s.CreateUserAsync(user)).ReturnsAsync(response);

            var result = await _controller.PostUser(user);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }


        [Fact]
        public async Task DeleteUser_ReturnsBadRequest_WhenDeleteFails()
        {
            var userId = Guid.NewGuid();
            var response = new ApplicationResponse<UserDto> { Success = false, Data = null, StatusCode = StatusCodes.Status400BadRequest };
            _userServiceMock.Setup(s => s.DeleteUserAsync(userId)).ReturnsAsync(response);

            var result = await _controller.DeleteUser(userId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }

        [Fact]
        public async Task DeleteUser_ReturnsOk_WhenDeleteSucceeds()
        {
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, UserName = "test", Email = "test@test.com", Password = "pass", IsActive = true, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow };
            var response = new ApplicationResponse<UserDto> { Success = true, Data = new UserDto { Id = userId, UserName = user.UserName, Email = user.Email, IsActive = user.IsActive, CreatedDate = user.CreatedDate, UpdatedDate = user.UpdatedDate }, StatusCode = StatusCodes.Status200OK };
            _userServiceMock.Setup(s => s.DeleteUserAsync(userId)).ReturnsAsync(response);

            var result = await _controller.DeleteUser(userId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }

        [Fact]
        public async Task GetUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var response = new ApplicationResponse<UserDto>
            {
                Success = false,
                Data = null,
                StatusCode = StatusCodes.Status404NotFound
            };
            _userServiceMock.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(response);

            var result = await _controller.GetUser(userId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }
        [Fact]
        public async Task PostUser_ReturnsBadRequest_WhenDuplicateUser()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "duplicateUser",
                Email = "duplicate@test.com",
                Password = "pass",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };
            var response = new ApplicationResponse<UserDto>
            {
                Success = false,
                Data = null,
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "A user with the same UserName or Email already exists. Please use unique values."
            };
            _userServiceMock.Setup(s => s.CreateUserAsync(user)).ReturnsAsync(response);

            // Act
            var result = await _controller.PostUser(user);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
            Assert.Equal("A user with the same UserName or Email already exists. Please use unique values.", response.Message);
        }
    }
}

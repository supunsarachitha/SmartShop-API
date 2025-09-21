using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SmartShop.API.Controllers;
using SmartShop.API.Interfaces;
using SmartShop.API.Models;
using SmartShop.API.Models.Responses;

namespace SmartShop.UnitTests;

 
public class SettingsControllerTests
{
    private readonly Mock<ISettingsService> _settingsServiceMock;
    private readonly Mock<ILogger<SettingsController>> _loggerMock;
    private readonly SettingsController _controller;

    public SettingsControllerTests()
    {
        _settingsServiceMock = new Mock<ISettingsService>();
        _loggerMock = new Mock<ILogger<SettingsController>>();
        _controller = new SettingsController(_loggerMock.Object, _settingsServiceMock.Object);
    }

    [Fact]
    public async Task GetSettings_ReturnsOk_WithSettingsList()
    {
        // Arrange
        var settings = new List<Setting>
            {
                new Setting { Id = Guid.NewGuid(), Key = "Theme", Value = "Dark" },
                new Setting { Id = Guid.NewGuid(), Key = "Language", Value = "en-US" }
            };
        var response = new ApplicationResponse<List<Setting>> { Success = true, Data = settings };
        _settingsServiceMock.Setup(s => s.GetAllSettingsAsync()).ReturnsAsync(response);

        // Act
        var result = await _controller.GetSettings();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returned = Assert.IsType<ApplicationResponse<List<Setting>>>(okResult.Value);
        Assert.True(returned.Success);
        Assert.Equal(2, returned.Data.Count);
    }

    [Fact]
    public async Task GetSetting_ReturnsOk_WhenFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var setting = new Setting { Id = id, Key = "Theme", Value = "Dark" };
        var response = new ApplicationResponse<Setting> { Success = true, Data = setting };
        _settingsServiceMock.Setup(s => s.GetSettingByIdAsync(id)).ReturnsAsync(response);

        // Act
        var result = await _controller.GetSetting(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returned = Assert.IsType<ApplicationResponse<Setting>>(okResult.Value);
        Assert.True(returned.Success);
        Assert.Equal(id, returned.Data.Id);
    }

    [Fact]
    public async Task GetSetting_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var response = new ApplicationResponse<Setting> { Success = false, StatusCode = 404 };
        _settingsServiceMock.Setup(s => s.GetSettingByIdAsync(id)).ReturnsAsync(response);

        // Act
        var result = await _controller.GetSetting(id);

        // Assert
        var notFoundResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task PutSetting_ReturnsOk_WhenUpdateSucceeds()
    {
        // Arrange
        var id = Guid.NewGuid();
        var setting = new Setting { Id = id, Key = "Theme", Value = "Light" };
        var response = new ApplicationResponse<Setting> { Success = true, Data = setting };
        _settingsServiceMock.Setup(s => s.UpdateSettingAsync(id, setting)).ReturnsAsync(response);

        // Act
        var result = await _controller.PutSetting(id, setting);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returned = Assert.IsType<ApplicationResponse<Setting>>(okResult.Value);
        Assert.True(returned.Success);
    }

    [Fact]
    public async Task PutSetting_ReturnsBadRequest_WhenIdMismatch()
    {
        // Arrange
        var id = Guid.NewGuid();
        var setting = new Setting { Id = Guid.NewGuid(), Key = "Theme", Value = "Light" };

        // Act
        var result = await _controller.PutSetting(id, setting);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task PostSetting_ReturnsCreated_WhenSuccess()
    {
        // Arrange
        var setting = new Setting { Id = Guid.NewGuid(), Key = "Theme", Value = "Dark" };
        var response = new ApplicationResponse<Setting> { Success = true, Data = setting };
        _settingsServiceMock.Setup(s => s.CreateSettingAsync(setting)).ReturnsAsync(response);

        // Act
        var result = await _controller.PostSetting(setting);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var returned = Assert.IsType<ApplicationResponse<Setting>>(createdResult.Value);
        Assert.True(returned.Success);
    }

    [Fact]
    public async Task PostSetting_ReturnsBadRequest_WhenFailed()
    {
        // Arrange
        var setting = new Setting { Id = Guid.NewGuid(), Key = "Theme", Value = "Dark" };
        var response = new ApplicationResponse<Setting> { Success = false, StatusCode = 400 };
        _settingsServiceMock.Setup(s => s.CreateSettingAsync(setting)).ReturnsAsync(response);

        // Act
        var result = await _controller.PostSetting(setting);

        // Assert
        var badRequest = Assert.IsType<ObjectResult>(result);
        Assert.Equal(400, badRequest.StatusCode);
    }

    [Fact]
    public async Task DeleteSetting_ReturnsOk_WhenSuccess()
    {
        // Arrange
        var id = Guid.NewGuid();
        var response = new ApplicationResponse<Setting> { Success = true, Data = new Setting { Id = id } };
        _settingsServiceMock.Setup(s => s.DeleteSettingAsync(id)).ReturnsAsync(response);

        // Act
        var result = await _controller.DeleteSetting(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returned = Assert.IsType<ApplicationResponse<Setting>>(okResult.Value);
        Assert.True(returned.Success);
        Assert.Equal(id, returned.Data.Id);
    }

    [Fact]
    public async Task DeleteSetting_ReturnsNotFound_WhenFailed()
    {
        // Arrange
        var id = Guid.NewGuid();
        var response = new ApplicationResponse<Setting> { Success = false, StatusCode = 404 };
        _settingsServiceMock.Setup(s => s.DeleteSettingAsync(id)).ReturnsAsync(response);

        // Act
        var result = await _controller.DeleteSetting(id);

        // Assert
        var notFoundResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
}

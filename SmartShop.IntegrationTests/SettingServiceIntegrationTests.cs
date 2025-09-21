using SmartShop.API.Helpers;
using SmartShop.API.Models;
using SmartShop.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShop.IntegrationTests
{
    public class SettingServiceIntegrationTests
    {
        private readonly DateTimeProvider _dateTimeProvider = new DateTimeProvider();

        private SmartShopDbContext CreateContext() => TestDbContextFactory.CreateContext();

        private SettingsService GetService(SmartShopDbContext context)
        {
            return new SettingsService(context, _dateTimeProvider);
        }

        [Fact]
        public async Task CreateSettingAsync_ShouldCreateSetting()
        {
            using var context = CreateContext();
            var service = GetService(context);
            var setting = new Setting
            {
                Id = Guid.NewGuid(),
                Key = "TestKey",
                Value = "TestValue",
                Description = "Test Description",
                CreatedDate = _dateTimeProvider.UtcNow
            };

            var response = await service.CreateSettingAsync(setting);

            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal("TestKey", response.Data.Key);
        }

        [Fact]
        public async Task GetAllSettingsAsync_ShouldReturnSettings()
        {
            using var context = CreateContext();
            var service = GetService(context);
            var setting = new Setting
            {
                Id = Guid.NewGuid(),
                Key = "AllKey",
                Value = "AllValue",
                Description = "All Description",
                CreatedDate = _dateTimeProvider.UtcNow
            };
            context.Settings.Add(setting);
            await context.SaveChangesAsync();

            var response = await service.GetAllSettingsAsync();

            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Contains(response.Data, s => s.Key == "AllKey");
        }

        [Fact]
        public async Task GetSettingByIdAsync_ShouldReturnSetting()
        {
            using var context = CreateContext();
            var service = GetService(context);
            var setting = new Setting
            {
                Id = Guid.NewGuid(),
                Key = "ByIdKey",
                Value = "ByIdValue",
                Description = "ById Description",
                CreatedDate = _dateTimeProvider.UtcNow
            };
            context.Settings.Add(setting);
            await context.SaveChangesAsync();

            var response = await service.GetSettingByIdAsync(setting.Id);

            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal("ByIdKey", response.Data.Key);
        }

        [Fact]
        public async Task UpdateSettingAsync_ShouldUpdateSetting()
        {
            using var context = CreateContext();
            var service = GetService(context);
            var setting = new Setting
            {
                Id = Guid.NewGuid(),
                Key = "UpdateKey",
                Value = "UpdateValue",
                Description = "Update Description",
                CreatedDate = _dateTimeProvider.UtcNow
            };
            context.Settings.Add(setting);
            await context.SaveChangesAsync();

            var updatedSetting = new Setting
            {
                Id = setting.Id, // Ensure the Id matches for update
                Key = "UpdatedKey",
                Value = "UpdatedValue",
                Description = "Updated Description",
                UpdatedDate = _dateTimeProvider.UtcNow
            };

            var response = await service.UpdateSettingAsync(setting.Id, updatedSetting);

            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal("UpdatedKey", response.Data.Key);
        }

        [Fact]
        public async Task DeleteSettingAsync_ShouldDeleteSetting()
        {
            using var context = CreateContext();
            var service = GetService(context);
            var setting = new Setting
            {
                Id = Guid.NewGuid(),
                Key = "DeleteKey",
                Value = "DeleteValue",
                Description = "Delete Description",
                CreatedDate = _dateTimeProvider.UtcNow
            };
            context.Settings.Add(setting);
            await context.SaveChangesAsync();

            var response = await service.DeleteSettingAsync(setting.Id);

            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal("DeleteKey", response.Data.Key);

            var getResponse = await service.GetSettingByIdAsync(setting.Id);
            Assert.False(getResponse.Success);
            Assert.Null(getResponse.Data);
        }
    }
}

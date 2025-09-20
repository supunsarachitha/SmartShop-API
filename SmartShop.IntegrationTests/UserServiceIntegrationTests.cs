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
    public class UserServiceIntegrationTests
    {
        private SmartShopDbContext CreateContext() => TestDbContextFactory.CreateContext();

        DateTimeProvider GetDateTimeProvider()
        {
            return new DateTimeProvider();
        }

        private UserService GetService(SmartShopDbContext context)
        {
            return new UserService(context, GetDateTimeProvider());
        }

        [Fact]
        public async Task CreateUserAsync_ShouldCreateUser()
        {
            using var context = CreateContext();
            var service = GetService(context);
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "testuser",
                Email = "testuser@example.com",
                Password = "password123"
            };

            var response = await service.CreateUserAsync(user);

            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal(user.UserName, response.Data.UserName);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnUsers()
        {
            using var context = CreateContext();
            var service = GetService(context);
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "anotheruser",
                Email = "anotheruser@example.com",
                Password = "password456"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var response = await service.GetAllUsersAsync();

            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Contains(response.Data, u => u.Id == user.Id);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser()
        {
            using var context = CreateContext();
            var service = GetService(context);
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "findme",
                Email = "findme@example.com",
                Password = "password789"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var response = await service.GetUserByIdAsync(user.Id);

            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal(user.Id, response.Data.Id);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldUpdateUser()
        {
            using var context = CreateContext();
            var service = GetService(context);
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "updateme",
                Email = "updateme@example.com",
                Password = "password000"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var updatedUser = new User
            {
                Id = user.Id,
                UserName = "updateduser",
                Email = "updateduser@example.com",
                Password = "Ab123456789@"
            };

            var response = await service.UpdateUserAsync(user.Id, updatedUser);

            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal(updatedUser.UserName, response.Data.UserName);
            Assert.Equal(updatedUser.Email, response.Data.Email);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldReturnError_WhenUserNotFound()
        {
            using var context = CreateContext();
            var service = GetService(context);
            var nonExistentId = Guid.NewGuid();
            var user = new User
            {
                Id = nonExistentId,
                UserName = "ghost",
                Email = "ghost@example.com",
                Password = "ghostpass"
            };

            var response = await service.UpdateUserAsync(nonExistentId, user);

            Assert.False(response.Success);
            Assert.Null(response.Data);
            Assert.NotNull(response.Message);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldDeleteUser()
        {
            using var context = CreateContext();
            var service = GetService(context);
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "deleteme",
                Email = "deleteme@example.com",
                Password = "deletepass"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var response = await service.DeleteUserAsync(user.Id);

            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal(user.Id, response.Data.Id);

            var dbUser = await context.Users.FindAsync(user.Id);
            Assert.Null(dbUser);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnError_WhenUserNotFound()
        {
            using var context = CreateContext();
            var service = GetService(context);
            var nonExistentId = Guid.NewGuid();

            var response = await service.DeleteUserAsync(nonExistentId);

            Assert.False(response.Success);
            Assert.Null(response.Data);
            Assert.NotNull(response.Message);
        }
    }
}

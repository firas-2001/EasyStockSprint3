using EasyStock.Models;
using EasyStock.Service;
using Microsoft.EntityFrameworkCore;

namespace EasyStock.Sprint3.Tests;

public class UserServiceTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnUsersOrderedWithRoles()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = new UserService(context, new AuthService(context));

        var users = await service.GetAllAsync();

        Assert.Equal(2, users.Count);
        Assert.Equal("admin-test", users[0].Username);
        Assert.All(users, user => Assert.NotNull(user.Role));
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUserWithRole()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = new UserService(context, new AuthService(context));

        var user = await service.GetByIdAsync(1);

        Assert.NotNull(user);
        Assert.Equal("admin-test", user!.Username);
        Assert.Equal("Admin", user.Role!.RoleName);
    }

    [Fact]
    public async Task GetRolesAsync_ShouldReturnRolesOrdered()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = new UserService(context, new AuthService(context));

        var roles = await service.GetRolesAsync();

        Assert.Equal(3, roles.Count);
        Assert.Equal(1, roles[0].RoleId);
        Assert.Equal("Admin", roles[0].RoleName);
    }

    [Fact]
    public async Task CreateAsync_ShouldPersistUser_WithHashedPassword()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var authService = new AuthService(context);
        var service = new UserService(context, authService);

        var user = new User
        {
            Username = "nouveau.user",
            Email = "nouveau@test.local",
            RoleId = 2,
            PlainPassword = "Gestion123!",
            ConfirmPassword = "Gestion123!"
        };

        await service.CreateAsync(user, "test-admin");

        var persisted = await context.User.FirstOrDefaultAsync(u => u.Username == "nouveau.user");
        Assert.NotNull(persisted);
        Assert.NotEqual("Gestion123!", persisted!.PwdHash);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenPasswordMissing()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = new UserService(context, new AuthService(context));

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(new User
        {
            Username = "nouveau.user",
            Email = "nouveau@test.local",
            RoleId = 2,
            PlainPassword = string.Empty
        }, "test-admin"));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenUsernameAlreadyExists()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = new UserService(context, new AuthService(context));

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(new User
        {
            Username = "admin-test",
            Email = "autre@test.local",
            RoleId = 2,
            PlainPassword = "Gestion123!"
        }, "test-admin"));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenEmailAlreadyExists()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = new UserService(context, new AuthService(context));

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(new User
        {
            Username = "autre.user",
            Email = "admin@test.local",
            RoleId = 2,
            PlainPassword = "Gestion123!"
        }, "test-admin"));
    }

    [Fact]
    public async Task UpdateAsync_ShouldKeepPassword_WhenNoNewPasswordProvided()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var authService = new AuthService(context);
        var service = new UserService(context, authService);
        var existing = await context.User.FirstAsync(u => u.UserId == 2);
        var originalHash = existing.PwdHash;

        await service.UpdateAsync(new User
        {
            UserId = existing.UserId,
            Username = existing.Username,
            Email = "operateur.maj@test.local",
            RoleId = 3,
            PlainPassword = string.Empty,
            ConfirmPassword = string.Empty
        }, "test-admin");

        var persisted = await context.User.FirstAsync(u => u.UserId == 2);
        Assert.Equal("operateur.maj@test.local", persisted.Email);
        Assert.Equal(originalHash, persisted.PwdHash);
    }

    [Fact]
    public async Task UpdateAsync_ShouldHashNewPassword_WhenProvided()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var authService = new AuthService(context);
        var service = new UserService(context, authService);
        var existing = await context.User.FirstAsync(u => u.UserId == 2);
        var originalHash = existing.PwdHash;

        await service.UpdateAsync(new User
        {
            UserId = existing.UserId,
            Username = "operateur-test",
            Email = existing.Email,
            RoleId = existing.RoleId,
            PlainPassword = "NouveauPass123!"
        }, "test-admin");

        var persisted = await context.User.FirstAsync(u => u.UserId == 2);
        Assert.NotEqual(originalHash, persisted.PwdHash);
        Assert.True(authService.VerifyPassword("NouveauPass123!", persisted.PwdHash));
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenUserNotFound()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = new UserService(context, new AuthService(context));

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateAsync(new User
        {
            UserId = 999,
            Username = "introuvable",
            Email = "introuvable@test.local",
            RoleId = 2
        }, "test-admin"));
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenUsernameAlreadyExists()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        context.User.Add(new User
        {
            UserId = 3,
            Username = "gestionnaire-test",
            Email = "gestionnaire@test.local",
            PwdHash = new AuthService(context).HashPassword("Gestion123!"),
            RoleId = 2,
            CreatedBy = "test",
            UpdatedBy = "test",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync();
        var service = new UserService(context, new AuthService(context));

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateAsync(new User
        {
            UserId = 2,
            Username = "gestionnaire-test",
            Email = "operateur@test.local",
            RoleId = 3
        }, "test-admin"));
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenEmailAlreadyExists()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        context.User.Add(new User
        {
            UserId = 3,
            Username = "gestionnaire-test",
            Email = "gestionnaire@test.local",
            PwdHash = new AuthService(context).HashPassword("Gestion123!"),
            RoleId = 2,
            CreatedBy = "test",
            UpdatedBy = "test",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync();
        var service = new UserService(context, new AuthService(context));

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateAsync(new User
        {
            UserId = 2,
            Username = "operateur-test",
            Email = "gestionnaire@test.local",
            RoleId = 3
        }, "test-admin"));
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrow_WhenDeletingCurrentUser()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = new UserService(context, new AuthService(context));

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteAsync(2, 2));
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrow_WhenUserDoesNotExist()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = new UserService(context, new AuthService(context));

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteAsync(999, 1));
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveUser_WhenAllowed()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = new UserService(context, new AuthService(context));

        await service.DeleteAsync(2, 1);

        Assert.False(await context.User.AnyAsync(u => u.UserId == 2));
    }
}

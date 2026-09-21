using EasyStock.Models;
using EasyStock.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace EasyStock.Sprint3.Tests;

public class AuthServiceTests
{
    [Fact]
    public async Task ValidateCredentialsAsync_ShouldReturnUser_WhenPasswordMatches()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = new AuthService(context);

        var user = await service.ValidateCredentialsAsync("admin-test", "Admin123!");

        Assert.NotNull(user);
        Assert.Equal("Admin", user!.Role!.RoleName);
    }

    [Fact]
    public async Task ValidateCredentialsAsync_ShouldReturnNull_WhenPasswordIsInvalid()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = new AuthService(context);

        var user = await service.ValidateCredentialsAsync("admin-test", "BadPassword1!");

        Assert.Null(user);
    }

    [Fact]
    public async Task ValidateCredentialsAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = new AuthService(context);

        var user = await service.ValidateCredentialsAsync("introuvable", "Admin123!");

        Assert.Null(user);
    }

    [Fact]
    public void HashPassword_AndVerifyPassword_ShouldBehaveConsistently()
    {
        using var context = TestDbFactory.CreateContext();
        var service = new AuthService(context);

        var hash = service.HashPassword("Admin123!");

        Assert.NotEqual("Admin123!", hash);
        Assert.True(service.VerifyPassword("Admin123!", hash));
        Assert.False(service.VerifyPassword("BadPassword1!", hash));
    }

    [Fact]
    public void SignIn_AndSignOut_ShouldManageSessionValues()
    {
        using var context = TestDbFactory.CreateContext();
        var service = new AuthService(context);
        var session = new TestSession();
        var httpContext = new DefaultHttpContext();
        httpContext.Features.Set<ISessionFeature>(new TestSessionFeature { Session = session });

        service.SignIn(httpContext, new User
        {
            UserId = 42,
            Username = "admin-test",
            Role = new Role { RoleName = "Admin" }
        });

        Assert.Equal(42, httpContext.Session.GetInt32("UserId"));
        Assert.Equal("admin-test", httpContext.Session.GetString("Username"));
        Assert.Equal("Admin", httpContext.Session.GetString("Role"));
        Assert.Equal("true", httpContext.Session.GetString("IsAuthenticated"));

        service.SignOut(httpContext);

        Assert.Null(httpContext.Session.GetInt32("UserId"));
        Assert.Null(httpContext.Session.GetString("Username"));
        Assert.Null(httpContext.Session.GetString("Role"));
        Assert.Null(httpContext.Session.GetString("IsAuthenticated"));
    }

    private sealed class TestSessionFeature : ISessionFeature
    {
        public ISession Session { get; set; } = new TestSession();
    }

    private sealed class TestSession : ISession
    {
        private readonly Dictionary<string, byte[]> _store = new();

        public IEnumerable<string> Keys => _store.Keys;
        public string Id { get; } = Guid.NewGuid().ToString();
        public bool IsAvailable => true;

        public void Clear() => _store.Clear();
        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public void Remove(string key) => _store.Remove(key);
        public void Set(string key, byte[] value) => _store[key] = value;
        public bool TryGetValue(string key, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out byte[]? value) => _store.TryGetValue(key, out value);
    }
}


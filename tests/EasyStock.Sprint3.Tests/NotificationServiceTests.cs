using EasyStock.Data;
using EasyStock.Service;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace EasyStock.Sprint3.Tests;

public class NotificationServiceTests
{
    [Fact]
    public async Task SendLowStockAlertAsync_ShouldReturnFalseWhenDisabled()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var article = await context.Article.FindAsync(300);
        var service = new NotificationService(
            context,
            Options.Create(new EmailSettings { Enabled = false }),
            NullLogger<NotificationService>.Instance);

        var sent = await service.SendLowStockAlertAsync(article!, "gestionnaire-test");

        Assert.False(sent);
    }
}

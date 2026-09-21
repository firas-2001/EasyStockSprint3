using EasyStock.Data;
using EasyStock.Models;
using EasyStock.Models.ViewModels;
using EasyStock.Service;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace EasyStock.Sprint3.Tests;

public class StockMovementServiceTests
{
    [Fact]
    public async Task RegisterEntryAsync_ShouldIncreaseStockAndPersistMovement()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = CreateService(context);

        var model = new StockMovementFormViewModel
        {
            ArticleId = 300,
            Quantite = 3,
            ReferenceExterne = "PO-001",
            Motif = "Réapprovisionnement test",
            EstReapprovisionnement = true
        };

        var movement = await service.RegisterEntryAsync(model, "gestionnaire-test", 1);

        Assert.Equal(MouvementType.Entree, movement.TypeMouvement);
        Assert.Equal(5, (await context.Article.FindAsync(300))!.NombreArticleActuel);
        Assert.True(movement.EstReapprovisionnement);
        Assert.Equal(1, await context.MouvementStock.CountAsync());
    }

    [Fact]
    public async Task RegisterOutputAsync_ShouldDecreaseStock()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = CreateService(context);

        var model = new StockMovementFormViewModel
        {
            ArticleId = 300,
            Quantite = 1,
            ReferenceExterne = "OUT-001",
            Motif = "Sortie test"
        };

        var movement = await service.RegisterOutputAsync(model, "operateur-test", 2);

        Assert.Equal(MouvementType.Sortie, movement.TypeMouvement);
        Assert.Equal(1, movement.StockApres);
        Assert.Equal(1, (await context.Article.FindAsync(300))!.NombreArticleActuel);
    }

    [Fact]
    public async Task RegisterOutputAsync_ShouldThrowWhenStockInsufficient()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = CreateService(context);

        var model = new StockMovementFormViewModel
        {
            ArticleId = 301,
            Quantite = 5,
            Motif = "Tentative invalide"
        };

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.RegisterOutputAsync(model, "operateur-test", 2));

        Assert.Equal("Stock insuffisant pour enregistrer cette sortie.", exception.Message);
        Assert.Equal(1, (await context.Article.FindAsync(301))!.NombreArticleActuel);
    }

    [Fact]
    public async Task RegisterReturnAsync_ShouldIncreaseStock()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = CreateService(context);

        var model = new StockMovementFormViewModel
        {
            ArticleId = 301,
            Quantite = 2,
            ReferenceExterne = "RET-001",
            Motif = "Retour test"
        };

        var movement = await service.RegisterReturnAsync(model, "operateur-test", 2);

        Assert.Equal(MouvementType.Retour, movement.TypeMouvement);
        Assert.Equal(3, (await context.Article.FindAsync(301))!.NombreArticleActuel);
    }

    [Fact]
    public async Task GetRecentAsync_ShouldReturnMovementsWithRelations()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = CreateService(context);

        await service.RegisterEntryAsync(new StockMovementFormViewModel { ArticleId = 300, Quantite = 1 }, "gestionnaire-test", 1);
        await service.RegisterReturnAsync(new StockMovementFormViewModel { ArticleId = 301, Quantite = 1 }, "operateur-test", 2);

        var items = await service.GetRecentAsync();

        Assert.Equal(2, items.Count);
        Assert.NotNull(items[0].Article);
        Assert.NotNull(items[0].User);
    }

    private static StockMovementService CreateService(ApplicationDbContext context)
    {
        var notification = new NotificationService(
            context,
            Options.Create(new EmailSettings { Enabled = false }),
            NullLogger<NotificationService>.Instance);

        return new StockMovementService(context, notification);
    }
}

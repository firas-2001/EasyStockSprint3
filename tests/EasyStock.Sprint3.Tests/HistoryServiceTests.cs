using EasyStock.Models;
using EasyStock.Models.ViewModels;
using EasyStock.Service;

namespace EasyStock.Sprint3.Tests;

public class HistoryServiceTests
{
    [Fact]
    public async Task GetHistoryAsync_ShouldFilterByTypeAndUser()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        context.MouvementStock.AddRange(
            new MouvementStock { MouvementStockId = 1, ReferenceCode = "ENT-TEST-1", ArticleId = 300, UserId = 1, TypeMouvement = MouvementType.Entree, Quantite = 2, StockAvant = 2, StockApres = 4, CreatedBy = "gestionnaire-test", CreatedAt = DateTime.UtcNow.AddDays(-1) },
            new MouvementStock { MouvementStockId = 2, ReferenceCode = "SOR-TEST-2", ArticleId = 301, UserId = 2, TypeMouvement = MouvementType.Sortie, Quantite = 1, StockAvant = 1, StockApres = 0, CreatedBy = "operateur-test", CreatedAt = DateTime.UtcNow }
        );
        await context.SaveChangesAsync();

        var service = new HistoryService(context);
        var result = await service.GetHistoryAsync(new StockMovementHistoryViewModel
        {
            TypeMouvement = MouvementType.Sortie,
            Username = "operateur"
        });

        Assert.Single(result);
        Assert.Equal("SOR-TEST-2", result[0].ReferenceCode);
    }

    [Fact]
    public async Task GetHistoryAsync_ShouldFilterByDateAndArticle()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        context.MouvementStock.AddRange(
            new MouvementStock { MouvementStockId = 1, ReferenceCode = "ENT-OLD", ArticleId = 300, UserId = 1, TypeMouvement = MouvementType.Entree, Quantite = 1, StockAvant = 1, StockApres = 2, CreatedBy = "gestionnaire-test", CreatedAt = DateTime.UtcNow.AddDays(-10) },
            new MouvementStock { MouvementStockId = 2, ReferenceCode = "ENT-NEW", ArticleId = 301, UserId = 1, TypeMouvement = MouvementType.Entree, Quantite = 2, StockAvant = 1, StockApres = 3, CreatedBy = "gestionnaire-test", CreatedAt = DateTime.UtcNow.AddDays(-1) }
        );
        await context.SaveChangesAsync();

        var service = new HistoryService(context);
        var result = await service.GetHistoryAsync(new StockMovementHistoryViewModel
        {
            StartDate = DateTime.UtcNow.AddDays(-2),
            ArticleId = 301
        });

        Assert.Single(result);
        Assert.Equal("ENT-NEW", result[0].ReferenceCode);
    }

    [Fact]
    public async Task GetReplenishmentHistoryAsync_ShouldReturnOnlyEntriesAndReplenishments()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        context.MouvementStock.AddRange(
            new MouvementStock { MouvementStockId = 1, ReferenceCode = "ENT-REP", ArticleId = 300, UserId = 1, TypeMouvement = MouvementType.Entree, Quantite = 2, StockAvant = 2, StockApres = 4, EstReapprovisionnement = true, CreatedBy = "gestionnaire-test", CreatedAt = DateTime.UtcNow },
            new MouvementStock { MouvementStockId = 2, ReferenceCode = "RET-001", ArticleId = 301, UserId = 2, TypeMouvement = MouvementType.Retour, Quantite = 1, StockAvant = 0, StockApres = 1, CreatedBy = "operateur-test", CreatedAt = DateTime.UtcNow }
        );
        await context.SaveChangesAsync();

        var service = new HistoryService(context);
        var result = await service.GetReplenishmentHistoryAsync(new StockMovementHistoryViewModel());

        Assert.Single(result);
        Assert.Equal("ENT-REP", result[0].ReferenceCode);
    }
    [Fact]
    public async Task GetHistoryAsync_ShouldIncludeUtcMovementsWithinSameLocalDay()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);

        var localZone = TimeZoneInfo.Local;
        var localMoment = new DateTime(2026, 4, 13, 20, 52, 0, DateTimeKind.Unspecified);
        var utcMoment = TimeZoneInfo.ConvertTimeToUtc(localMoment, localZone);

        context.MouvementStock.Add(new MouvementStock
        {
            MouvementStockId = 3,
            ReferenceCode = "SOR-LOCAL-DAY",
            ArticleId = 300,
            UserId = 1,
            TypeMouvement = MouvementType.Sortie,
            Quantite = 1,
            StockAvant = 9,
            StockApres = 8,
            CreatedBy = "gestionnaire-test",
            CreatedAt = utcMoment
        });
        await context.SaveChangesAsync();

        var service = new HistoryService(context);
        var result = await service.GetHistoryAsync(new StockMovementHistoryViewModel
        {
            StartDate = new DateTime(2026, 4, 13),
            EndDate = new DateTime(2026, 4, 13)
        });

        Assert.Contains(result, item => item.ReferenceCode == "SOR-LOCAL-DAY");
    }
}

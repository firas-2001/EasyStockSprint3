using EasyStock.Models;
using EasyStock.Models.ViewModels;
using EasyStock.Service;

namespace EasyStock.Sprint3.Tests;

public class ReportAndExportServiceTests
{
    [Fact]
    public async Task BuildInventoryReportAsync_ShouldReturnRows()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var historyService = new HistoryService(context);
        var reportService = new ReportService(context, historyService);

        var report = await reportService.BuildInventoryReportAsync();

        Assert.Equal("Inventaire global", report.Title);
        Assert.Contains("Équipement", report.Headers);
        Assert.NotEmpty(report.Rows);
    }

    [Fact]
    public async Task BuildLowStockReportAsync_ShouldReturnOnlyLowStockArticles()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var article = await context.Article.FindAsync(300);
        article!.NombreArticleActuel = 1;
        article.NombreArticleMinimum = 2;
        await context.SaveChangesAsync();

        var reportService = new ReportService(context, new HistoryService(context));
        var report = await reportService.BuildLowStockReportAsync();

        Assert.Single(report.Rows);
        Assert.Equal("300", report.Rows[0][0]);
    }

    [Fact]
    public async Task BuildMovementReportAsync_ShouldRespectFilters()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        context.MouvementStock.AddRange(
            new MouvementStock { MouvementStockId = 1, ReferenceCode = "ENT-RPT", ArticleId = 300, UserId = 1, TypeMouvement = MouvementType.Entree, Quantite = 2, StockAvant = 1, StockApres = 3, CreatedBy = "gestionnaire-test", CreatedAt = DateTime.UtcNow },
            new MouvementStock { MouvementStockId = 2, ReferenceCode = "SOR-RPT", ArticleId = 301, UserId = 2, TypeMouvement = MouvementType.Sortie, Quantite = 1, StockAvant = 1, StockApres = 0, CreatedBy = "operateur-test", CreatedAt = DateTime.UtcNow }
        );
        await context.SaveChangesAsync();

        var reportService = new ReportService(context, new HistoryService(context));
        var report = await reportService.BuildMovementReportAsync(new StockMovementHistoryViewModel { TypeMouvement = MouvementType.Sortie });

        Assert.Single(report.Rows);
        Assert.Equal("SOR-RPT", report.Rows[0][0]);
    }

    [Fact]
    public async Task BuildReplenishmentReportAsync_ShouldReturnEntries()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        context.MouvementStock.Add(new MouvementStock { MouvementStockId = 1, ReferenceCode = "ENT-RAPP", ArticleId = 300, UserId = 1, TypeMouvement = MouvementType.Entree, Quantite = 5, StockAvant = 2, StockApres = 7, EstReapprovisionnement = true, CreatedBy = "gestionnaire-test", CreatedAt = DateTime.UtcNow });
        await context.SaveChangesAsync();

        var reportService = new ReportService(context, new HistoryService(context));
        var report = await reportService.BuildReplenishmentReportAsync(new StockMovementHistoryViewModel());

        Assert.Single(report.Rows);
        Assert.Equal("ENT-RAPP", report.Rows[0][0]);
    }

    [Fact]
    public void ExportCsv_ShouldContainHeadersAndRows()
    {
        var service = new ExportService();
        var dataset = new ReportDataset
        {
            Title = "Test",
            Headers = new List<string> { "Col1", "Col2" },
            Rows = new List<IReadOnlyList<string>> { new List<string> { "A", "B" } }
        };

        var bytes = service.ExportCsv(dataset);
        var text = System.Text.Encoding.UTF8.GetString(bytes);

        Assert.Contains("Col1", text);
        Assert.Contains("A", text);
    }

    [Fact]
    public void ExportExcel_ShouldReturnDocumentBytes()
    {
        var service = new ExportService();
        var dataset = new ReportDataset
        {
            Title = "Test",
            Headers = new List<string> { "Col1" },
            Rows = new List<IReadOnlyList<string>> { new List<string> { "Valeur" } }
        };

        var bytes = service.ExportExcel(dataset);
        Assert.True(bytes.Length > 0);
    }

    [Fact]
    public void ExportPdf_ShouldReturnDocumentBytes()
    {
        var service = new ExportService();
        var dataset = new ReportDataset
        {
            Title = "Test",
            Headers = new List<string> { "Col1" },
            Rows = new List<IReadOnlyList<string>> { new List<string> { "Valeur" } }
        };

        var bytes = service.ExportPdf(dataset);
        Assert.True(bytes.Length > 0);
    }
    [Fact]
    public async Task BuildMovementReportAsync_ShouldIncludeUtcMovementsInsideSelectedLocalDay()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);

        var localZone = TimeZoneInfo.Local;
        var localMoment = new DateTime(2026, 4, 13, 20, 52, 0, DateTimeKind.Unspecified);
        var utcMoment = TimeZoneInfo.ConvertTimeToUtc(localMoment, localZone);

        context.MouvementStock.Add(new MouvementStock
        {
            MouvementStockId = 3,
            ReferenceCode = "SOR-RPT-LOCAL-DAY",
            ArticleId = 300,
            UserId = 1,
            TypeMouvement = MouvementType.Sortie,
            Quantite = 1,
            StockAvant = 9,
            StockApres = 8,
            CreatedBy = "gestionnaire-test",
            CreatedAt = utcMoment,
            ReferenceExterne = "AFF-0007-0004"
        });
        await context.SaveChangesAsync();

        var reportService = new ReportService(context, new HistoryService(context));
        var report = await reportService.BuildMovementReportAsync(new StockMovementHistoryViewModel
        {
            StartDate = new DateTime(2026, 4, 13),
            EndDate = new DateTime(2026, 4, 13)
        });

        Assert.Contains(report.Rows, row => row[0] == "SOR-RPT-LOCAL-DAY");
    }
}

using EasyStock.Data;
using EasyStock.Models;
using EasyStock.Models.ViewModels;
using EasyStock.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace EasyStock.Sprint3.Tests;

public class AffectationServiceTests
{
    [Fact]
    public async Task GetAssignableArticlesAsync_ShouldReturnOnlyArticlesWithStock()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var articleWithoutStock = await context.Article.FirstAsync(a => a.ArticleId == 301);
        articleWithoutStock.NombreArticleActuel = 0;
        await context.SaveChangesAsync();
        var service = CreateService(context);

        var articles = await service.GetAssignableArticlesAsync();

        Assert.Contains(articles, a => a.ArticleId == 300);
        Assert.DoesNotContain(articles, a => a.ArticleId == 301);
    }

    [Fact]
    public async Task GetActiveAsync_ShouldReturnOnlyActiveAffectations_AndFilterByNom()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        context.Affectation.AddRange(
            new Affectation
            {
                AffectationId = 10,
                ReferenceCode = "AFF-0300-0001",
                ArticleId = 300,
                EmployeId = 10,
                EtatSortie = "Fonctionnel",
                AssignedAt = DateTime.UtcNow.AddDays(-2),
                IsActive = true,
                CreatedBy = "test",
                UpdatedBy = "test",
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                UpdatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new Affectation
            {
                AffectationId = 11,
                ReferenceCode = "AFF-0300-0002",
                ArticleId = 300,
                EmployeId = 11,
                EtatSortie = "Fonctionnel",
                AssignedAt = DateTime.UtcNow.AddDays(-1),
                IsActive = false,
                ReturnedAt = DateTime.UtcNow,
                CreatedBy = "test",
                UpdatedBy = "test",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow
            });
        await context.SaveChangesAsync();
        var service = CreateService(context);

        var active = await service.GetActiveAsync("Nom", "Benali");

        Assert.Single(active);
        Assert.Equal(10, active[0].EmployeId);
        Assert.True(active[0].IsActive);
    }

    [Fact]
    public async Task GetHistoryAsync_ShouldReturnAllHistory_AndFilterByPrenom()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        context.Affectation.AddRange(
            new Affectation
            {
                AffectationId = 10,
                ReferenceCode = "AFF-0300-0001",
                ArticleId = 300,
                EmployeId = 10,
                EtatSortie = "Fonctionnel",
                AssignedAt = DateTime.UtcNow.AddDays(-2),
                IsActive = true,
                CreatedBy = "test",
                UpdatedBy = "test",
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                UpdatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new Affectation
            {
                AffectationId = 11,
                ReferenceCode = "AFF-0300-0002",
                ArticleId = 301,
                EmployeId = 11,
                EtatSortie = "Fonctionnel",
                AssignedAt = DateTime.UtcNow.AddDays(-1),
                IsActive = false,
                ReturnedAt = DateTime.UtcNow,
                CreatedBy = "test",
                UpdatedBy = "test",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow
            });
        await context.SaveChangesAsync();
        var service = CreateService(context);

        var history = await service.GetHistoryAsync("Prenom", "Samuel");

        Assert.Single(history);
        Assert.Equal(11, history[0].EmployeId);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnAffectationWithArticleAndEmploye()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        context.Affectation.Add(new Affectation
        {
            AffectationId = 10,
            ReferenceCode = "AFF-0300-0001",
            ArticleId = 300,
            EmployeId = 10,
            EtatSortie = "Fonctionnel",
            AssignedAt = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = "test",
            UpdatedBy = "test",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync();
        var service = CreateService(context);

        var affectation = await service.GetByIdAsync(10);

        Assert.NotNull(affectation);
        Assert.NotNull(affectation!.Article);
        Assert.NotNull(affectation.Employe);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateActiveAffectation_DecreaseStock_AndCreateStockOutputMovement()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = CreateService(context);

        await service.CreateAsync(new AffectationCreateViewModel
        {
            ArticleId = 300,
            EmployeId = 10,
            EtatSortie = "Fonctionnel",
            Notes = "Attribution initiale"
        }, "test-admin", 1);

        var affectation = await context.Affectation.FirstOrDefaultAsync();
        var article = await context.Article.FirstAsync(a => a.ArticleId == 300);
        var movement = await context.MouvementStock.SingleAsync();

        Assert.NotNull(affectation);
        Assert.True(affectation!.IsActive);
        Assert.Equal(10, affectation.EmployeId);
        Assert.Equal(1, article.NombreArticleActuel);
        Assert.Equal(MouvementType.Sortie, movement.TypeMouvement);
        Assert.Equal(1, movement.Quantite);
        Assert.Equal(2, movement.StockAvant);
        Assert.Equal(1, movement.StockApres);
        Assert.Equal(affectation.ReferenceCode, movement.ReferenceExterne);
        Assert.Contains("Nadia Benali", movement.Motif);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenArticleDoesNotExist()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = CreateService(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(new AffectationCreateViewModel
        {
            ArticleId = 999,
            EmployeId = 10,
            EtatSortie = "Fonctionnel"
        }, "test-admin"));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenEmployeIsInactive()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        context.Employe.Add(new Employe
        {
            EmployeId = 20,
            Prenom = "Legacy",
            Nom = "Inactif",
            EmailProfessionnel = "legacy@test.local",
            Departement = "TI",
            Poste = "Ancien",
            IsActive = false
        });
        await context.SaveChangesAsync();
        var service = CreateService(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(new AffectationCreateViewModel
        {
            ArticleId = 300,
            EmployeId = 20,
            EtatSortie = "Fonctionnel"
        }, "test-admin"));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenStockIsUnavailable()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var article = await context.Article.FirstAsync(a => a.ArticleId == 301);
        article.NombreArticleActuel = 0;
        await context.SaveChangesAsync();

        var service = CreateService(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(new AffectationCreateViewModel
        {
            ArticleId = 301,
            EmployeId = 10,
            EtatSortie = "Fonctionnel"
        }, "test-admin"));
    }

    [Fact]
    public async Task GetAssignableEmployesAsync_ShouldFilterByNomPrenomOrId()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = CreateService(context);

        var byNom = await service.GetAssignableEmployesAsync("Nom", "Benali");
        var byPrenom = await service.GetAssignableEmployesAsync("Prenom", "Samuel");
        var byId = await service.GetAssignableEmployesAsync("Id", "12");

        Assert.Single(byNom);
        Assert.Equal(10, byNom[0].EmployeId);
        Assert.Single(byPrenom);
        Assert.Equal(11, byPrenom[0].EmployeId);
        Assert.Single(byId);
        Assert.Equal(12, byId[0].EmployeId);
    }

    [Fact]
    public async Task ReturnAsync_ShouldCloseAffectation_IncreaseStock_AndCreateStockReturnMovement()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        context.Affectation.Add(new Affectation
        {
            AffectationId = 10,
            ReferenceCode = "AFF-0300-0001",
            ArticleId = 300,
            EmployeId = 10,
            EtatSortie = "Fonctionnel",
            AssignedAt = DateTime.UtcNow.AddDays(-1),
            IsActive = true,
            CreatedBy = "test",
            UpdatedBy = "test",
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            UpdatedAt = DateTime.UtcNow.AddDays(-1)
        });
        var article = await context.Article.FirstAsync(a => a.ArticleId == 300);
        article.NombreArticleActuel = 1;
        await context.SaveChangesAsync();

        var service = CreateService(context);

        await service.ReturnAsync(10, new AffectationReturnViewModel
        {
            EtatRetour = "Fonctionnel",
            Notes = "Retour conforme"
        }, "test-admin", 1);

        var affectation = await context.Affectation.FirstAsync(a => a.AffectationId == 10);
        var updatedArticle = await context.Article.FirstAsync(a => a.ArticleId == 300);
        var movement = await context.MouvementStock.SingleAsync();

        Assert.False(affectation.IsActive);
        Assert.NotNull(affectation.ReturnedAt);
        Assert.Equal("Fonctionnel", affectation.EtatRetour);
        Assert.Equal(2, updatedArticle.NombreArticleActuel);
        Assert.Equal(MouvementType.Retour, movement.TypeMouvement);
        Assert.Equal(1, movement.Quantite);
        Assert.Equal(1, movement.StockAvant);
        Assert.Equal(2, movement.StockApres);
        Assert.Equal(affectation.ReferenceCode, movement.ReferenceExterne);
        Assert.Contains("Nadia Benali", movement.Motif);
    }

    [Fact]
    public async Task ReturnAsync_ShouldThrow_WhenAffectationDoesNotExist()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = CreateService(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.ReturnAsync(999, new AffectationReturnViewModel
        {
            EtatRetour = "Fonctionnel"
        }, "test-admin"));
    }

    [Fact]
    public async Task ReturnAsync_ShouldThrow_WhenAffectationAlreadyReturned()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        context.Affectation.Add(new Affectation
        {
            AffectationId = 10,
            ReferenceCode = "AFF-0300-0001",
            ArticleId = 300,
            EmployeId = 10,
            EtatSortie = "Fonctionnel",
            EtatRetour = "Fonctionnel",
            AssignedAt = DateTime.UtcNow.AddDays(-2),
            ReturnedAt = DateTime.UtcNow.AddDays(-1),
            IsActive = false,
            CreatedBy = "test",
            UpdatedBy = "test",
            CreatedAt = DateTime.UtcNow.AddDays(-2),
            UpdatedAt = DateTime.UtcNow.AddDays(-1)
        });
        await context.SaveChangesAsync();
        var service = CreateService(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.ReturnAsync(10, new AffectationReturnViewModel
        {
            EtatRetour = "Fonctionnel"
        }, "test-admin"));
    }
    private static AffectationService CreateService(ApplicationDbContext context)
    {
        var notificationService = new NotificationService(
            context,
            Options.Create(new EmailSettings { Enabled = false }),
            NullLogger<NotificationService>.Instance);

        return new AffectationService(context, notificationService);
    }
}


using EasyStock.Models;
using EasyStock.Service;
using Microsoft.EntityFrameworkCore;

namespace EasyStock.Sprint3.Tests;

public class ArticleServiceTests
{
    [Fact]
    public async Task AddArticle_ShouldPersistArticle()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = new ArticleService(context);

        var article = new Article
        {
            Name = "Laptop T14",
            Description = "Poste portable",
            CategoryId = 100,
            FournisseurId = 200,
            NombreArticleActuel = 10,
            NombreArticleMinimum = 3,
            CreatedBy = "test",
            UpdatedBy = "test",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await service.AddArticle(article);

        var persisted = await context.Article.FirstOrDefaultAsync(a => a.Name == "Laptop T14");
        Assert.NotNull(persisted);
        Assert.Equal(100, persisted!.CategoryId);
        Assert.Equal(200, persisted.FournisseurId);
    }

    [Fact]
    public async Task UpdateArticle_ShouldPersistNewValues()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = new ArticleService(context);

        var article = new Article
        {
            Name = "Switch 24p",
            Description = "Switch initial",
            CategoryId = 101,
            FournisseurId = 201,
            NombreArticleActuel = 4,
            NombreArticleMinimum = 2,
            CreatedBy = "test",
            UpdatedBy = "test",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await service.AddArticle(article);

        article.Description = "Switch coeur reseau";
        article.NombreArticleActuel = 7;
        await service.UpdateArticle(article);

        var persisted = await context.Article.FirstAsync(a => a.ArticleId == article.ArticleId);
        Assert.Equal("Switch coeur reseau", persisted.Description);
        Assert.Equal(7, persisted.NombreArticleActuel);
    }

    [Fact]
    public async Task DeleteArticle_ShouldRemoveEntity()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = new ArticleService(context);

        var article = new Article
        {
            Name = "Dock USB-C",
            Description = "Station d'accueil",
            CategoryId = 100,
            FournisseurId = 200,
            NombreArticleActuel = 8,
            NombreArticleMinimum = 2,
            CreatedBy = "test",
            UpdatedBy = "test",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await service.AddArticle(article);

        await service.DeleteArticle(article);

        var exists = await context.Article.AnyAsync(a => a.ArticleId == article.ArticleId);
        Assert.False(exists);
    }

    [Fact]
    public async Task GetArticleById_ShouldReturnArticleWithRelations()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = new ArticleService(context);

        var article = new Article
        {
            Name = "Ecran 27",
            Description = "Moniteur bureautique",
            CategoryId = 100,
            FournisseurId = 200,
            NombreArticleActuel = 12,
            NombreArticleMinimum = 4,
            CreatedBy = "test",
            UpdatedBy = "test",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await service.AddArticle(article);

        var found = await service.GetArticleById(article.ArticleId);

        Assert.NotNull(found);
        Assert.Equal("Ecran 27", found!.Name);
        Assert.NotNull(found.Category);
        Assert.NotNull(found.Fournisseur);
    }

    [Fact]
    public async Task ArticleExists_ShouldReturnExpectedValue()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = new ArticleService(context);

        var article = new Article
        {
            Name = "AP WiFi",
            Description = "Point d'acces",
            CategoryId = 101,
            FournisseurId = 201,
            NombreArticleActuel = 6,
            NombreArticleMinimum = 2,
            CreatedBy = "test",
            UpdatedBy = "test",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await service.AddArticle(article);

        Assert.True(service.ArticleExists(article.ArticleId));
        Assert.False(service.ArticleExists(99999));
    }

    [Fact]
    public async Task GetListeArticle_ShouldReturnAllArticles()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = new ArticleService(context);

        await service.AddArticle(new Article
        {
            Name = "Casque",
            Description = "Audio",
            CategoryId = 100,
            FournisseurId = 200,
            NombreArticleActuel = 15,
            NombreArticleMinimum = 5,
            CreatedBy = "test",
            UpdatedBy = "test",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        await service.AddArticle(new Article
        {
            Name = "Souris",
            Description = "Peripherique",
            CategoryId = 100,
            FournisseurId = 201,
            NombreArticleActuel = 25,
            NombreArticleMinimum = 10,
            CreatedBy = "test",
            UpdatedBy = "test",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        var list = await service.GetListeArticle();
        Assert.True(list.Count >= 2);
    }

    [Fact]
    public async Task GetCategoriesSelectList_ShouldReturnOrderedCategories()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = new ArticleService(context);

        var selectList = await service.GetCategoriesSelectList();
        var items = selectList.Items.Cast<Category>().ToList();

        Assert.Equal(2, items.Count);
        Assert.Equal("Postes", items[0].Name);
        Assert.Equal("Reseau", items[1].Name);
    }
}

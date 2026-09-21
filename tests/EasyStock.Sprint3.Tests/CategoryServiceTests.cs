using EasyStock.Models;
using EasyStock.Service;
using Microsoft.EntityFrameworkCore;

namespace EasyStock.Sprint3.Tests;

public class CategoryServiceTests
{
    [Fact]
    public async Task AddCategory_ShouldPersistCategory()
    {
        await using var context = TestDbFactory.CreateContext();
        var service = new CategoryService(context);

        var category = new Category
        {
            Name = "Infrastructure",
            CreatedBy = "test",
            UpdatedBy = "test",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await service.Add(category);

        var persisted = await context.Category.FirstOrDefaultAsync(c => c.Name == "Infrastructure");
        Assert.NotNull(persisted);
    }

    [Fact]
    public async Task GetAll_ShouldReturnCategoriesOrdered()
    {
        await using var context = TestDbFactory.CreateContext();
        var service = new CategoryService(context);

        await service.Add(new Category { Name = "Zebra", CreatedBy = "test", UpdatedBy = "test", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
        await service.Add(new Category { Name = "Alpha", CreatedBy = "test", UpdatedBy = "test", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });

        var categories = await service.GetAll();

        Assert.Equal("Alpha", categories[0].Name);
        Assert.Equal("Zebra", categories[1].Name);
    }

    [Fact]
    public async Task UpdateCategory_ShouldPersistNewName()
    {
        await using var context = TestDbFactory.CreateContext();
        var service = new CategoryService(context);

        var category = new Category
        {
            Name = "Reseau",
            CreatedBy = "test",
            UpdatedBy = "test",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await service.Add(category);

        category.Name = "Reseau coeur";
        await service.Update(category);

        var persisted = await context.Category.FirstAsync(c => c.CategoryId == category.CategoryId);
        Assert.Equal("Reseau coeur", persisted.Name);
    }

    [Fact]
    public async Task DeleteCategory_ShouldRemoveEntity()
    {
        await using var context = TestDbFactory.CreateContext();
        var service = new CategoryService(context);

        var category = new Category
        {
            Name = "Peripheriques",
            CreatedBy = "test",
            UpdatedBy = "test",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await service.Add(category);

        await service.Delete(category);

        var exists = await context.Category.AnyAsync(c => c.CategoryId == category.CategoryId);
        Assert.False(exists);
    }

    [Fact]
    public async Task GetById_ShouldReturnCategory()
    {
        await using var context = TestDbFactory.CreateContext();
        var service = new CategoryService(context);

        var category = new Category
        {
            Name = "Securite",
            CreatedBy = "test",
            UpdatedBy = "test",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await service.Add(category);

        var found = await service.GetById(category.CategoryId);

        Assert.NotNull(found);
        Assert.Equal("Securite", found!.Name);
    }

    [Fact]
    public void Exists_ShouldReturnExpectedValue()
    {
        using var context = TestDbFactory.CreateContext();
        var service = new CategoryService(context);

        var category = new Category
        {
            Name = "Impression",
            CreatedBy = "test",
            UpdatedBy = "test",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        context.Category.Add(category);
        context.SaveChanges();

        Assert.True(service.Exists(category.CategoryId));
        Assert.False(service.Exists(99999));
    }
}

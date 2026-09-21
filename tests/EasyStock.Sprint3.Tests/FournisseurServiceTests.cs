using EasyStock.Models;
using EasyStock.Service;
using Microsoft.EntityFrameworkCore;

namespace EasyStock.Sprint3.Tests;

public class FournisseurServiceTests
{
    [Fact]
    public async Task AddFournisseur_ShouldPersistFournisseur()
    {
        await using var context = TestDbFactory.CreateContext();
        var service = new FournisseurService(context);

        var fournisseur = new Fournisseur
        {
            Nom = "SoftChoice",
            ContactEmail = "contact@softchoice.ca",
            PhoneNumber = "514-555-0101",
            Address = "Montreal",
            CreatedBy = "test",
            UpdatedBy = "test",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await service.AddFournissuer(fournisseur);

        var persisted = await context.Fournisseur.FirstOrDefaultAsync(f => f.Nom == "SoftChoice");
        Assert.NotNull(persisted);
        Assert.Equal("Montreal", persisted!.Address);
    }

    [Fact]
    public async Task GetListeFournissuer_ShouldReturnFournisseursOrdered()
    {
        await using var context = TestDbFactory.CreateContext();
        var service = new FournisseurService(context);

        await service.AddFournissuer(new Fournisseur { Nom = "Zeta", ContactEmail = "zeta@test.ca", PhoneNumber = "1", Address = "Montreal", CreatedBy = "test", UpdatedBy = "test", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
        await service.AddFournissuer(new Fournisseur { Nom = "Alpha", ContactEmail = "alpha@test.ca", PhoneNumber = "2", Address = "Laval", CreatedBy = "test", UpdatedBy = "test", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });

        var fournisseurs = await service.GetListeFournissuer();

        Assert.Equal("Alpha", fournisseurs[0].Nom);
        Assert.Equal("Zeta", fournisseurs[1].Nom);
    }

    [Fact]
    public async Task GetFournissuersForArticle_ShouldReturnSelectList()
    {
        await using var context = TestDbFactory.CreateContext();
        var service = new FournisseurService(context);

        await service.AddFournissuer(new Fournisseur { Nom = "Beta", ContactEmail = "beta@test.ca", PhoneNumber = "1", Address = "Montreal", CreatedBy = "test", UpdatedBy = "test", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });

        var selectList = await service.GetFournissuersForArticle();
        var items = selectList.Items.Cast<Fournisseur>().ToList();

        Assert.Single(items);
        Assert.Equal("Beta", items[0].Nom);
    }

    [Fact]
    public async Task UpdateFournisseur_ShouldPersistNewValues()
    {
        await using var context = TestDbFactory.CreateContext();
        var service = new FournisseurService(context);

        var fournisseur = new Fournisseur
        {
            Nom = "CDW",
            ContactEmail = "cdw@old.ca",
            PhoneNumber = "514-555-0001",
            Address = "Laval",
            CreatedBy = "test",
            UpdatedBy = "test",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await service.AddFournissuer(fournisseur);

        fournisseur.ContactEmail = "cdw@new.ca";
        fournisseur.Address = "Montreal";
        await service.UpdateFournissuer(fournisseur);

        var persisted = await context.Fournisseur.FirstAsync(f => f.FournisseurId == fournisseur.FournisseurId);
        Assert.Equal("cdw@new.ca", persisted.ContactEmail);
        Assert.Equal("Montreal", persisted.Address);
    }

    [Fact]
    public async Task DeleteFournisseur_ShouldRemoveEntity()
    {
        await using var context = TestDbFactory.CreateContext();
        var service = new FournisseurService(context);

        var fournisseur = new Fournisseur
        {
            Nom = "Ingram",
            ContactEmail = "ingram@test.ca",
            PhoneNumber = "514-555-0002",
            Address = "Quebec",
            CreatedBy = "test",
            UpdatedBy = "test",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await service.AddFournissuer(fournisseur);

        await service.DeleteFournissuer(fournisseur);

        var exists = await context.Fournisseur.AnyAsync(f => f.FournisseurId == fournisseur.FournisseurId);
        Assert.False(exists);
    }

    [Fact]
    public async Task GetFournisseurById_ShouldReturnExpectedEntity()
    {
        await using var context = TestDbFactory.CreateContext();
        var service = new FournisseurService(context);

        var fournisseur = new Fournisseur
        {
            Nom = "Dell",
            ContactEmail = "dell@test.ca",
            PhoneNumber = "514-555-0003",
            Address = "Toronto",
            CreatedBy = "test",
            UpdatedBy = "test",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await service.AddFournissuer(fournisseur);

        var found = await service.GetFournissuerById(fournisseur.FournisseurId);

        Assert.NotNull(found);
        Assert.Equal("Dell", found!.Nom);
    }

    [Fact]
    public void FournisseurExists_ShouldReturnExpectedValue()
    {
        using var context = TestDbFactory.CreateContext();
        var service = new FournisseurService(context);

        var fournisseur = new Fournisseur
        {
            Nom = "HP",
            ContactEmail = "hp@test.ca",
            PhoneNumber = "514-555-0004",
            Address = "Montreal",
            CreatedBy = "test",
            UpdatedBy = "test",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        context.Fournisseur.Add(fournisseur);
        context.SaveChanges();

        Assert.True(service.FournissuerExists(fournisseur.FournisseurId));
        Assert.False(service.FournissuerExists(99999));
    }
}

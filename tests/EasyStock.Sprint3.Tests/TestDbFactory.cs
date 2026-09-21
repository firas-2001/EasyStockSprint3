using EasyStock.Data;
using EasyStock.Models;
using EasyStock.Service;
using Microsoft.EntityFrameworkCore;

namespace EasyStock.Sprint3.Tests;

internal static class TestDbFactory
{
    public static ApplicationDbContext CreateContext(string? dbName = null)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: dbName ?? Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    public static async Task SeedBaseDataAsync(ApplicationDbContext context)
    {
        if (!await context.Role.AnyAsync())
        {
            context.Role.AddRange(
                new Role { RoleId = 1, RoleName = "Admin", Description = "Accès complet" },
                new Role { RoleId = 2, RoleName = "Gestionnaire", Description = "Gestion opérationnelle" },
                new Role { RoleId = 3, RoleName = "Operateur", Description = "Exploitation" }
            );
        }

        var authService = new AuthService(context);

        if (!await context.User.AnyAsync())
        {
            context.User.AddRange(
                new User
                {
                    UserId = 1,
                    Username = "admin-test",
                    Email = "admin@test.local",
                    PwdHash = authService.HashPassword("Admin123!"),
                    RoleId = 1,
                    CreatedBy = "test",
                    UpdatedBy = "test",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    UserId = 2,
                    Username = "operateur-test",
                    Email = "operateur@test.local",
                    PwdHash = authService.HashPassword("Operateur123!"),
                    RoleId = 3,
                    CreatedBy = "test",
                    UpdatedBy = "test",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }

        if (!await context.Employe.AnyAsync())
        {
            context.Employe.AddRange(
                new Employe { EmployeId = 10, Prenom = "Nadia", Nom = "Benali", EmailProfessionnel = "nadia.benali@test.local", Departement = "RH", Poste = "Conseillere", IsActive = true },
                new Employe { EmployeId = 11, Prenom = "Samuel", Nom = "Tremblay", EmailProfessionnel = "samuel.tremblay@test.local", Departement = "TI", Poste = "Technicien", IsActive = true },
                new Employe { EmployeId = 12, Prenom = "Karim", Nom = "El Fassi", EmailProfessionnel = "karim.elfassi@test.local", Departement = "Logistique", Poste = "Coordonnateur", IsActive = true }
            );
        }

        if (!await context.Category.AnyAsync())
        {
            context.Category.AddRange(
                new Category { CategoryId = 100, Name = "Postes", CreatedBy = "test", UpdatedBy = "test", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Category { CategoryId = 101, Name = "Reseau", CreatedBy = "test", UpdatedBy = "test", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            );
        }

        if (!await context.Fournisseur.AnyAsync())
        {
            context.Fournisseur.AddRange(
                new Fournisseur { FournisseurId = 200, Nom = "CDW", ContactEmail = "cdw@test.ca", PhoneNumber = "514-555-0001", Address = "Montreal", CreatedBy = "test", UpdatedBy = "test", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Fournisseur { FournisseurId = 201, Nom = "Ingram", ContactEmail = "ingram@test.ca", PhoneNumber = "514-555-0002", Address = "Laval", CreatedBy = "test", UpdatedBy = "test", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            );
        }

        if (!await context.Article.AnyAsync())
        {
            context.Article.AddRange(
                new Article { ArticleId = 300, Name = "Portable T14", Description = "Portable de test", CategoryId = 100, FournisseurId = 200, NombreArticleActuel = 2, NombreArticleMinimum = 1, CreatedBy = "test", UpdatedBy = "test", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Article { ArticleId = 301, Name = "Station accueil", Description = "Dock de test", CategoryId = 100, FournisseurId = 201, NombreArticleActuel = 1, NombreArticleMinimum = 0, CreatedBy = "test", UpdatedBy = "test", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            );
        }

        await context.SaveChangesAsync();
    }
}

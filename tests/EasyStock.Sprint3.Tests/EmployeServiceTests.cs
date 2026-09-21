using EasyStock.Service;

namespace EasyStock.Sprint3.Tests;

public class EmployeServiceTests
{
    [Fact]
    public async Task GetActifsAsync_ShouldFilterByNomPrenomOrId()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = new EmployeService(context);

        var byNom = await service.GetActifsAsync("Nom", "Benali");
        var byPrenom = await service.GetActifsAsync("Prenom", "Samuel");
        var byId = await service.GetActifsAsync("Id", "12");

        Assert.Single(byNom);
        Assert.Single(byPrenom);
        Assert.Single(byId);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnExpectedEmploye()
    {
        await using var context = TestDbFactory.CreateContext();
        await TestDbFactory.SeedBaseDataAsync(context);
        var service = new EmployeService(context);

        var employe = await service.GetByIdAsync(10);

        Assert.NotNull(employe);
        Assert.Equal("Nadia", employe!.Prenom);
    }
}

using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Menu;

namespace Tests.CabDeliveryCompanyKata.Adapter.Console;

public class FakeRepositoryTest
{
    [Fact]
    public void Repository_CanCreateAnItem()
    {
        var menuRepository = new FakeDatabaseRepository<Menu>();

        menuRepository.Create(new Menu()
        {
            Cabs = new List<CabDto>(), Customers = new List<CustomerDto>()
        });

        var displayMenu = menuRepository.Read();

        Assert.Single(displayMenu);
        Assert.Equal(1, displayMenu.First().Id);
        Assert.Equal(0, displayMenu.First().Cabs.Count);
        Assert.Equal(0, displayMenu.First().Customers.Count);
    }

    [Fact]
    public void Repository_CanAddTwoItems()
    {
        var menuRepository = new FakeDatabaseRepository<Menu>();
        menuRepository.Create(new Menu()
        {
            Cabs = new List<CabDto>(), Customers = new List<CustomerDto>()
        });
        menuRepository.Create(new Menu()
        {
            Cabs = new List<CabDto>(), Customers = new List<CustomerDto>()
        });

        var displayMenu = menuRepository.Read();

        Assert.Equal(2, displayMenu.Count);
        Assert.Equal(1, displayMenu.First().Id);
        Assert.Equal(0, displayMenu.First().Cabs.Count);
        Assert.Equal(0, displayMenu.First().Customers.Count);
    }

    [Fact]
    public void Repository_CanUpdateAnItem()
    {
        var menuRepository = new FakeDatabaseRepository<Menu>();
        menuRepository.Create(new Menu()
        {
            Cabs = new List<CabDto>(), Customers = new List<CustomerDto>()
        });
        menuRepository.Create(new Menu()
        {
            Cabs = new List<CabDto>(), Customers = new List<CustomerDto>()
        });

        menuRepository.Update(2, new Menu()
        {
            Cabs = new List<CabDto>()
            {
                new("", 0, 0.0, 0.0)
            },
            Customers = new List<CustomerDto>()
            {
                new() { Id = 1, Name = "hello world"}
            }
        });

        var displayMenu = menuRepository.Read();

        Assert.Equal(2, displayMenu.Count);
        Assert.Equal(2, displayMenu.Find(x => x.Id == 2).Id);
        Assert.Equal(1, displayMenu.Find(x => x.Id == 2).Cabs.Count);
        Assert.Equal(1, displayMenu.Find(x => x.Id == 2).Customers.Count);
    }

    [Fact]
    public void Repository_CanDeleteAnItem()
    {
        var menuRepository = new FakeDatabaseRepository<Menu>();
        menuRepository.Create(new Menu()
        {
            Cabs = new List<CabDto>(), Customers = new List<CustomerDto>()
        });
        menuRepository.Create(new Menu()
        {
            Cabs = new List<CabDto>(), Customers = new List<CustomerDto>()
        });
        menuRepository.Create(new Menu()
        {
            Cabs = new List<CabDto>(), Customers = new List<CustomerDto>()
        });
        menuRepository.Delete(2);

        var displayMenu = menuRepository.Read();

        Assert.Equal(2, displayMenu.Count);
        Assert.Equal(1, displayMenu.First().Id);
        Assert.Equal(0, displayMenu.First().Cabs.Count);
        Assert.Equal(0, displayMenu.First().Customers.Count);
        Assert.Equal(3, displayMenu.Skip(1).First().Id);
        Assert.Equal(0, displayMenu.Skip(1).First().Cabs.Count);
        Assert.Equal(0, displayMenu.Skip(1).First().Customers.Count);
    }
    [Fact]
    public void Repository_CanSearchUsingFilters()
    {
        var menuRepository = new FakeDatabaseRepository<Menu>();
        menuRepository.Create(new Menu()
        {
            Cabs = new List<CabDto>(), Customers = new List<CustomerDto>()
        });
        menuRepository.Create(new Menu()
        {
            Cabs = new List<CabDto>(), Customers = new List<CustomerDto>()
        });
        menuRepository.Create(new Menu()
        {
            Cabs = new List<CabDto>(), Customers = new List<CustomerDto>()
        });
        menuRepository.Delete(2);

        var displayMenu = menuRepository
            .Search(x => x.Id == 3);

        Assert.Equal(1, displayMenu.Count);
        Assert.Equal(3, displayMenu.First().Id);
        Assert.Equal(0, displayMenu.First().Cabs.Count);
        Assert.Equal(0, displayMenu.First().Customers.Count);
    }
}
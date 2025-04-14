using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;

namespace Tests.CabDeliveryCompanyKata.Adapter.Console;

public class FakeRepositoryTest_MenuRepository
{
    // [Fact]
    // public void Repository_CanCreateAnItem()
    // {
    //     var menuRepository = new FakeDatabaseRepository<Menu>();
    //
    //     menuRepository.Create(new Menu()
    //     {
    //         Fleets = [], Customer = []
    //     });
    //
    //     var displayMenu = menuRepository.Read();
    //
    //     Assert.Single(displayMenu);
    //     Assert.Equal(1, displayMenu.First().Id);
    //     Assert.Equal(0, displayMenu.First().Fleets.Count);
    //     Assert.Equal(0, displayMenu.First().Customer.Count);
    // }
    //
    // [Fact]
    // public void Repository_CanAddTwoItems()
    // {
    //     var menuRepository = new FakeDatabaseRepository<Menu>();
    //     menuRepository.Create(new Menu()
    //     {
    //         Fleets = [], Customer = []
    //     });
    //     menuRepository.Create(new Menu()
    //     {
    //         Fleets = [], Customer = []
    //     });
    //
    //     var displayMenu = menuRepository.Read();
    //
    //     Assert.Equal(2, displayMenu.Count);
    //     Assert.Equal(1, displayMenu.First().Id);
    //     Assert.Equal(0, displayMenu.First().Fleets.Count);
    //     Assert.Equal(0, displayMenu.First().Customer.Count);
    // }
    //
    // [Fact]
    // public void Repository_CanUpdateAnItem()
    // {
    //     var menuRepository = new FakeDatabaseRepository<Menu>();
    //     menuRepository.Create(new Menu()
    //     {
    //         Fleets = new List<Fleet>(), Customer = new List<FleetCoordinator>()
    //     });
    //     menuRepository.Create(new Menu()
    //     {
    //         Fleets = new List<Fleet>(), Customer = new List<FleetCoordinator>()
    //     });
    //
    //     menuRepository.Update(2, new Menu()
    //     {
    //         Fleets = [new() { Cabs = [new("", 0, 0.0, 0.0)] }],
    //         Customer = [new() { Patrons = [new PatronDto() { Id = 1, Name = "Hello World" }] }]
    //     });
    //
    //     var displayMenu = menuRepository.Read();
    //
    //     Assert.Equal(2, displayMenu.Count);
    //     Assert.Equal(2, displayMenu.Find(x => x.Id == 2).Id);
    //     Assert.Equal(1, displayMenu.Find(x => x.Id == 2).Fleets.Count);
    //     Assert.Equal(1, displayMenu.Find(x => x.Id == 2).Customer.Count);
    // }
    //
    // [Fact]
    // public void Repository_CanDeleteAnItem()
    // {
    //     var menuRepository = new FakeDatabaseRepository<Menu>();
    //     menuRepository.Create(new Menu()
    //     {
    //         Fleets = new List<Fleet>(), Customer = new List<FleetCoordinator>()
    //     });
    //     menuRepository.Create(new Menu()
    //     {
    //         Fleets = new List<Fleet>(), Customer = new List<FleetCoordinator>()
    //     });
    //     menuRepository.Create(new Menu()
    //     {
    //         Fleets = new List<Fleet>(), Customer = new List<FleetCoordinator>()
    //     });
    //     menuRepository.Delete(2);
    //
    //     var displayMenu = menuRepository.Read();
    //
    //     Assert.Equal(2, displayMenu.Count);
    //     Assert.Equal(1, displayMenu.First().Id);
    //     Assert.Equal(0, displayMenu.First().Fleets.Count);
    //     Assert.Equal(0, displayMenu.First().Customer.Count);
    //     Assert.Equal(3, displayMenu.Skip(1).First().Id);
    //     Assert.Equal(0, displayMenu.Skip(1).First().Fleets.Count);
    //     Assert.Equal(0, displayMenu.Skip(1).First().Customer.Count);
    // }
    // [Fact]
    // public void Repository_CanSearchUsingFilters()
    // {
    //     var menuRepository = new FakeDatabaseRepository<Menu>();
    //     menuRepository.Create(new Menu()
    //     {
    //         Fleets = new List<Fleet>(), Customer = new List<FleetCoordinator>()
    //     });
    //     menuRepository.Create(new Menu()
    //     {
    //         Fleets = new List<Fleet>(), Customer = new List<FleetCoordinator>()
    //     });
    //     menuRepository.Create(new Menu()
    //     {
    //         Fleets = new List<Fleet>(), Customer = new List<FleetCoordinator>()
    //     });
    //     menuRepository.Delete(2);
    //
    //     var displayMenu = menuRepository
    //         .Search(x => x.Id == 3);
    //
    //     Assert.Equal(1, displayMenu.Count);
    //     Assert.Equal(3, displayMenu.First().Id);
    //     Assert.Equal(0, displayMenu.First().Fleets.Count);
    //     Assert.Equal(0, displayMenu.First().Customer.Count);
    // }
}
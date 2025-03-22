using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Tests.CabDeliveryCompanyKata.Adapter.Console;

public class FakeRepositoryTestNonGenericCustomerListRepository
{
    [Fact(Skip = "Not sure what I was up to with this one")]
    public void Repository_CanCreateAnItem()
    {
        var customerListRepository = new FakeCustomerListDatabaseRepository();

        customerListRepository.Save(new CustomerList()
        {
            Customers = new List<Customer>()
        });

        var customerListDto = customerListRepository.Read();

        Assert.NotNull(customerListDto);
        Assert.Equal(1, customerListDto.Id);
        Assert.Equal(0, customerListDto.Customers.Count);
    }
    [Fact]
    public void Repository_CanCreateAnItemWithDetails()
    {
        var customerListRepository = new FakeDatabaseRepository<FleetCoordinator>();
        var firstCustomer = new PatronDto()
        {
            Id = 7,
            Name = "Hello World",
            Status = CustomerStatus.Delivered,
            CustomerId = 7,
            EndLocation = "2 Destination Lane",
            MenuId = 1,
            StartLocation = "1 Starting Drive"
        };
        customerListRepository.Create(new FleetCoordinator()
        {
            Patrons =
            [
                firstCustomer
            ]
        });

        var customerListDto = customerListRepository.Read();

        Assert.Single(customerListDto);
        Assert.Equal(1, customerListDto.First().Id);
        Assert.Equal(1, customerListDto.First().Patrons.Count);
        Assert.Equivalent(firstCustomer, customerListDto.First().Patrons.First());
    }

    [Fact]
    public void Repository_CanAddTwoItems()
    {
        var customerListRepository = new FakeDatabaseRepository<FleetCoordinator>();
        customerListRepository.Create(new FleetCoordinator()
        {
            Patrons = new List<PatronDto>()
        });
        customerListRepository.Create(new FleetCoordinator()
        {
            Patrons = new List<PatronDto>()
        });

        var customerListDto = customerListRepository.Read();

        Assert.Equal(2, customerListDto.Count);
        Assert.Equal(1, customerListDto.First().Id);
        Assert.Equal(0, customerListDto.First().Patrons.Count);
    }

    [Fact]
    public void Repository_CanUpdateAnItem()
    {
        var customerListRepository = new FakeDatabaseRepository<FleetCoordinator>();
        customerListRepository.Create(new FleetCoordinator()
        {
            Patrons = new List<PatronDto>()
        });
        customerListRepository.Create(new FleetCoordinator()
        {
            Patrons = new List<PatronDto>()
        });

        customerListRepository.Update(2, new FleetCoordinator()
        {
            Patrons = new List<PatronDto>()
            {
                new() { Id = 1, Name = "hello world"}
            }
        });

        var customerListDto = customerListRepository.Read();

        Assert.Equal(2, customerListDto.Count);
        Assert.Equal(2, customerListDto.Find(x => x.Id == 2).Id);
        Assert.Equal(1, customerListDto.Find(x => x.Id == 2).Patrons.Count);
    }

    [Fact]
    public void Repository_CanDeleteAnItem()
    {
        var customerListRepository = new FakeDatabaseRepository<FleetCoordinator>();
        customerListRepository.Create(new FleetCoordinator()
        {
            Patrons = new List<PatronDto>()
        });
        customerListRepository.Create(new FleetCoordinator()
        {
            Patrons = new List<PatronDto>()
        });
        customerListRepository.Create(new FleetCoordinator()
        {
            Patrons = new List<PatronDto>()
        });
        customerListRepository.Delete(2);

        var customerListDto = customerListRepository.Read();

        Assert.Equal(2, customerListDto.Count);
        Assert.Equal(1, customerListDto.First().Id);
        Assert.Equal(0, customerListDto.First().Patrons.Count);
        Assert.Equal(3, customerListDto.Skip(1).First().Id);
        Assert.Equal(0, customerListDto.Skip(1).First().Patrons.Count);
    }
    [Fact]
    public void Repository_CanSearchUsingFilters()
    {
        var customerListRepository = new FakeDatabaseRepository<FleetCoordinator>();
        customerListRepository.Create(new FleetCoordinator()
        {
            Patrons = new List<PatronDto>()
        });
        customerListRepository.Create(new FleetCoordinator()
        {
            Patrons = new List<PatronDto>()
        });
        customerListRepository.Create(new FleetCoordinator()
        {
            Patrons = new List<PatronDto>()
        });
        customerListRepository.Delete(2);

        var customerListDto = customerListRepository
            .Search(x => x.Id == 3);

        Assert.Equal(1, customerListDto.Count);
        Assert.Equal(3, customerListDto.First().Id);
        Assert.Equal(0, customerListDto.First().Patrons.Count);
    }
}
using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Adapter.@in.ConsoleAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
using Production.EmmaCabCompany.Application;
using Production.EmmaCabCompany.Application.CustomerList.Commands.Command;
using Production.EmmaCabCompany.Application.Fleet;
using Production.EmmaCabCompany.Domain;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Tests.CabDeliveryCompanyKata;

public class ApplicationHandlerTests
{
    private readonly CabContext _cabContext;

    public ApplicationHandlerTests()
    {
        _cabContext = new CabContext(
            new DbContextOptionsBuilder<CabContext>()
                .UseSqlite($"Data Source={Guid.NewGuid()}.db").Options);
        _cabContext.Database.Migrate();
    }

    [Fact]
    public void CanRegisterAsNewUser()
    {
        var dispatchController = new ApplicationHandler(_cabContext);

        dispatchController.Handle(new NewUserRegistered());

        Assert.NotNull(_cabContext.Customers.FirstOrDefault());
    }

    [Fact]
    public void CanLoginAsNewUser()
    {
        var dispatchController = new ApplicationHandler(_cabContext);
        dispatchController.Handle(new NewUserRegistered());

        var customer = dispatchController.Handle(new UserLogin() { Id = 1 });

        Assert.Equal(1, customer.Id);
    }

    [Fact]
    public void CanAddNewCab()
    {
        var dispatchController = new ApplicationHandler(_cabContext);
        dispatchController.Handle(new NewUserRegistered());
        var customer = dispatchController.Handle(new UserLogin() { Id = 1 });

        dispatchController.Handle(new AddCabCommand("Evan", 0.00, 0.01, customer.Id));

        Assert.NotNull(_cabContext.CabDrivers.FirstOrDefault());
    }

    [Fact]
    public void PatronCanRequestCab()
    {
        var dispatchController = new ApplicationHandler(_cabContext);
        dispatchController.Handle(new NewUserRegistered());
        var customer = dispatchController.Handle(new UserLogin() { Id = 1 });
        dispatchController.Handle(new AddCabCommand("Evan", 0.00, 0.01, customer.Id));

        dispatchController.Handle(new CustomerCabRequested(
            "Dan", "1 Fulton Drive", "1 Destination Lane",
            customer.Id ?? 0));

        Assert.NotNull(_cabContext.CabDrivers.FirstOrDefault());
        Assert.Equal(PatronStatus.CustomerCallInProgress, _cabContext.Patrons.FirstOrDefault().Status);
    }
    [Fact]
    public void DispatchCanRequestRide()
    {
        var dispatchController = new ApplicationHandler(_cabContext);
        dispatchController.Handle(new NewUserRegistered());
        var customer = dispatchController.Handle(new UserLogin() { Id = 1 });
        dispatchController.Handle(new AddCabCommand("Evan", 0.00, 0.01, customer.Id));
        dispatchController.Handle(new CustomerCabRequested(
            "Dan", "1 Fulton Drive", "1 Destination Lane",
            customer.Id ?? 0));

        dispatchController.Handle(new CustomerRideRequested() { CustomerListId = 1 });

        Assert.Equal(CabStatus.TransportingCustomer, _cabContext.CabDrivers.FirstOrDefault()._status);
        Assert.Equal(PatronStatus.WaitingPickup, _cabContext.Patrons.FirstOrDefault().Status);
    }
    [Fact]
    public void CabCanPickupPatron()
    {
        var dispatchController = new ApplicationHandler(_cabContext);
        dispatchController.Handle(new NewUserRegistered());
        var customer = dispatchController.Handle(new UserLogin() { Id = 1 });
        dispatchController.Handle(new AddCabCommand("Evan", 0.00, 0.01, customer.Id));
        dispatchController.Handle(new CustomerCabRequested(
            "Dan", "1 Fulton Drive", "1 Destination Lane",
            customer.Id ?? 0));
        dispatchController.Handle(new CustomerRideRequested() { CustomerListId = 1 });

        dispatchController.Handle(new CustomerPickedUp() { CustomerListId = 1 });

        Assert.Equal(CabStatus.TransportingCustomer, _cabContext.CabDrivers.FirstOrDefault()._status);
        Assert.Equal(PatronStatus.Enroute, _cabContext.Patrons.FirstOrDefault().Status);
    }
    [Fact]
    public void CabCanDropoffPatron()
    {
        var dispatchController = new ApplicationHandler(_cabContext);
        dispatchController.Handle(new NewUserRegistered());
        var customer = dispatchController.Handle(new UserLogin() { Id = 1 });
        dispatchController.Handle(new AddCabCommand("Evan", 0.00, 0.01, customer.Id));
        dispatchController.Handle(new CustomerCabRequested(
            "Dan", "1 Fulton Drive", "1 Destination Lane",
            customer.Id ?? 0));
        dispatchController.Handle(new CustomerRideRequested() { CustomerListId = 1 });
        dispatchController.Handle(new CustomerPickedUp() { CustomerListId = 1 });

        dispatchController.Handle(new CustomerDelivered() { CustomerListId = 1 });

        Assert.Equal(CabStatus.Available, _cabContext.CabDrivers.FirstOrDefault()._status);
        Assert.Equal(PatronStatus.Delivered, _cabContext.Patrons.FirstOrDefault().Status);
    }
}
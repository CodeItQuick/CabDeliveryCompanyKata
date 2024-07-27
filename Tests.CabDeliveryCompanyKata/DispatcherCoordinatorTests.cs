using Production.EmmaCabCompany;
using Production.EmmaCabCompany.Domain;

namespace Tests.CabDeliveryCompanyKata;

public class DispatcherCoordinatorTests
{
    [Fact]
    public void CanPickupCustomer()
    {
        var cabs = new Cab("Diane's Cab", 20, 46.2382, 63.1311);
        var dispatch = new DispatcherCoordinator();
        var customer = new Customer(
            "Emma", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        dispatch.AddCab(cabs);
        dispatch.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane");
        dispatch.RideRequest();

        dispatch.PickupCustomer();
        Assert.Equal(customer.Name, dispatch.FindEnroutePassenger(CustomerStatus.Enroute)?.PassengerName);
        dispatch.DropOffCustomer();

        Assert.Null(dispatch.FindEnroutePassenger(CustomerStatus.Enroute)?.PassengerName);
    }
    [Fact]
    public void CanPickupTwoCustomersWithTwoCabs()
    {
        var cabs = new Cab("Dan's Cab", 20, 46.2382, 63.1311);
        var cabTwo = new Cab("Evan's Cab", 20, 46.2382, 63.1311);
        var dispatch = new DispatcherCoordinator();
        dispatch.AddCab(cabs);
        dispatch.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane");
        dispatch.RideRequest();
        dispatch.PickupCustomer();
        dispatch.AddCab(cabTwo);
        dispatch.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane");
        dispatch.RideRequest();
        dispatch.PickupCustomer();

        dispatch.DropOffCustomer();
        dispatch.DropOffCustomer();

        Assert.Null(dispatch.FindEnroutePassenger(CustomerStatus.Enroute));
        Assert.Null(dispatch.FindEnroutePassenger(CustomerStatus.Enroute));
    }
    [Fact]
    public void CanPickupTwoCustomersWithTwoCabsSecondOrdering()
    {
        var cabs = new Cab("Dan's Cab", 20, 46.2382, 63.1311);
        var cabTwo = new Cab("Evan's Cab", 20, 46.2382, 63.1311);
        var dispatch = new DispatcherCoordinator();
        dispatch.AddCab(cabs);
        dispatch.AddCab(cabTwo);
        dispatch.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane");
        dispatch.RideRequest();
        dispatch.CustomerCabCall("Lisa", "1 Fulton Drive", "1 Destination Lane");
        dispatch.RideRequest();
        dispatch.PickupCustomer();
        dispatch.PickupCustomer();
        Assert.NotNull(dispatch.FindEnroutePassenger(CustomerStatus.Enroute));
        Assert.NotNull(dispatch.FindEnroutePassenger(CustomerStatus.Enroute));

        dispatch.DropOffCustomer();
        Assert.Equal("Emma", dispatch.RetrieveCustomerInState(CustomerStatus.Delivered)?.Name);
        dispatch.DropOffCustomer();

        Assert.Equal("Lisa", dispatch.RetrieveCustomerInState(CustomerStatus.Delivered)?.Name);
    }
    [Fact]
    public void CannotPickupCustomerIfNotAvailable()
    {
        var cabs = new Cab("Dan's Cab", 20, 46.2382, 63.1311);
        var dispatch = new DispatcherCoordinator();
        var customerTwo = new Customer(
            "Lisa", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        dispatch.AddCab(cabs);
        dispatch.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane");
        var rideRequest = cabs.RequestRideFor(customerTwo);

        Assert.Throws<SystemException>(() => dispatch.PickupCustomer());

        Assert.True(rideRequest); // TODO: this should be false, there is no customer call
    }
    [Fact]
    public void CannotPickupCustomerIfNoCabs()
    {
        var dispatch = new DispatcherCoordinator();

        Assert.Throws<SystemException>(() => dispatch.RideRequest());
        Assert.Throws<SystemException>(() => dispatch.PickupCustomer());
        Assert.Throws<SystemException>(() => dispatch.DropOffCustomer());
    }
    [Fact]
    public void CabNotRequestedByDispatcherAllCallsFail()
    {
        var cabs = new Cab("Diane's Cab", 20, 46.2382, 63.1311);
        var dispatch = new DispatcherCoordinator();
        dispatch.AddCab(cabs);
        dispatch.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane");
        Assert.Throws<SystemException>(() => dispatch.PickupCustomer());
        Assert.Null(dispatch.FindEnroutePassenger(CustomerStatus.Enroute));
        Assert.Throws<SystemException>(() => dispatch.DropOffCustomer());

        Assert.Null(dispatch.FindEnroutePassenger(CustomerStatus.Enroute));
    }
    [Fact]
    public void CustomerNotPickedUpCannotDropOff()
    {
        var cabs = new Cab("Diane's Cab", 20, 46.2382, 63.1311);
        var dispatch = new DispatcherCoordinator();
        dispatch.AddCab(cabs);
        dispatch.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane");
        dispatch.RideRequest();
        Assert.NotNull(dispatch.FindEnroutePassenger(CustomerStatus.WaitingPickup));
        Assert.Throws<SystemException>(() => dispatch.DropOffCustomer());

        Assert.NotNull(dispatch.FindEnroutePassenger(CustomerStatus.WaitingPickup));
    }
    [Fact]
    public void CannotPickupTwoCustomerFares()
    {
        var dispatch = new DispatcherCoordinator();
        var cabs = new Cab("Diane's Cab", 20, 46.2382, 63.1311);
        dispatch.AddCab(cabs);
        dispatch.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane");
        dispatch.CustomerCabCall("Lisa", "1 Fulton Drive", "1 Destination Lane");
        dispatch.RideRequest();
        Assert.Throws<SystemException>(() => dispatch.RideRequest());
        
        Assert.True(dispatch.CustomerInState(CustomerStatus.CustomerCallInProgress));
    }

    [Fact]
    public void CanExportCustomerListToFile()
    {
        var dispatch = new DispatcherCoordinator();

        var exportCustomerList = dispatch.ExportCustomerList();
        
        Assert.Equal(0, exportCustomerList.Count);
    }
    [Fact]
    public void CanExportCustomerCallInProgressListToFile()
    {
        var dispatch = new DispatcherCoordinator();

        var cabs = new Cab("Diane's Cab", 20, 46.2382, 63.1311);
        dispatch.AddCab(cabs);
        dispatch.CustomerCabCall("Emma", "1 Fulton Drive", "1 Destination Lane");
        
        var exportCustomerList = dispatch.ExportCustomerList();
        
        Assert.Equal(1, exportCustomerList.Count);
    }
}
using Production.EmmaCabCompany;
using Production.EmmaCabCompany.Domain;

namespace Tests.CabDeliveryCompanyKata;

public class RadioFleetTests
{
    [Fact]
    public void CanPickupCustomer()
    {
        var cabs = new Cab("Diane's Cab", 20);
        var dispatch = new RadioFleet();
        var customer = new Customer(
            "Emma", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        dispatch.AddCab(cabs);
        dispatch.CustomerCabCall(new List<string>() { "Emma" });
        dispatch.RideRequest();

        dispatch.PickupCustomer();
        Assert.Equal(customer.name, dispatch.FindEnroutePassenger(CustomerStatus.Enroute)?.PassengerName);
        dispatch.DropOffCustomer();

        Assert.Null(dispatch.FindEnroutePassenger(CustomerStatus.Enroute)?.PassengerName);
    }
    [Fact]
    public void CanPickupTwoCustomersWithTwoCabs()
    {
        var cabs = new Cab("Dan's Cab", 20);
        var cabTwo = new Cab("Evan's Cab", 20);
        var dispatch = new RadioFleet();
        var customer = new Customer(
            "Emma", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        var customerTwo = new Customer(
            "Lisa", 
            "2 Fulton Drive", 
            "2 Final Destination Lane");
        dispatch.AddCab(cabs);
        var customerNames = new List<string>() { "Emma", "Lisa" };
        dispatch.CustomerCabCall(customerNames);
        dispatch.RideRequest();
        dispatch.PickupCustomer();
        dispatch.AddCab(cabTwo);
        dispatch.CustomerCabCall(customerNames);
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
        var cabs = new Cab("Dan's Cab", 20);
        var cabTwo = new Cab("Evan's Cab", 20);
        var dispatch = new RadioFleet();
        var customer = new Customer(
            "Emma", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        var customerTwo = new Customer(
            "Lisa", 
            "2 Fulton Drive", 
            "2 Final Destination Lane");
        dispatch.AddCab(cabs);
        dispatch.AddCab(cabTwo);
        var customerNames = new List<string>() { "Emma", "Lisa" };
        dispatch.CustomerCabCall(customerNames);
        dispatch.RideRequest();
        dispatch.CustomerCabCall(customerNames);
        dispatch.RideRequest();
        dispatch.PickupCustomer();
        dispatch.PickupCustomer();
        Assert.NotNull(dispatch.FindEnroutePassenger(CustomerStatus.Enroute));
        Assert.NotNull(dispatch.FindEnroutePassenger(CustomerStatus.Enroute));

        dispatch.DropOffCustomer();
        Assert.Equal("Emma", dispatch.DroppedOffCustomer().Single().PassengerName);
        dispatch.DropOffCustomer();

        Assert.Equal("Lisa", dispatch.DroppedOffCustomer().Single().PassengerName);
    }
    [Fact]
    public void CannotPickupCustomerIfNotAvailable()
    {
        var cabs = new Cab("Dan's Cab", 20);
        var dispatch = new RadioFleet();
        var customer = new Customer(
            "Emma", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        var customerTwo = new Customer(
            "Lisa", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        dispatch.AddCab(cabs);
        dispatch.CustomerCabCall(new List<string>() { "Dan" });
        var rideRequest = cabs.RequestRideFor(customerTwo);

        Assert.Throws<SystemException>(() => dispatch.PickupCustomer());

        Assert.True(rideRequest); // TODO: this should be false, there is no customer call
    }
    [Fact]
    public void CannotPickupCustomerIfNoCabs()
    {
        var dispatch = new RadioFleet();
        var customer = new Customer(
            "Emma", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        
        Assert.Throws<SystemException>(() => dispatch.RideRequest());
        Assert.Throws<SystemException>(() => dispatch.PickupCustomer());
        Assert.Throws<SystemException>(() => dispatch.DropOffCustomer());
    }
    [Fact]
    public void CabNotRequestedByDispatcherAllCallsFail()
    {
        var cabs = new Cab("Diane's Cab", 20);
        var dispatch = new RadioFleet();
        var customer = new Customer(
            "Emma", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        dispatch.AddCab(cabs);
        dispatch.CustomerCabCall(new List<string>() { "Dan" });
        Assert.Throws<SystemException>(() => dispatch.PickupCustomer());
        Assert.Null(dispatch.FindEnroutePassenger(CustomerStatus.Enroute));
        Assert.Throws<SystemException>(() => dispatch.DropOffCustomer());

        Assert.Null(dispatch.FindEnroutePassenger(CustomerStatus.Enroute));
    }
    [Fact]
    public void CustomerNotPickedUpCannotDropOff()
    {
        var cabs = new Cab("Diane's Cab", 20);
        var dispatch = new RadioFleet();
        var customer = new Customer(
            "Emma", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        dispatch.AddCab(cabs);
        dispatch.CustomerCabCall(new List<string>() { "Dan" });
        dispatch.RideRequest();
        Assert.NotNull(dispatch.FindEnroutePassenger(CustomerStatus.WaitingPickup));
        Assert.Throws<SystemException>(() => dispatch.DropOffCustomer());

        Assert.NotNull(dispatch.FindEnroutePassenger(CustomerStatus.WaitingPickup));
    }
    [Fact]
    public void CannotPickupTwoCustomerFares()
    {
        var cabs = new Cab("Diane's Cab", 20);
        var dispatch = new RadioFleet();
        var customer = new Customer(
            "Emma", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        var customerTwo = new Customer(
            "Lisa", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        dispatch.AddCab(cabs);
        dispatch.CustomerCabCall(new List<string>() { "Emma", "Lisa" });
        dispatch.CustomerCabCall(new List<string>() { "Emma", "Lisa" });
        dispatch.RideRequest();
        Assert.Throws<SystemException>(() => dispatch.RideRequest());
        
        Assert.True(dispatch.CustomerInState(CustomerStatus.CustomerCallInProgress));
    }
}
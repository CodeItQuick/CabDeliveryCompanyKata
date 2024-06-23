using Production.EmmaCabCompany;
using Production.EmmaCabCompany.Domain;

namespace Tests.CabDeliveryCompanyKata;

public class DispatchTests
{
    [Fact]
    public void CanPickupCustomer()
    {
        var cabs = new Cab("Diane's Cab", 20);
        var dispatch = new Dispatch();
        var customer = new Customer(
            "Emma", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        dispatch.AddCab(cabs);
        dispatch.RideRequest(customer);

        dispatch.PickupCustomer();
        Assert.Equal(customer.name, dispatch.FindEnroutePassenger(customer)?.PassengerName);
        dispatch.DropOffCustomer();

        Assert.Null(dispatch.FindEnroutePassenger(customer)?.PassengerName);
    }
    [Fact]
    public void CanPickupTwoCustomersWithTwoCabs()
    {
        var cabs = new Cab("Dan's Cab", 20);
        var cabTwo = new Cab("Evan's Cab", 20);
        var dispatch = new Dispatch();
        var customer = new Customer(
            "Emma", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        var customerTwo = new Customer(
            "Lisa", 
            "2 Fulton Drive", 
            "2 Final Destination Lane");
        dispatch.AddCab(cabs);
        dispatch.RideRequest(customer);
        dispatch.PickupCustomer();
        dispatch.AddCab(cabTwo);
        dispatch.RideRequest(customerTwo);
        dispatch.PickupCustomer();

        dispatch.DropOffCustomer();
        dispatch.DropOffCustomer();

        Assert.Null(dispatch.FindEnroutePassenger(customer));
        Assert.Null(dispatch.FindEnroutePassenger(customerTwo));
    }
    [Fact]
    public void CanPickupTwoCustomersWithTwoCabsSecondOrdering()
    {
        var cabs = new Cab("Dan's Cab", 20);
        var cabTwo = new Cab("Evan's Cab", 20);
        var dispatch = new Dispatch();
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
        dispatch.RideRequest(customer);
        var customerOneRequestRide = dispatch.FindEnroutePassenger(customer);
        dispatch.RideRequest(customerTwo);
        var customerTwoRequestRide = dispatch.FindEnroutePassenger(customer);
        dispatch.PickupCustomer();
        dispatch.PickupCustomer();
        Assert.NotNull(dispatch.FindEnroutePassenger(customer));
        Assert.NotNull(dispatch.FindEnroutePassenger(customer));

        dispatch.DropOffCustomer();
        dispatch.DropOffCustomer();

        Assert.NotNull(customerOneRequestRide);
        Assert.NotNull(customerTwoRequestRide);
        Assert.Null(dispatch.FindEnroutePassenger(customer));
        Assert.Null(dispatch.FindEnroutePassenger(customer));
    }
    [Fact]
    public void CannotPickupCustomerIfNotAvailable()
    {
        var cabs = new Cab("Dan's Cab", 20);
        var dispatch = new Dispatch();
        var customer = new Customer(
            "Emma", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        var customerTwo = new Customer(
            "Lisa", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        dispatch.AddCab(cabs);
        var rideRequest = cabs.RequestRideFor(customerTwo);

        Assert.Throws<SystemException>(() => dispatch.PickupCustomer());

        Assert.True(rideRequest); // TODO: this should be false, there is no customer call
    }
    [Fact]
    public void CannotPickupCustomerIfNoCabs()
    {
        var dispatch = new Dispatch();
        var customer = new Customer(
            "Emma", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        
        Assert.Throws<SystemException>(() => dispatch.RideRequest(customer));
        Assert.Throws<SystemException>(() => dispatch.PickupCustomer());
        Assert.Throws<SystemException>(() => dispatch.DropOffCustomer());
    }
    [Fact]
    public void CabNotRequestedByDispatcherAllCallsFail()
    {
        var cabs = new Cab("Diane's Cab", 20);
        var dispatch = new Dispatch();
        var customer = new Customer(
            "Emma", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        dispatch.AddCab(cabs);
        Assert.Throws<SystemException>(() => dispatch.PickupCustomer());
        Assert.Null(dispatch.FindEnroutePassenger(customer));
        dispatch.DropOffCustomer();

        Assert.Null(dispatch.FindEnroutePassenger(customer));
    }
    [Fact]
    public void CustomerNotPickedUpCannotDropOff()
    {
        var cabs = new Cab("Diane's Cab", 20);
        var dispatch = new Dispatch();
        var customer = new Customer(
            "Emma", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        dispatch.AddCab(cabs);
        dispatch.RideRequest(customer);
        Assert.NotNull(dispatch.FindEnroutePassenger(customer));
        dispatch.DropOffCustomer();

        Assert.NotNull(dispatch.FindEnroutePassenger(customer));
    }
    [Fact]
    public void CannotPickupTwoCustomerFares()
    {
        var cabs = new Cab("Diane's Cab", 20);
        var dispatch = new Dispatch();
        var customer = new Customer(
            "Emma", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        var customerTwo = new Customer(
            "Lisa", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        dispatch.AddCab(cabs);
        dispatch.RideRequest(customer);
        var rideRequested = dispatch.FindEnroutePassenger(customer);
        dispatch.PickupCustomer();
        dispatch.DropOffCustomer();
        var rideRequestedTwo = dispatch.FindEnroutePassenger(customerTwo);
        
        Assert.NotNull(rideRequested);
        Assert.Null(rideRequestedTwo);
    }
}
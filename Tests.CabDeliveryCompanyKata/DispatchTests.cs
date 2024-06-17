using Production.EmmaCabCompany;

namespace Tests.CabDeliveryCompanyKata;

public class DispatchTests
{
    [Fact]
    public void CanPickupCustomer()
    {
        var cabCompanyPrinter = new FakeCabCompanyPrinter();
        var cabs = new Cab("Diane's Cab", 20);
        var dispatch = new Dispatch();
        var customer = new Customer(
            "Emma", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        dispatch.AddCab(cabs);
        dispatch.RideRequest(customer);

        var pickupCustomer = dispatch.PickupCustomer(customer);
        var dropOffCustomers = dispatch.DropOffCustomers();

        Assert.True(pickupCustomer);
        Assert.Single(dropOffCustomers);
    }
    [Fact]
    public void CanPickupTwoCustomersWithTwoCabs()
    {
        var cabCompanyPrinter = new FakeCabCompanyPrinter();
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
        var pickupCustomer = dispatch.PickupCustomer(customer);
        dispatch.AddCab(cabTwo);
        dispatch.RideRequest(customerTwo);
        var pickupCustomerTwo = dispatch.PickupCustomer(customerTwo);

        var dropOffCustomers = dispatch.DropOffCustomers();

        Assert.True(pickupCustomer);
        Assert.True(pickupCustomerTwo);
        Assert.Equal(2, dropOffCustomers.Count);
    }
    [Fact]
    public void CanPickupTwoCustomersWithTwoCabsSecondOrdering()
    {
        var cabCompanyPrinter = new FakeCabCompanyPrinter();
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
        var cabOneAdded = dispatch.AddCab(cabs);
        var cabTwoAdded = dispatch.AddCab(cabTwo);
        var customerOneRequestRide = dispatch.RideRequest(customer);
        var customerTwoRequestRide = dispatch.RideRequest(customerTwo);
        var customerOnePickedUp = dispatch.PickupCustomer(customer);
        var customerTwoPickedUp = dispatch.PickupCustomer(customerTwo);

        var dropOffCustomers = dispatch.DropOffCustomers();

        Assert.True(cabOneAdded);
        Assert.True(cabTwoAdded);
        Assert.NotNull(customerOneRequestRide);
        Assert.NotNull(customerTwoRequestRide);
        Assert.True(customerOnePickedUp);
        Assert.True(customerTwoPickedUp);
        Assert.Equal(2, dropOffCustomers.Count);
    }
    [Fact]
    public void CannotPickupCustomerIfNotAvailable()
    {
        var cabCompanyPrinter = new FakeCabCompanyPrinter();
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
        var addCab = dispatch.AddCab(cabs);
        var rideRequest = cabs.RideRequest(customerTwo);

        var pickupCustomer = dispatch.PickupCustomer(customer);

        Assert.True(addCab);
        Assert.True(rideRequest); // TODO: this should be false, there is no customer call
        Assert.False(pickupCustomer);
    }
    [Fact]
    public void CannotPickupCustomerIfNoCabs()
    {
        var dispatch = new Dispatch();
        var customer = new Customer(
            "Emma", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        
        Assert.Throws<SystemException>(() =>  dispatch.RideRequest(customer));
        var pickupCustomer = dispatch.PickupCustomer(customer);
        Assert.False(pickupCustomer);
        Assert.Throws<SystemException>(() => dispatch.DropOffCustomers());
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
        var pickupCustomer = dispatch.PickupCustomer(customer);
        var allDroppedOff = dispatch.DropOffCustomers();

        Assert.False(pickupCustomer);
        Assert.Empty(allDroppedOff);
    }
    [Fact]
    public void CustomerNotPickedUpCannotDropOff()
    {
        var cabCompanyPrinter = new FakeCabCompanyPrinter();
        var cabs = new Cab("Diane's Cab", 20);
        var dispatch = new Dispatch();
        var customer = new Customer(
            "Emma", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        dispatch.AddCab(cabs);
        var rideRequested = dispatch.RideRequest(customer);
        var allDroppedOff = dispatch.DropOffCustomers();

        Assert.NotNull(rideRequested);
        Assert.Empty(allDroppedOff);
    }
    [Fact]
    public void CannotPickupTwoCustomerFares()
    {
        var cabCompanyPrinter = new FakeCabCompanyPrinter();
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
        var rideRequested = dispatch.RideRequest(customer);
        var customerPickedUp = dispatch.PickupCustomer(customerTwo);
        var allDroppedOff = dispatch.DropOffCustomers();

        Assert.NotNull(rideRequested);
        Assert.False(customerPickedUp);
        Assert.Empty(allDroppedOff);
    }
}
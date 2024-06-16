using Production.EmmaCabCompany;

namespace Tests.CabDeliveryCompanyKata;

public class DispatchTests
{
    [Fact]
    public void CanPickupCustomer()
    {
        var cabCompanyPrinter = new FakeCabCompanyPrinter();
        var cabs = new Cab("Diane's Cab", cabCompanyPrinter, 20);
        var dispatch = new Dispatch(cabCompanyPrinter);
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
        var cabs = new Cab("Dan's Cab", cabCompanyPrinter, 20);
        var cabTwo = new Cab("Evan's Cab", cabCompanyPrinter, 20);
        var dispatch = new Dispatch(cabCompanyPrinter);
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
        var cabs = new Cab("Dan's Cab", cabCompanyPrinter, 20);
        var cabTwo = new Cab("Evan's Cab", cabCompanyPrinter, 20);
        var dispatch = new Dispatch(cabCompanyPrinter);
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
        dispatch.RideRequest(customerTwo);
        dispatch.PickupCustomer(customer);
        dispatch.PickupCustomer(customerTwo);

        dispatch.DropOffCustomers();
        
        // TODO: Broken, Dan's cab doesn't pick anyone up
        Assert.Contains("Evan's Cab picked up Lisa at 2 Fulton Drive.", 
            cabCompanyPrinter.List());
        Assert.Contains("Dan's Cab picked up Emma at 1 Fulton Drive.", 
            cabCompanyPrinter.List()); 
        Assert.Contains("Evan's Cab dropped off Lisa at 2 Final Destination Lane.", 
            cabCompanyPrinter.List()); 
        Assert.Contains("Dan's Cab dropped off Emma at 1 Final Destination Lane.", 
            cabCompanyPrinter.List()); 
        Assert.Equal(4, cabCompanyPrinter.CountMessages());
    }
    [Fact]
    public void CannotPickupCustomerIfNotAvailable()
    {
        var cabCompanyPrinter = new FakeCabCompanyPrinter();
        var cabs = new Cab("Dan's Cab", cabCompanyPrinter, 20);
        var dispatch = new Dispatch(cabCompanyPrinter);
        var customer = new Customer(
            "Emma", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        var customerTwo = new Customer(
            "Lisa", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        dispatch.AddCab(cabs);
        cabs.RideRequest(customerTwo);

        dispatch.PickupCustomer(customer);
        
        Assert.Equal(0, cabCompanyPrinter.CountMessages());
    }
    [Fact]
    public void CannotPickupCustomerIfNoCabs()
    {
        var cabCompanyPrinter = new FakeCabCompanyPrinter();
        var cabs = new Cab("Diane's Cab", cabCompanyPrinter, 20);
        var dispatch = new Dispatch(cabCompanyPrinter);
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
        var cabCompanyPrinter = new FakeCabCompanyPrinter();
        var cabs = new Cab("Diane's Cab", cabCompanyPrinter, 20);
        var dispatch = new Dispatch(cabCompanyPrinter);
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
        var cabs = new Cab("Diane's Cab", cabCompanyPrinter, 20);
        var dispatch = new Dispatch(cabCompanyPrinter);
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
        var cabs = new Cab("Diane's Cab", cabCompanyPrinter, 20);
        var dispatch = new Dispatch(cabCompanyPrinter);
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
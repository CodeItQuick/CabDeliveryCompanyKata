using Production.EmmaCabCompany;

namespace Tests.CabDeliveryCompanyKata;

public class DispatchTests
{
    [Fact]
    public void CanPickupCustomer()
    {
        var cabCompanyPrinter = new FakeCabCompanyPrinter();
        var cabs = new Cabs("Dan's Cab", cabCompanyPrinter, 20);
        var dispatch = new Dispatch(cabCompanyPrinter);
        var customer = new Customer(
            "Emma", 
            "1 Fulton Drive", 
            "1 Final Destination Lane");
        dispatch.AddCab(cabs);
        dispatch.RideRequest(customer);
        dispatch.PickupCustomer(customer);

        dispatch.DropOffCustomers();
        
        Assert.Contains("Evan's Cab picked up Emma at 1 Fulton Drive.", 
            cabCompanyPrinter.Retrieve(0));
        Assert.Contains("Evan's Cab dropped off Emma at 1 Final Destination Lane.", 
            cabCompanyPrinter.Retrieve(1));
    }
    [Fact]
    public void CannotPickupCustomerIfNotAvailable()
    {
        var cabCompanyPrinter = new FakeCabCompanyPrinter();
        var cabs = new Cabs("Dan's Cab", cabCompanyPrinter, 20);
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
}
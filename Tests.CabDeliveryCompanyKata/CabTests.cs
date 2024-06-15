using Production.EmmaCabCompany;

namespace Tests.CabDeliveryCompanyKata;

public class CabTests
{
    [Fact]
    public void CabCanAcceptRideRequest()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        var cab = new Cabs("Evan's Cab", cabCompanyPrinter, 20);
        
        var acceptedRide = cab.RideRequest(new Customer("Lisa", "1 Fulton Drive", "1 Destination Avenue"));

        Assert.True(acceptedRide);
    }
    [Fact]
    public void CabDenyRideRequestWhenNotAvailable()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        var cab = new Cabs("Evan's Cab", cabCompanyPrinter, 20);
        cab.RideRequest(new Customer("Emma", "1 Fulton Drive", "1 Destination Avenue"));

        var pickupSuccess = cab.PickupCustomer(new Customer("Lisa", "1 Fulton Drive", "1 Destination Avenue"));

        Assert.False(pickupSuccess);
    }
    [Fact]
    public void CabAcceptRideRequestWhenAvailableAfterPickingUpFare()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        var cab = new Cabs("Evan's Cab", cabCompanyPrinter, 20);
        var acceptedRide = cab.RideRequest(new Customer("Lisa", "1 Fulton Drive", "1 Destination Avenue"));
        
        cab.PickupCustomer(new Customer("Lisa", "1 Fulton Drive", "1 Destination Avenue"));
        cab.DropOffCustomer();

        Assert.True(acceptedRide);
    }
}
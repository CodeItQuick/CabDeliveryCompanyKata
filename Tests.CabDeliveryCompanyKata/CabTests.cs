using Production.EmmaCabCompany;

namespace Tests.CabDeliveryCompanyKata;

public class CabTests
{
    [Fact]
    public void CabCanAcceptRideRequest()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        var cab = new Cabs("Evan's Cab", cabCompanyPrinter, 20);


        var acceptedRide = cab.RideRequest();

        Assert.True(acceptedRide);
    }
    [Fact]
    public void CabDenyRideRequestWhenNotAvailable()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        var cab = new Cabs("Evan's Cab", cabCompanyPrinter, 20);
        cab.PickupCustomer(new Customer("Lisa", "1 Fulton Drive", "1 Destination Avenue"));

        var acceptedRide = cab.RideRequest();

        Assert.False(acceptedRide);
    }
    [Fact]
    public void CabAcceptRideRequestWhenAvailableAfterPickingUpFare()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        var cab = new Cabs("Evan's Cab", cabCompanyPrinter, 20);
        cab.PickupCustomer(new Customer("Lisa", "1 Fulton Drive", "1 Destination Avenue"));
        cab.DropOffCustomer();
        
        var acceptedRide = cab.RideRequest();

        Assert.True(acceptedRide);
    }
}
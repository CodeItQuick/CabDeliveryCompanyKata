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
    public void CabCanAcceptARide()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        var cab = new Cabs("Evan's Cab", cabCompanyPrinter, 20);
        var passenger = new Customer("Lisa", "1 Fulton Drive", "1 Destination Avenue");
        cab.RideRequest(passenger);
        cab.PickupCustomer(passenger);

        var dropOffCustomer = cab.DropOffCustomer();

        Assert.True(dropOffCustomer);
    }
    [Fact]
    public void CabCanAcceptMultipleRides()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        var cab = new Cabs("Evan's Cab", cabCompanyPrinter, 20);
        var passenger = new Customer("Lisa", "1 Fulton Drive", "1 Destination Avenue");
        cab.RideRequest(passenger);
        cab.PickupCustomer(passenger);
        var dropOffCustomer = cab.DropOffCustomer();
        var passengerTwo = new Customer("Emma", "1 Fulton Drive", "1 Destination Avenue");
        cab.RideRequest(passengerTwo);
        cab.PickupCustomer(passengerTwo);
        var dropOffCustomerTwo = cab.DropOffCustomer();

        Assert.True(dropOffCustomer);
        Assert.True(dropOffCustomerTwo);
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
        var passenger = new Customer("Lisa", "1 Fulton Drive", "1 Destination Avenue");
        var acceptedRide = cab.RideRequest(passenger);
        var pickupCustomer = cab.PickupCustomer(passenger);

        var dropOffCustomer = cab.DropOffCustomer();

        Assert.True(acceptedRide);
        Assert.True(pickupCustomer);
        Assert.True(dropOffCustomer);
    }
    [Fact]
    public void CabDenyRideRequestWhenNotAvailableAfterPickingUpAnotherFare()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        var cab = new Cabs("Evan's Cab", cabCompanyPrinter, 20);
        var passenger = new Customer("Lisa", "1 Fulton Drive", "1 Destination Avenue");
        var passengerDenied = new Customer("Emma", "2 Fulton Drive", "2 Destination Avenue");
        var acceptedRide = cab.RideRequest(passenger);
        var pickupCustomer = cab.PickupCustomer(passengerDenied);

        var dropOffCustomer = cab.DropOffCustomer();

        Assert.True(acceptedRide);
        Assert.False(pickupCustomer);
        Assert.False(dropOffCustomer);
    }
}
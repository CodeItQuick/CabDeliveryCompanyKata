using Production.EmmaCabCompany;

namespace Tests.CabDeliveryCompanyKata;

public class CabTests
{
    [Fact]
    public void CabCanAcceptRideRequest()
    {
        var cab = new Cab("Evan's Cab", 20, 46.2382, 63.1311);
        
        var acceptedRide = cab.RequestRideFor(new Customer("Lisa", "1 Fulton Drive", "1 Destination Avenue"));

        Assert.True(acceptedRide);
    }
    [Fact]
    public void CabCanAcceptARide()
    {
        var cab = new Cab("Evan's Cab", 20, 46.2382, 63.1311);
        var passenger = new Customer("Lisa", "1 Fulton Drive", "1 Destination Avenue");
        cab.RequestRideFor(passenger);
        cab.PickupAssignedCustomer(passenger);

        var dropOffCustomer = cab.DropOffCustomer();

        Assert.True(dropOffCustomer);
    }
    [Fact]
    public void CabCanAcceptMultipleRides()
    {
        var cab = new Cab("Evan's Cab", 20, 46.2382, 63.1311);
        var passenger = new Customer("Lisa", "1 Fulton Drive", "1 Destination Avenue");
        cab.RequestRideFor(passenger);
        cab.PickupAssignedCustomer(passenger);
        var dropOffCustomer = cab.DropOffCustomer();
        var passengerTwo = new Customer("Emma", "1 Fulton Drive", "1 Destination Avenue");
        cab.RequestRideFor(passengerTwo);
        cab.PickupAssignedCustomer(passengerTwo);
        var dropOffCustomerTwo = cab.DropOffCustomer();

        Assert.True(dropOffCustomer);
        Assert.True(dropOffCustomerTwo);
    }
    [Fact]
    public void CabDenyRideRequestWhenNotAvailable()
    {
        var cab = new Cab("Evan's Cab", 20, 46.2382, 63.1311);
        cab.RequestRideFor(new Customer("Emma", "1 Fulton Drive", "1 Destination Avenue"));

        var pickupSuccess = cab.PickupAssignedCustomer(new Customer("Lisa", "1 Fulton Drive", "1 Destination Avenue"));

        Assert.False(pickupSuccess);
    }
    [Fact]
    public void CabAcceptRideRequestWhenAvailableAfterPickingUpFare()
    {
        var cab = new Cab("Evan's Cab", 20, 46.2382, 63.1311);
        var passenger = new Customer("Lisa", "1 Fulton Drive", "1 Destination Avenue");
        var acceptedRide = cab.RequestRideFor(passenger);
        var pickupCustomer = cab.PickupAssignedCustomer(passenger);

        var dropOffCustomer = cab.DropOffCustomer();

        Assert.True(acceptedRide);
        Assert.True(pickupCustomer);
        Assert.True(dropOffCustomer);
    }
    [Fact]
    public void CabDenyRideRequestWhenNotAvailableAfterPickingUpAnotherFare()
    {
        var cab = new Cab("Evan's Cab", 20, 46.2382, 63.1311);
        var passenger = new Customer("Lisa", "1 Fulton Drive", "1 Destination Avenue");
        var passengerDenied = new Customer("Emma", "2 Fulton Drive", "2 Destination Avenue");
        var acceptedRide = cab.RequestRideFor(passenger);
        var pickupCustomer = cab.PickupAssignedCustomer(passengerDenied);

        var dropOffCustomer = cab.DropOffCustomer();

        Assert.True(acceptedRide);
        Assert.False(pickupCustomer);
        Assert.False(dropOffCustomer);
    }

    [Fact]
    public void CabKnowsItsCurrentLocation()
    {
        var cab = new Cab("Evan's Cab", 20, 46.2382, 63.1311);

        var location = cab.CurrentLocation();
        
        Assert.Equal((46.2382, 63.1311), location);
    }
}
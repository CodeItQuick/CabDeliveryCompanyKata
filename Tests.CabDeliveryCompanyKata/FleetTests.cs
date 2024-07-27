using Production.EmmaCabCompany;
using Production.EmmaCabCompany.Domain;

namespace Tests.CabDeliveryCompanyKata;

public class FleetTests
{
    // TODO: what if you cancel while a cab is enroute
    [Fact]
    public void DispatchCannotAssignPassengerToEmptyFleet()
    {
        var fleet = new Fleet();
        var customer = new Customer("Evan", "1 Fulton Drive", "2 Destination Lane");

        Assert.Throws<SystemException>(() => fleet.RideRequested(customer));
    }
    [Fact]
    public void DispatchCanAssignPassengerToFleet()
    {
        var fleet = new Fleet();
        fleet.AddCab(new Cab("Dan's Cab", 20, 46.2382, 63.1311));
        var customer = new Customer("Evan", "1 Fulton Drive", "2 Destination Lane");

        fleet.RideRequested(customer);
        
        Assert.Equivalent(new CabInfo()
        {
            PassengerName = "Evan",
            CabName = "Dan's Cab",
            Destination = "2 Destination Lane",
            StartLocation = "1 Fulton Drive"
        }, fleet.LastRideAssigned());
    }
    [Fact]
    public void DispatchCanAssignPassengerToTwoPassengerFleet()
    {
        var fleet = new Fleet();
        fleet.AddCab(new Cab("Dan's Cab", 20, 46.2382, 63.1311));
        fleet.AddCab(new Cab("Lisa's Cab", 20, 46.2382, 63.1311));
        var customer = new Customer("Evan", "1 Fulton Drive", "2 Destination Lane");

        fleet.RideRequested(customer);
        fleet.RideRequested(customer);
        
        Assert.Equivalent(new CabInfo()
        {
            PassengerName = "Evan",
            CabName = "Lisa's Cab",
            Destination = "2 Destination Lane",
            StartLocation = "1 Fulton Drive"
        }, fleet.LastRideAssigned());
    }
    [Fact]
    public void DispatchCanAssignTwoPassengerToTwoPassengerFleet()
    {
        var fleet = new Fleet();
        fleet.AddCab(new Cab("Dan's Cab", 20, 46.2382, 63.1311));
        fleet.AddCab(new Cab("Lisa's Cab", 20, 46.2382, 63.1311));
        var customer = new Customer("Evan", "1 Fulton Drive", "2 Destination Lane");
        var customerTwo = new Customer("Emma", "2 Fulton Drive", "3 Destination Lane");
        fleet.RideRequested(customer);

        fleet.RideRequested(customerTwo);
        
        Assert.Equal("Dan's Cab", fleet.FindCab(customer));
        Assert.Equal("Lisa's Cab", fleet.FindCab(customerTwo));
    }

    [Fact]
    public void FleetCanDecideWhichCabToSendByLatitude()
    {
        var fleet = new Fleet();
        var cab = new Cab("Evan's Cab", 20, 46.2382, 63.1311);
        var cabTwo = new Cab("Dan's Cab", 20, 46.5555, 63.1311);
        fleet.AddCab(cab);
        fleet.AddCab(cabTwo);
        // Customer Location GPS: 46.5556, 63.1311
        var customer = new Customer("Evan", "1 Fulton Drive", "2 Destination Lane");
        
        fleet.RideRequested(customer);

        Assert.Equal("Dan's Cab", fleet.FindCab(customer));
    }
    [Fact]
    public void FleetCanDecideWhichCabToSendByLongitude()
    {
        var fleet = new Fleet();
        var cab = new Cab("Evan's Cab", 20, 46.5556, 63.1333);
        var cabTwo = new Cab("Dan's Cab", 20, 46.5556, 63.1312);
        fleet.AddCab(cab);
        fleet.AddCab(cabTwo);
        // Customer Location GPS: 46.5556, 63.1311
        var customer = new Customer("Evan", "1 Fulton Drive", "2 Destination Lane");
        
        fleet.RideRequested(customer);

        Assert.Equal("Dan's Cab", fleet.FindCab(customer));
    }
    [Fact]
    public void FleetCanDecideWhichCabToSendByLongitudeAndLatitude()
    {
        var fleet = new Fleet();
        var cab = new Cab("Evan's Cab", 20, 46.5554, 63.1310);
        var cabTwo = new Cab("Dan's Cab", 20, 46.7556, 63.7312);
        fleet.AddCab(cab);
        fleet.AddCab(cabTwo);
        // Customer Location GPS: 46.5556, 63.1311
        var customer = new Customer("Evan", "1 Fulton Drive", "2 Destination Lane");
        
        fleet.RideRequested(customer);

        Assert.Equal("Evan's Cab", fleet.FindCab(customer));
    }
}
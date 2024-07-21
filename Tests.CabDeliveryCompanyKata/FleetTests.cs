using Production.EmmaCabCompany;

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
        fleet.AddCab(new Cab("Dan's Cab", 20));
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
        fleet.AddCab(new Cab("Dan's Cab", 20));
        fleet.AddCab(new Cab("Lisa's Cab", 20));
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
        fleet.AddCab(new Cab("Dan's Cab", 20));
        fleet.AddCab(new Cab("Lisa's Cab", 20));
        var customer = new Customer("Evan", "1 Fulton Drive", "2 Destination Lane");
        var customerTwo = new Customer("Emma", "2 Fulton Drive", "3 Destination Lane");
        fleet.RideRequested(customer);

        fleet.RideRequested(customerTwo);
        
        Assert.Equivalent(new CabInfo()
        {
            PassengerName = "Emma",
            CabName = "Lisa's Cab",
            Destination = "3 Destination Lane",
            StartLocation = "2 Fulton Drive"
        }, fleet.LastRideAssigned());
    }
}
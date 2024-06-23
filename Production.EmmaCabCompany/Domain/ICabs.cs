namespace Production.EmmaCabCompany;

public interface ICabs
{
    public bool PickupCustomer(Customer customer);
    public bool IsEnroute();
    public bool IsAvailable();
    public bool IsStatus(CabStatus requestedStatus);
    public bool DropOffCustomer();
    public bool RequestRideFor(Customer? customer);
    public bool RideInProgress();
    public CabInfo CabInfo();
    public bool ContainsPassenger();
}
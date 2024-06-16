namespace Production.EmmaCabCompany;

public interface ICabs
{
    public bool PickupCustomer(Customer customer);
    public bool DropOffCustomer();
    public bool RideRequest(Customer? customer);
    public bool RideInProgress();
    public CabInfo CabInfo();
}
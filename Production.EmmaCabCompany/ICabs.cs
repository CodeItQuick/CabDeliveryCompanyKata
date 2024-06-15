namespace Production.EmmaCabCompany;

public interface ICabs
{
    public bool PickupCustomer(Customer customer);
    public bool DropOffCustomer();
}
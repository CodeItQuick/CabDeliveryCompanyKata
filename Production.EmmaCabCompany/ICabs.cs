namespace Production.EmmaCabCompany;

public interface ICabs
{
    public void PickupCustomer(Customer customer);
    public void DropOffCustomer(Customer customer);
    int Wallet { get; set; }
}
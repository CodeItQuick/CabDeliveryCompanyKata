namespace Production.EmmaCabCompany;

public class EmmaCabCompany 
{
    public static void CallCab(List<ICabs> cabs, List<Customer> customers)
    {
        cabs.First().PickupCustomer(customers.First());
        cabs.First().DropOffCustomer(customers.First());
    }
}
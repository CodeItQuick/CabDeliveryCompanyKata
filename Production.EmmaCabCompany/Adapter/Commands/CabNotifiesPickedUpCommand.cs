namespace Production.EmmaCabCompany.Commands;

public static class CabNotifiesPickedUpCommand
{
    public static void Select(Dispatch dispatch, List<Customer> customersAwaitingPickup, List<Customer> customersPickedUp, ICabCompanyPrinter cabCompanyPrinter)
    {
        if (dispatch.NoCabsInFleet())
        {
            cabCompanyPrinter.WriteLine("There are currently no cabs in the fleet.");
        }
        else if (customersAwaitingPickup.Count == 0)
        {
            cabCompanyPrinter.WriteLine("There are currently no customer's assigned to cabs.");
        }
        else
        {
            var customer = customersAwaitingPickup.FirstOrDefault();
            dispatch.PickupCustomer(customer);
            customersPickedUp.Add(customer);
            customersAwaitingPickup.RemoveAt(0);
        }
    }
}
namespace Production.EmmaCabCompany.Commands;

public static class CabNotifiesPickedUpCommand
{
    public static string Select(
        Dispatch dispatch, 
        List<Customer> customersAwaitingPickup, 
        List<Customer> customersPickedUp)
    {
        if (dispatch.NoCabsInFleet())
        {
            return "There are currently no cabs in the fleet.";
        }

        if (customersAwaitingPickup.Count == 0)
        {
            return "There are currently no customer's assigned to cabs.";
        }
        var customer = customersAwaitingPickup.FirstOrDefault();
        dispatch.PickupCustomer(customer);
        customersPickedUp.Add(customer);
        customersAwaitingPickup.RemoveAt(0);
        return "Notified dispatcher of pickup";
    }
}
namespace Production.EmmaCabCompany.Commands;

public static class CustomerCancelledCabRideCommand
{
    public static List<string> Select(List<Customer> customersAwaitingPickup, List<Customer> customersPickedUp, ICabCompanyPrinter cabCompanyPrinter)
    {
        var list = new List<string>();
        if (customersAwaitingPickup.Any())
        {
            customersAwaitingPickup.RemoveAt(customersAwaitingPickup.Count - 1);
            list.Add("Customer cancelled request before cab assigned.");
            
        }

        if (!customersPickedUp.Any())
        {
            return list;
        }
        customersPickedUp.RemoveAt(customersPickedUp.Count - 1);
        list.Add("Customer cancelled request before cab got there.");

        return list;
    }
}
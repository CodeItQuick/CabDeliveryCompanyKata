namespace Production.EmmaCabCompany.Commands;

public static class CustomerCancelledCabRideCommand
{
    public static void Select(List<Customer> customersAwaitingPickup, List<Customer> customersPickedUp, ICabCompanyPrinter cabCompanyPrinter)
    {
        if (customersAwaitingPickup.Any())
        {
            customersAwaitingPickup.RemoveAt(customersAwaitingPickup.Count - 1);
            cabCompanyPrinter.WriteLine("Customer cancelled request before cab assigned.");
        }

        if (customersPickedUp.Any())
        {
            customersPickedUp.RemoveAt(customersPickedUp.Count - 1);
            cabCompanyPrinter.WriteLine("Customer cancelled request before cab got there.");
        }
    }
}
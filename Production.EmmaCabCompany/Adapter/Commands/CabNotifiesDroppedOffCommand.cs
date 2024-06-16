namespace Production.EmmaCabCompany.Commands;

public static class CabNotifiesDroppedOffCommand
{
    public static List<Customer> Select(List<Customer> customersPickedUp, Dispatch dispatch, ICabCompanyPrinter cabCompanyPrinter)
    {
        if (customersPickedUp.Count == 0)
        {
            cabCompanyPrinter.WriteLine("There are currently no customer's assigned to cabs.");
        }
        else
        {
            var droppedOffCustomers = dispatch.DropOffCustomers();
            foreach (var cabInfo in droppedOffCustomers)
            {
                cabCompanyPrinter.WriteLine($"{cabInfo.CabName} dropped off {cabInfo.PassengerName} at {cabInfo.Destination}.");
            }
            customersPickedUp = new List<Customer>();
        }

        return customersPickedUp;
    }
}
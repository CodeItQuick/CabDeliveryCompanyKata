namespace Production.EmmaCabCompany.Commands;

public static class CabNotifiesDroppedOffCommand
{
    public static List<string> Select(List<Customer> customersPickedUp, Dispatch dispatch)
    {
        var list = new List<string>();
        if (customersPickedUp.Count == 0)
        {
            return new List<string> { "There are currently no customer's assigned to cabs." };
        }
        else
        {
            var droppedOffCustomers = dispatch.DropOffCustomers();
            foreach (var cabInfo in droppedOffCustomers)
            {
                list.Add($"{cabInfo.CabName} dropped off {cabInfo.PassengerName} at {cabInfo.Destination}.");
            }
            customersPickedUp = new List<Customer>();
        }

        return list;
    }
}
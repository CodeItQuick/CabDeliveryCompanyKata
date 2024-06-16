namespace Production.EmmaCabCompany.Commands;

public static class SendCabRequestCommand
{
    public static void Select(
        Dispatch dispatch, 
        List<Customer> customersCallInProgress,
        List<Customer> customersAwaitingPickup, 
        ICabCompanyPrinter cabCompanyPrinter)
    {
        if (dispatch.NoCabsInFleet())
        {
            cabCompanyPrinter.WriteLine("There are currently no cabs in the fleet.");
        }
        else if (customersCallInProgress.Any())
        {
            try
            {
                var customer = customersCallInProgress.Skip(0).First();
                var cabInfo = dispatch.RideRequest(customer);
                if (cabInfo != null)
                {
                    cabCompanyPrinter.WriteLine($"{cabInfo.CabName} picked up {cabInfo.PassengerName} at {cabInfo.StartLocation}.");
                }
                customersAwaitingPickup.Add(customer);
                customersCallInProgress.RemoveAt(0);
                cabCompanyPrinter.WriteLine("Cab assigned to customer.");
            }
            catch (SystemException ex)
            {
                cabCompanyPrinter.WriteLine(ex.Message);
            }
        }
        else
        {
            cabCompanyPrinter.WriteLine("There are currently no customer's waiting for cabs.");
        }
    }
}
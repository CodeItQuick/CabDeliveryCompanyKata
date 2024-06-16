namespace Production.EmmaCabCompany.Commands;

public static class SendCabRequestCommand
{
    public static List<string> Select(
        Dispatch dispatch, 
        List<Customer> customersCallInProgress,
        List<Customer> customersAwaitingPickup, 
        ICabCompanyPrinter cabCompanyPrinter)
    {
        if (dispatch.NoCabsInFleet())
        {
            return ["There are currently no cabs in the fleet."];
        }
        if (customersCallInProgress.Any())
        {
            try
            {
                var customer = customersCallInProgress.Skip(0).First();
                var cabInfo = dispatch.RideRequest(customer);
                if (cabInfo != null)
                {
                    customersAwaitingPickup.Add(customer);
                    customersCallInProgress.RemoveAt(0);
                    return
                    [
                        $"{cabInfo.CabName} picked up {cabInfo.PassengerName} at {cabInfo.StartLocation}.",
                        "Cab assigned to customer."
                    ];
                }
                customersAwaitingPickup.Add(customer);
                customersCallInProgress.RemoveAt(0);
                return ["Cab assigned to customer."];
            }
            catch (SystemException ex)
            {
                cabCompanyPrinter.WriteLine(ex.Message);
            }
        }
        else
        {
            return ["There are currently no customer's waiting for cabs."];
        }

        return ["There are no customer calls in progress"];
    }
}
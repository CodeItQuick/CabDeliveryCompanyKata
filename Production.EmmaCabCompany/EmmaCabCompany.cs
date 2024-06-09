namespace Production.EmmaCabCompany;

public class EmmaCabCompany 
{
    public static void CallCab(List<ICabs> cabs, List<Customer> customers)
    {
        customers.Sort(
            (x, y) => y.RideHistory.NameOfCabsTaken.Count() - x.RideHistory.NameOfCabsTaken.Count());
        var numberFaresTaken = customers.FirstOrDefault()?.RideHistory.NameOfCabsTaken.Count ?? 0;
        while (customers.Any(x => x.RideHistory.NameOfCabsTaken.Count == numberFaresTaken))
        {
            // find the customer remaining
            var currentCustomer = customers.FirstOrDefault(x => x.RideHistory.NameOfCabsTaken.Count == numberFaresTaken);
            if (currentCustomer == null) continue;
            
            // Grab poorest cab
            cabs.Sort((x, y) => x.Wallet - y.Wallet);
            var poorestCabbie = cabs.First();
            poorestCabbie.PickupCustomer(currentCustomer);
            poorestCabbie.DropOffCustomer(currentCustomer);
        }
    }
}
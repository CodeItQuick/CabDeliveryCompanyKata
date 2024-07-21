using Production.EmmaCabCompany.Domain;

namespace Production.EmmaCabCompany;

public class CabService
{
    private readonly DispatcherCoordinator dispatcherCoordinator;
    private readonly IFileHandler _fileHandler;

    public CabService(DispatcherCoordinator dispatcherCoordinator, IFileHandler fileHandler)
    {
        var customerList = fileHandler.ReadCustomerList();
        var customerListStrings = customerList.Select(x => x);
        Dictionary<Customer, CustomerStatus> customerDictionary = new Dictionary<Customer, CustomerStatus>();
        foreach (var customer in customerListStrings)
        {
            var customerAttribs = customer.Split(",");
            if (customerAttribs.Length <= 2) continue;
            var customerKey = new Customer(customerAttribs[0], customerAttribs[1], customerAttribs[2]);
            var customerStatus = Enum.Parse<CustomerStatus>(customerAttribs[3], true);
            customerDictionary.Add(customerKey, customerStatus);
        }
        dispatcherCoordinator.RebuildCustomerDictionary(customerDictionary);
        var cabList = fileHandler.ReadReadCabList();
        var cabListStrings = cabList.Select(x => x).ToList();
        List<Cab> cabStoredList = new List<Cab>();
        foreach (var cab in cabListStrings)
        {
            var cabAttributes = cab.Split(",");
            if (cabAttributes.Length < 1 || string.IsNullOrWhiteSpace(cabAttributes[0])) continue;
            var cabValue = new Cab(cabAttributes[0], 20);
            cabStoredList.Add(cabValue);
        }
        dispatcherCoordinator.RebuildCabList(cabStoredList);
        this.dispatcherCoordinator = dispatcherCoordinator;
        _fileHandler = fileHandler;
    }

    public string CustomerCabCall(string customerName)
    {
        
        dispatcherCoordinator.CustomerCabCall(customerName);
        var exportedCustomerList = dispatcherCoordinator.ExportCustomerList();
        string[] exportedCustomers = exportedCustomerList
            .Select(x => 
                $"{x.Key.Name}," +
                $"{x.Key.EndLocation}," +
                $"{x.Key.StartLocation}," +
                $"{x.Value}"
            ).ToArray();
        _fileHandler.WriteCustomerList(exportedCustomers);
        return customerName;
    }
    public void CancelPickup()
    {
        dispatcherCoordinator.CancelPickup();
    }
    public string[] SendCabRequest()
    {
        try
        {
            dispatcherCoordinator.RideRequest();

            var cabInfo = dispatcherCoordinator.FindEnroutePassenger(CustomerStatus.WaitingPickup);
            
            return
            [
                $"{cabInfo?.CabName} picked up {cabInfo?.PassengerName} at {cabInfo?.StartLocation}.",
                "Cab assigned to customer."
            ];
        }
        catch (Exception ex)
        {
            throw new SystemException(ex.Message);
        }
    }
    public void PickupCustomer()
    {
        try
        {
            dispatcherCoordinator.PickupCustomer();
        }
        catch (Exception ex)
        {
            throw new SystemException(ex.Message);
        }
    }
    public List<CabInfo?> DropOffCustomer()
    {
        try
        {
            dispatcherCoordinator.DropOffCustomer();
            var customerInState = dispatcherCoordinator
                .RetrieveCustomerInState(CustomerStatus.Delivered);
            return [new CabInfo()
            {
                CabName = "Evan's Cab",
                StartLocation = customerInState.StartLocation,
                Destination = customerInState.EndLocation,
                PassengerName = customerInState.Name
            }];
        }
        catch (Exception ex)
        {
            throw new SystemException(ex.Message);
        }
    }
    public void AddCab(Cab cab)
    {
        dispatcherCoordinator.AddCab(cab);
        string[] cabList = dispatcherCoordinator.ExportCabList();
        _fileHandler.WriteCabList(cabList);
    }
    public void RemoveCab()
    {
        dispatcherCoordinator.RemoveCab();
    }
}

public interface IFileHandler : IFileReader, IFileWriter
{
}
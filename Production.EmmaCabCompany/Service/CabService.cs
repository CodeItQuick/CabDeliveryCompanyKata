using Production.EmmaCabCompany.Domain;

namespace Production.EmmaCabCompany.Service;

public class CabService
{
    private readonly DispatcherCoordinator _dispatcherCoordinator;
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
        var cabListStrings = cabList
            .Select(x => x)
            .ToList();
        List<Cab> cabStoredList = new List<Cab>();
        foreach (var cab in cabListStrings)
        {
            var cabAttributes = cab.Split(",");
            if (cabAttributes.Length < 1 || string.IsNullOrWhiteSpace(cabAttributes[0])) continue;
            var cabValue = new Cab(cabAttributes[0], 20);
            if (!string.IsNullOrWhiteSpace(cabAttributes[1]))
            {
                var customer = new Customer(cabAttributes[1], cabAttributes[2], cabAttributes[3]);
                cabValue.RequestRideFor(customer);
            }
            cabStoredList.Add(cabValue);
        }
        dispatcherCoordinator.RebuildCabList(cabStoredList);
        this._dispatcherCoordinator = dispatcherCoordinator;
        _fileHandler = fileHandler;
    }

    public string CustomerCabCall(string customerName)
    {
        
        _dispatcherCoordinator.CustomerCabCall(customerName);
        ExportPersistence();
        return customerName;
    }

    public void CancelPickup()
    {
        _dispatcherCoordinator.CancelPickup();
        ExportPersistence();
    }

    public string[] SendCabRequest()
    {
        try
        {
            _dispatcherCoordinator.RideRequest();

            var cabInfo = _dispatcherCoordinator.FindEnroutePassenger(CustomerStatus.WaitingPickup);
            
            ExportPersistence();
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
            _dispatcherCoordinator.PickupCustomer();
            ExportPersistence();
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
            _dispatcherCoordinator.DropOffCustomer();
            var customerInState = _dispatcherCoordinator
                .RetrieveCustomerInState(CustomerStatus.Delivered);
            ExportPersistence();
            return [new CabInfo()
            {
                CabName = "Evan's Cab",
                StartLocation = customerInState?.StartLocation,
                Destination = customerInState?.EndLocation,
                PassengerName = customerInState?.Name
            }];
        }
        catch (Exception ex)
        {
            throw new SystemException(ex.Message);
        }
    }

    public void AddCab(Cab cab)
    {
        _dispatcherCoordinator.AddCab(cab);
        ExportPersistence();
    }
    public void RemoveCab()
    {
        _dispatcherCoordinator.RemoveCab();
        ExportPersistence();
    }
    private void ExportPersistence()
    {
        var exportedCustomerList = _dispatcherCoordinator.ExportCustomerList();
        string[] exportedCustomers = exportedCustomerList
            .Select(x => 
                $"{x.Key.Name}," +
                $"{x.Key.StartLocation}," +
                $"{x.Key.EndLocation}," +
                $"{x.Value}"
            ).ToArray();
        _fileHandler.WriteCustomerList(exportedCustomers);
        string[] cabList = _dispatcherCoordinator.ExportCabList();
        _fileHandler.WriteCabList(cabList);
    }
}

public interface IFileHandler : IFileReader, IFileWriter
{
}
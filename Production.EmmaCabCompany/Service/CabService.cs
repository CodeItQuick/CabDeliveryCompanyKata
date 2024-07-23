using Production.EmmaCabCompany.Adapter.@in;
using Production.EmmaCabCompany.Domain;

namespace Production.EmmaCabCompany.Service;

public class CabService
{
    private readonly DispatcherCoordinator _dispatcherCoordinator;
    private readonly IFileHandler _fileHandler;

    public CabService(DispatcherCoordinator dispatcherCoordinator, IFileHandler handler)
    {
        var cabFileRepository = new CabFileRepository(handler);
        var customerDirectory = cabFileRepository.LoadedCustomerDirectory();
        dispatcherCoordinator.RebuildCustomerDictionary(customerDirectory);
        
        var loadedFleetState = cabFileRepository.LoadedFleetState();
        dispatcherCoordinator.RebuildCabList(loadedFleetState);
        
        _dispatcherCoordinator = dispatcherCoordinator;
        _fileHandler = handler;
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
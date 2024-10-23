using Production.EmmaCabCompany.Adapter.@in;
using Production.EmmaCabCompany.Adapter.@out.CabFileAdapter;
using Production.EmmaCabCompany.Domain;

namespace Production.EmmaCabCompany.Service;

public class CabService
{
    private readonly DispatcherCoordinator _dispatcherCoordinator;
    private CabFileRepository _cabFileRepository;

    public CabService(DispatcherCoordinator dispatcherCoordinator, CabFileRepository cabFileRepository)
    {
        _cabFileRepository = cabFileRepository;
        var customerDirectory = _cabFileRepository.LoadedCustomerDirectory();
        dispatcherCoordinator.RebuildCustomerDictionary(customerDirectory);
        
        var loadedFleetState = _cabFileRepository.LoadedFleetState();
        dispatcherCoordinator.RebuildCabList(loadedFleetState);
        
        _dispatcherCoordinator = dispatcherCoordinator;
    }

    public string? CustomerCabCall(string? customerName, string? startLocation, string? destinationLane)
    {
        var customer = new Customer(customerName, startLocation, destinationLane);
        _dispatcherCoordinator.CustomerCabCall(customer);
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
                $"{x.Value}," +
                $"{x.Key.PickupLocation.Item1}," +
                $"{x.Key.PickupLocation.Item2}"
            ).ToArray();
        _cabFileRepository.WriteCustomerList(exportedCustomers);
        string[] cabList = _dispatcherCoordinator.ExportCabList();
        _cabFileRepository.WriteCabList(cabList);
    }
}
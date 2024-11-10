using Production.EmmaCabCompany.Adapter.@in;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Application;
using Production.EmmaCabCompany.Domain;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Production.EmmaCabCompany.Service;

public class CabServiceHandler
{
    private readonly DispatcherCoordinator _dispatcherCoordinator;
    private CabFileRepository _cabFileRepository;

    public CabServiceHandler(DispatcherCoordinator dispatcherCoordinator, CabFileRepository cabFileRepository)
    {
        _cabFileRepository = cabFileRepository;
        _dispatcherCoordinator = dispatcherCoordinator;
    }

    public string? CustomerCabCall(string? customerName, string? startLocation, string? destinationLane)
    {
        var customerDirectory = _cabFileRepository.RetrieveCustomerDirectory();
        _dispatcherCoordinator.RebuildCustomerDictionary(customerDirectory);
        var loadedFleetState = _cabFileRepository.RetrieveFleet();
        _dispatcherCoordinator.RebuildCabList(loadedFleetState);
        
        var customer = new Customer(customerName, startLocation, destinationLane);
        _dispatcherCoordinator.CustomerCabCall(customer);
        ExportPersistence();
        return customerName;
    }

    public void CancelPickup()
    {
        var customerDirectory = _cabFileRepository.RetrieveCustomerDirectory();
        _dispatcherCoordinator.RebuildCustomerDictionary(customerDirectory);
        var loadedFleetState = _cabFileRepository.RetrieveFleet();
        _dispatcherCoordinator.RebuildCabList(loadedFleetState);

        _dispatcherCoordinator.CancelPickup();
        ExportPersistence();
    }

    public string[] SendCabRequest()
    {
        try
        {
            var customerDirectory = _cabFileRepository.RetrieveCustomerDirectory();
            _dispatcherCoordinator.RebuildCustomerDictionary(customerDirectory);
            var loadedFleetState = _cabFileRepository.RetrieveFleet();
            _dispatcherCoordinator.RebuildCabList(loadedFleetState);

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
            var customerDirectory = _cabFileRepository.RetrieveCustomerDirectory();
            _dispatcherCoordinator.RebuildCustomerDictionary(customerDirectory);
            var loadedFleetState = _cabFileRepository.RetrieveFleet();
            _dispatcherCoordinator.RebuildCabList(loadedFleetState);

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
            var customerDirectory = _cabFileRepository.RetrieveCustomerDirectory();
            _dispatcherCoordinator.RebuildCustomerDictionary(customerDirectory);
            var loadedFleetState = _cabFileRepository.RetrieveFleet();
            _dispatcherCoordinator.RebuildCabList(loadedFleetState);

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
        var customerDirectory = _cabFileRepository.RetrieveCustomerDirectory();
        _dispatcherCoordinator.RebuildCustomerDictionary(customerDirectory);
        var loadedFleetState = _cabFileRepository.RetrieveFleet();
        _dispatcherCoordinator.RebuildCabList(loadedFleetState);
        
        // add the cab to the domain
        _dispatcherCoordinator.AddCab(cab);
        
        
        // export the cab list with the new cab
        ExportPersistence();
    }
    public void RemoveCab()
    {
        var customerDirectory = _cabFileRepository.RetrieveCustomerDirectory();
        _dispatcherCoordinator.RebuildCustomerDictionary(customerDirectory);
        var loadedFleetState = _cabFileRepository.RetrieveFleet();
        _dispatcherCoordinator.RebuildCabList(loadedFleetState);

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
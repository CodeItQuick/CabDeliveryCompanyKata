using Production.EmmaCabCompany.Domain;

namespace Production.EmmaCabCompany;

public class CabService(DispatcherCoordinator dispatcherCoordinator)
{
    public string CustomerCabCall(string customerName)
    {
        
        dispatcherCoordinator.CustomerCabCall(customerName);
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
            return dispatcherCoordinator.DroppedOffCustomer();
        }
        catch (Exception ex)
        {
            throw new SystemException(ex.Message);
        }
    }

    public void AddCab(Cab cab)
    {
        dispatcherCoordinator.AddCab(cab);
    }

    public void RemoveCab()
    {
        dispatcherCoordinator.RemoveCab();
    }
}
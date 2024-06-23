namespace Production.EmmaCabCompany.Domain;

public class Dispatch 
{
    
    private readonly Fleet _fleet = new();

    public void AddCab(Cab cab)
    {
        _fleet.AddCab(cab);
    }

    public void RemoveCab()
    {
        if (_fleet.AllCabsOccupied())
        {
            throw new SystemException("Cab cannot be removed until passenger dropped off.");
        }
        _fleet.RemoveCab();
    }

    public void RideRequest(Customer? customer)
    {
        _fleet.RideRequested(customer);
    }

    public CabInfo? FindEnroutePassenger(Customer customer)
    {
        
        if (_fleet.IsEnroute(customer))
        {
            return new CabInfo()
            {
                PassengerName = customer.name,
                CabName = _fleet.FindCab(customer),
                StartLocation = customer.startLocation,
                Destination = customer.endLocation
            };
        }

        return null;
    }

    public void PickupCustomer(Customer customer)
    {
        _fleet.PickupCustomer(customer);
    }

    public void DropOffCustomer()
    {
        _fleet.DropOffCustomer();
    }
    

    public bool NoCabsInFleet()
    {
        return _fleet.NoCabsInFleet();
    }
    public bool CustomersStillInTransport()
    {
        return _fleet.CustomersStillInTransport();
    }

    public List<CabInfo> DroppedOffCustomers()
    {
        var droppedOff = _fleet.LastAssigned();
        return new List<CabInfo>() { droppedOff };
    }
}
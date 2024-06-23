namespace Production.EmmaCabCompany;

public class Fleet 
{
    private readonly List<Cab> _fleet = new();
    private bool _rideRequested;
    private CabInfo? _lastAssignedCab;

    public void AddCab(Cab cab)
    {
        _fleet.Add(cab);
    }

    public void RemoveCab()
    {
        if (_fleet[^1].RideInProgress())
        {
            _fleet.RemoveAt(_fleet.Count - 1);
        }

    }
    public void RideRequested(Customer? customer)
    {
        _rideRequested = false;
        _lastAssignedCab = null;
        for (int i = 0; i < _fleet.Count; i++)
        {
            if (_rideRequested == false)
            {
                var rideRequest = _fleet[i].RequestRideFor(customer);
                if (rideRequest == true)
                {
                    _rideRequested = true;
                    _lastAssignedCab = _fleet[i].CabInfo();
                }
            }
        }
    }

    public CabInfo? LastAssigned()
    {
        return _lastAssignedCab;
    }
}
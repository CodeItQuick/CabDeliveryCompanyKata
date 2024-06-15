namespace Production.EmmaCabCompany;

public class Dispatch 
{
    private readonly ICabCompanyPrinter _cabCompanyPrinter;
    private List<ICabs> _fleet = new List<ICabs>();

    public Dispatch(ICabCompanyPrinter cabCompanyPrinter)
    {
        _cabCompanyPrinter = cabCompanyPrinter;
    }

    // Dispatcher?
    public void AddCab(ICabs cab)
    {
        _fleet.Add(cab);
    }

    public void CallCab(Customer customer)
    {
        for (int i = 0; i < _fleet.Count; i++)
        {
            var success = _fleet[i].PickupCustomer(customer);
            if (success == true)
            {
                _fleet[i].DropOffCustomer();
            }
            
        }
    }

    public void RideRequest(Customer customer)
    {
        var rideRequested = false;
        for (int i = 0; i < _fleet.Count; i++)
        {
            if (rideRequested == false)
            {
                var rideRequest = _fleet[i].RideRequest(customer);
                if (rideRequest == true)
                {
                    rideRequested = true;
                }
            }
        }

        if (rideRequested == false)
        {
            _cabCompanyPrinter.WriteLine($"Dispatch failed to pickup {customer.name} as there are no available cabs.");
        }
    }
}
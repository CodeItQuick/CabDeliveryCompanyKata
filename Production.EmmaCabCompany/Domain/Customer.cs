namespace Production.EmmaCabCompany;

public class Customer
{
    public readonly string? Name;
    public readonly string? StartLocation;
    public readonly string? EndLocation;
    private Cab? _currentCab;

    public Customer(string? customerName, string? startLocation, string? endLocation)
    {
        Name = customerName;
        this.StartLocation = startLocation;
        this.EndLocation = endLocation;
    }

    public bool IsPickedUp()
    {
        return _currentCab != null;
    }

    public void IsInCab(Cab cab)
    {
        _currentCab = cab;
    }

    // TODO: Not tested directly yet
    public void ExitCab()
    {
        _currentCab = null;
    }
}
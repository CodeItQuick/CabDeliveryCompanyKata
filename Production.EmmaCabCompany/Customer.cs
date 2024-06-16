namespace Production.EmmaCabCompany;

public class Customer
{
    public string name;
    public string startLocation;
    public string endLocation;
    private Cab? currentCab;

    public Customer(string customerName, string startLocation, string endLocation)
    {
        name = customerName;
        this.startLocation = startLocation;
        this.endLocation = endLocation;
    }

    public bool IsPickedUp()
    {
        return currentCab != null;
    }

    public void IsInCab(Cab cab)
    {
        currentCab = cab;
    }

    // TODO: Not tested directly yet
    public void ExitCab()
    {
        currentCab = null;
    }
}
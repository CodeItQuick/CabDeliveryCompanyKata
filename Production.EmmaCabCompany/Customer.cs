namespace Production.EmmaCabCompany;

public class Customer
{
    public string CustomerName { get; }
    public string StartLocation { get; }
    public string EndLocation { get; }
    public bool IsInCab { get; private set; } = false;
    public string CabName { get; private set; } = "";

    public int Wallet { get; set; }
    public RideHistory RideHistory { get; set; } = new RideHistory();

    public Customer(string customerName, string startLocation, string endLocation, int wallet)
    {
        CustomerName = customerName;
        StartLocation = startLocation;
        EndLocation = endLocation;
        Wallet = wallet;
    }

    public void TakeCab(string cabName)
    {
        IsInCab = true;
        CabName = cabName;
        RideHistory.NameOfCabsTaken.Add(cabName);
        
    }

    public void PayCabbie(int fare)
    {
        Wallet -= fare;
        IsInCab = false;
    }
}

public class RideHistory
{
    public List<string> NameOfCabsTaken { get; set; } = new();
}
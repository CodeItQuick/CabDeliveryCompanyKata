namespace Production.EmmaCabCompany;

public class Customer
{
    public string CustomerName { get; set; }
    public string StartLocation { get; set; }
    public string EndLocation { get; set; }
    public bool IsInCab { get; set; } = false;
    public string CabName { get; set; } = "";

    public int Wallet { get; set; }
    public RideHistory RideHistory { get; set; } = new RideHistory();

    public Customer(string customerName, string startLocation, string endLocation, int wallet)
    {
        CustomerName = customerName;
        StartLocation = startLocation;
        EndLocation = endLocation;
        Wallet = wallet;
    }
}

public class RideHistory
{
    public List<string> NameOfCabsTaken { get; set; } = new();
}
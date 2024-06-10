namespace Production.EmmaCabCompany;

public class Customer
{
    private IFileReader _fileReader = new FileReader();
    private IFileWriter _fileWriter = new FileWriter();
    public string CustomerName { get; }
    public string StartLocation { get; }
    public string EndLocation { get; }
    public bool IsInCab { get; private set; } = false;
    public string CabName { get; private set; } = "";

    public int Wallet { get; set; }
    public RideHistory RideHistory { get; set; } = new RideHistory();

    public Customer(
        string customerName, string startLocation, string endLocation, 
        int wallet, IFileWriter fileWriter, IFileReader fileReader)
    {
        CustomerName = customerName;
        StartLocation = startLocation;
        EndLocation = endLocation;
        Wallet = wallet;
        _fileWriter = fileWriter;
        _fileReader = fileReader;
    }

    public void TakeCab(string cabName)
    {
        IsInCab = true;
        CabName = cabName;
        RideHistory.NameOfCabsTaken.Add(cabName);
        _fileWriter.Write("dummyfile.csv", RideHistory.NameOfCabsTaken.ToArray());
    }

    public string[] RetrieveRideHistory()
    {
        return _fileReader.Read("dummyfile.csv");
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
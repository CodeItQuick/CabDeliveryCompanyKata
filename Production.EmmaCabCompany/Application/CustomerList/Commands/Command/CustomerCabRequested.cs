namespace Production.EmmaCabCompany.Application.CustomerList.Commands.Command;

public class CustomerCabRequested
{
    public CustomerCabRequested(string customerName, string startLocation, string endLocation)
    {
        CustomerName = customerName;
        StartLocation = startLocation;
        EndLocation = endLocation;
    }

    public string CustomerName { get; init; }
    public string StartLocation { get; init; }
    public string EndLocation { get; init; }
}
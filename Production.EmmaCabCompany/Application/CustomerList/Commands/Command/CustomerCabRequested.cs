namespace Production.EmmaCabCompany.Application.CustomerList.Commands.Command;

public class CustomerCabRequested : IEvent
{
    public CustomerCabRequested(string customerName, string startLocation, string endLocation, int customerId)
    {
        CustomerId = customerId;
        CustomerName = customerName;
        StartLocation = startLocation;
        EndLocation = endLocation;
    }

    public int CustomerId { get;  }

    public string CustomerName { get; init; }
    public string StartLocation { get; init; }
    public string EndLocation { get; init; }
}
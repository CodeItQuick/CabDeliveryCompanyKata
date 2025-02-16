namespace Production.EmmaCabCompany.Application.CustomerList.Commands.Command;

public class CustomerRideRequested : IEvent
{
    public int CustomerListId { get; set; }
}
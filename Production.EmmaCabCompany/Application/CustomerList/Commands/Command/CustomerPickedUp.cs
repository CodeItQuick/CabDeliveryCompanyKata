namespace Production.EmmaCabCompany.Application.CustomerList.Commands.Command;

public class CustomerPickedUp : IEvent
{
    public int CustomerListId { get; set; }
}
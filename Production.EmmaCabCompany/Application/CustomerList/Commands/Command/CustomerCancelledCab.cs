namespace Production.EmmaCabCompany.Application.CustomerList.Commands.Command;

public class CustomerCancelledCab : IEvent
{
    public int CustomerListId { get; set; }
}
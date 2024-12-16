namespace Production.EmmaCabCompany.Application.CustomerList.Commands.Command;

public class CustomerDelivered : IEvent
{
    public int CustomerListId { get; set; }
}
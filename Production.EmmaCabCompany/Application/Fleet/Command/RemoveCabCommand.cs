namespace Production.EmmaCabCompany.Application.Fleet;

public class RemoveCabCommand(int userId) : IEvent
{
    public int UserId { get; } = userId;
}
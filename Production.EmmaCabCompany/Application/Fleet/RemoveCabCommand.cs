namespace Production.EmmaCabCompany.Application.Fleet;

public class RemoveCabCommand(int fleetId) : IEvent
{
    public int FleetId { get; init; } = fleetId;
}
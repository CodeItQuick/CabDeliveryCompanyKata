namespace Production.EmmaCabCompany.Application.Fleet;

public class RemoveCabCommand(int fleetId)
{
    public int FleetId { get; init; } = fleetId;
}
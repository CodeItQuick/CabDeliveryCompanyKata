namespace Production.EmmaCabCompany.Application;

public class RemoveCabCommand(int fleetId)
{
    public int FleetId { get; init; } = fleetId;
}
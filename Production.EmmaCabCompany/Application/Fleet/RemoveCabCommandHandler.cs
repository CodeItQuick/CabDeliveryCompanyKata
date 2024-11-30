namespace Production.EmmaCabCompany.Application;

public class RemoveCabCommandHandler : IRemoveCabCommandHandler
{
    private readonly IFleetRepository _fleetRepository;

    public RemoveCabCommandHandler(IFleetRepository fleetRepository)
    {
        _fleetRepository = fleetRepository;
    }

    // this should be "Handle"
    public void Handle(RemoveCabCommand addCabCommand)
    {
        _fleetRepository.RemoveCab(addCabCommand.FleetId);
    }
}

public interface IRemoveCabCommandHandler
{
    public void Handle(RemoveCabCommand addCabCommand);
}
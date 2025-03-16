namespace Production.EmmaCabCompany.Application.Fleet;

public class RemoveCabCommandCommandHandler : IRemoveCabCommandCommandHandler
{
    private readonly IFleetRepository _fleetRepository;

    public RemoveCabCommandCommandHandler(IFleetRepository fleetRepository)
    {
        _fleetRepository = fleetRepository;
    }

    public void Handle(RemoveCabCommand addCabCommand)
    {
        _fleetRepository.Remove(addCabCommand.FleetId);
    }
}

public interface IRemoveCabCommandCommandHandler : ICommandHandler<RemoveCabCommand>;
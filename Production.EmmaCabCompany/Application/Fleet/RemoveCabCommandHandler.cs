namespace Production.EmmaCabCompany.Application.Fleet;

public class RemoveCabCommandHandler : IRemoveCabCommandHandler
{
    private readonly IFleetRepository _fleetRepository;

    public RemoveCabCommandHandler(IFleetRepository fleetRepository)
    {
        _fleetRepository = fleetRepository;
    }

    public void Handle(RemoveCabCommand addCabCommand)
    {
        _fleetRepository.RemoveCab(addCabCommand.FleetId);
    }

    public int Handle<TS>(TS @event) where TS : RemoveCabCommand
    {
        _fleetRepository.RemoveCab(@event.FleetId);
        return @event.FleetId;
    }
}

public interface IRemoveCabCommandHandler : IHandler<RemoveCabCommand>;
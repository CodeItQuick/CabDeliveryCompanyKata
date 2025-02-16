namespace Production.EmmaCabCompany.Application.Fleet;

public class AddCabCommandHandler : IAddCommandHandler
{
    private readonly IFleetRepository _fleetRepository;

    public AddCabCommandHandler(IFleetRepository fleetRepository)
    {
        _fleetRepository = fleetRepository;
    }

    public int Handle<TS>(TS request) where TS : AddCabCommand
    {
        _fleetRepository.AddCab(request.CabName, request.Latitude, request.Longitude);
        return 1;
    }
}


public interface IAddCommandHandler : IHandler<AddCabCommand>;
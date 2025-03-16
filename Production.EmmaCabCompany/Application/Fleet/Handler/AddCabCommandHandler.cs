namespace Production.EmmaCabCompany.Application.Fleet;

public class AddCabCabCommandHandler : IAddCabCommandHandler
{
    private readonly IFleetRepository _fleetRepository;

    public AddCabCabCommandHandler(IFleetRepository fleetRepository)
    {
        _fleetRepository = fleetRepository;
    }

    public void Handle(AddCabCommand request)
    {
        _fleetRepository.AddCab(request.CabName, request.Latitude, request.Longitude);
    }
}

public interface IAddCabCommandHandler : ICommandHandler<AddCabCommand>;
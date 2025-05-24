using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;

namespace Production.EmmaCabCompany.Application;

public class AddCabCommandHandler : IAddCabCommandHandler
{
    private readonly IFleetRepository _fleetRepository;

    public AddCabCommandHandler(IFleetRepository fleetRepository)
    {
        _fleetRepository = fleetRepository;
    }

    // this should be "Handle"
    public void Handle(AddCabCommand addCabCommand)
    {
        _fleetRepository.AddCab(addCabCommand.CabName, addCabCommand.Latitude, addCabCommand.Longitude);
    }
}

public interface IAddCabCommandHandler
{
    public void Handle(AddCabCommand addCabCommand);
}
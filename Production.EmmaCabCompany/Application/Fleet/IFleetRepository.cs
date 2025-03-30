using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Domain.Fleet;

namespace Production.EmmaCabCompany.Application.Fleet;

public interface IFleetRepository
{
    void Save(Cab cab);
    void Save(Adapter.OutAdapter.CabFileAdapter.Fleet.Fleet fleet);
    void Save(FleetCoordinator fleetCoordinator);
    void Remove(int fleetId);
    Adapter.OutAdapter.CabFileAdapter.Fleet.Fleet GetFleetById(Adapter.OutAdapter.CabFileAdapter.Fleet.Fleet fleet);
}
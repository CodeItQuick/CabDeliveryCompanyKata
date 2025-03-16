using Production.EmmaCabCompany.Domain.Fleet;

namespace Production.EmmaCabCompany.Application.Fleet;

public interface IFleetRepository
{
    void Save(Cab cab);
    void Remove(int fleetId);
}
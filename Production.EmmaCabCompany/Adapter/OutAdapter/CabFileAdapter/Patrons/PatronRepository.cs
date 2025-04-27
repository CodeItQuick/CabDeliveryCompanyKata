using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Application.CustomerList;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Patrons;

public class PatronRepository : IPatronRepository
{
    private CabContext _cabContext;

    public PatronRepository(CabContext cabContext)
    {
        _cabContext = cabContext;
    }

    public Patron GetById(int? customerId)
    {
        var patron = _cabContext.Patrons
            .Include(x => x.Customer)
            .FirstOrDefault(x => x.Id == customerId)!;
        return new Patron(patron.Name, patron.StartLocation, patron.EndLocation)
        {
            Id = patron.Id, CustomerId = patron.Customer.Id
        };
    }

    public void Save(Patron entity)
    {
        var fleetCoordinator = _cabContext.Customers
            .FirstOrDefault(x => x.Id == entity.CustomerId);
        var patronDto = new PatronDto()
        {
            Id = entity.Id, Name = entity.Name, Status = entity.Status, 
            EndLocation = entity.EndLocation, StartLocation = entity.StartLocation,
            Customer = fleetCoordinator
        };
        if (patronDto.Id != null)
        {
            _cabContext.Patrons.Update(patronDto);
        }
        else
        {
            _cabContext.Patrons.Add(patronDto);
        }
        _cabContext.SaveChanges();
        _cabContext.ChangeTracker.Clear();
    }

    public void Remove(int entityId)
    {
        throw new NotImplementedException();
    }

}
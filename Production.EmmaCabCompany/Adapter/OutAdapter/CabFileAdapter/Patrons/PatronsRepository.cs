using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Application.CustomerList;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Patrons;

public class PatronsRepository : IPatronsRepository
{
    private CabContext _cabContext;

    public PatronsRepository(CabContext cabContext)
    {
        _cabContext = cabContext;
    }

    public PatronList GetById(int? customerId)
    {
        var patrons = _cabContext.Patrons
            .Include(x => x.Customer)
            .Where(x => x.Customer.Id == customerId)
            .Select(x => new Patron(x.Name, x.StartLocation, x.EndLocation)
            {
                Id = x.Id, CustomerId = x.Customer.Id, Status = x.Status
            })
            .ToList();
        _cabContext.ChangeTracker.Clear();
        return new PatronList() { Patrons = patrons };
    }

    public void Save(PatronList entity)
    {
        _cabContext.Patrons.AddRange(entity.Patrons.Select(x => new PatronDto()
        {
            Customer = new Customer() { Id = x.CustomerId!.Value }, Id = x.Id, EndLocation = x.EndLocation, 
            Name = x.Name, Status = x.Status,
            PickupLocation = x.PickupLocation, StartLocation = x.StartLocation
        }));
        _cabContext.SaveChanges();
    }

    public void Remove(int entityId)
    {
        throw new NotImplementedException();
    }
}
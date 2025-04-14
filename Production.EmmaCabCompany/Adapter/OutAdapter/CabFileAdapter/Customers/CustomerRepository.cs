using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Application;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Customers;

public class CustomerRepository : ICustomerRepository, IDisposable, IAsyncDisposable
{
    private CabContext _cabContext;

    public CustomerRepository(CabContext cabContext)
    {
        _cabContext = cabContext;
    }

    public Domain.Customers.Customer GetById(int customerId)
    {
        var customer = _cabContext.Customers
            .FirstOrDefault(x => x.Id == customerId)!;
        return new Domain.Customers.Customer() { Id = customer.Id };
    }

    public void Save(Domain.Customers.Customer entity)
    {
        _cabContext.Customers.Add(new Customer());
        _cabContext.SaveChanges();
    }

    public void Remove(int entityId)
    {
        _cabContext.Customers.Remove(new Customer() { Id = entityId });
        _cabContext.SaveChanges();
    }

    public void Dispose()
    {
        _cabContext.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _cabContext.DisposeAsync();
    }
}
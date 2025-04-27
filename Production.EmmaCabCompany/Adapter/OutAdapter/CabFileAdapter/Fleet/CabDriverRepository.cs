using System.Text;
using Microsoft.EntityFrameworkCore;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Application.Fleet;
using Production.EmmaCabCompany.Domain.Fleet;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;

public class CabDriverRepository : ICabDriverRepository, IDisposable
{
    private CabContext _cabContext;

    public CabDriverRepository(CabContext cabContext)
    {
        _cabContext = cabContext;
    }

    public void Save(Cab entity)
    {
        var hasSetStatus = CabStatus.TryParse(entity._status.ToString(), out CabStatus adapterStatus);
        var customer = _cabContext.Customers.Find(entity.CustomerId);
        var cabDriver = new CabDriver()
        {
            Id = entity.Id,
            Customer = customer,
            _cabName = entity._cabName,
            _latitude = entity._latitude,
            _longitude = entity._longitude,
            _status = hasSetStatus ? adapterStatus : CabStatus.Available
        };
        if (cabDriver.Id.HasValue)
        {
            _cabContext.CabDrivers.Update(cabDriver);
        }
        else
        {
            // _cabContext.ChangeTracker.DetectChanges();
            // _cabContext.Customers.Attach(customer);
            _cabContext.CabDrivers.Add(cabDriver);
            // _cabContext.ChangeTracker.DetectChanges();
        }
        _cabContext.SaveChanges();
    }

    public void Remove(int entityId)
    {
        try
        {
            var cab = _cabContext.CabDrivers
                .Include(x => x.Customer)
                .FirstOrDefault(x => x.Customer.Id == entityId);
            if (cab!.IsStatus(CabStatus.Available))
            {
                cab.Customer = null;
                _cabContext.CabDrivers.Remove(cab);
            }
            else
            {
                throw new ArgumentNullException();
            }
            _cabContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Cab cannot be removed until passenger dropped off.");
        }
    }

    public Cab GetById(int? customerId)
    {
        var cabDriver = _cabContext.CabDrivers
            .Include(fleet => fleet.Customer)
            .FirstOrDefault(x => x.Id == customerId);
        _cabContext.ChangeTracker.Clear();
        return new Cab(cabDriver._cabName, cabDriver._wallet, cabDriver._latitude, cabDriver._longitude);
    }

    public void Dispose()
    {
        _cabContext.Dispose();
    }

    public async Task StreamToFile(string filename, int customerId)
    {
        await using var fileStream = File.Create(filename);
        IQueryable<CabDriver> query = _cabContext.CabDrivers.Where(x => x.Customer.Id == customerId);
        await query.ForEachAsync(x =>
        {
            var unicodeEncoding = new UTF8Encoding();
            byte[] result = unicodeEncoding.GetBytes($"{x.Id}, {x._cabName}, {x._latitude}, {x._longitude}\n");
            if (fileStream.CanSeek)
            {
                fileStream.Seek(0, SeekOrigin.End);
            }

            if (fileStream.CanWrite)
            {
                fileStream.WriteAsync(result, 0, result.Length);
            }
        });
        fileStream.Flush();
    }
}
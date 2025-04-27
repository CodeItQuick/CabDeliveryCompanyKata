using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;

namespace Tests.CabDeliveryCompanyKata;

public class FakeDatabaseRepository<T> : IDatabaseRepository<T> where T : IdentityClass
{
    private readonly Dictionary<int, T> _records = new();

    public void Save(T entity)
    {
        entity.Id = _records.Count + 1;
        _records.Add(entity.Id, entity);
    }
    public void Update(int recordId, T updatedRecord)
    {
        updatedRecord.Id = _records.Keys.First(x => x == recordId);
        _records[recordId] = updatedRecord;
    }
    public void Delete(int recordId)
    {
        _records.Remove(recordId);
    }
    public List<T> Read()
    {
        return _records.Values
            .ToList();
    }
    public List<T> Search(Func<T, bool> filter)
    {
        return _records.Values
            .Where(filter)
            // .Order(orders) ?? how to orderby?
            .ToList();
    }
    public T GetById(int recordId)
    {
        return _records[recordId];
    }
}

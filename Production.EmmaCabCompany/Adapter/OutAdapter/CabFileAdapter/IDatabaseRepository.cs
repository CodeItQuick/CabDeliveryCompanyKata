namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;

public interface IDatabaseRepository<T> where T : IdentityClass
{
    public T GetById(int recordId);
    public void Create(T entity);
    public void Delete(int recordId);
    // Bad idea below?
    // public void Update(int recordId, T updatedRecord);
    // public List<T> Read();
    // public List<T> Search(Func<T, bool> filter);
}

public class IdentityClass
{
    public virtual int Id { get; set; }
}
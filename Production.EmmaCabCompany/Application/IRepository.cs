namespace Production.EmmaCabCompany.Application;

public interface IRepository<T>
{
    void Save(T entity);
    void Remove(int entityId);
    T GetById(int customerId);
}
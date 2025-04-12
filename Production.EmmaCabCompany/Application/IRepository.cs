namespace Production.EmmaCabCompany.Application;

public interface IRepository<T> where T : new()
{
    void Save(T entity);
    void Remove(int entityId);
    T GetById(T entity);
}
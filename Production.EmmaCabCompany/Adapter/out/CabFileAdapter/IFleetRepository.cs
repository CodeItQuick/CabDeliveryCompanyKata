namespace Production.EmmaCabCompany.Adapter.@out.CabFileAdapter;

public interface IFleetRepository
{
    void AddCab(string cabName, double latitude, double longitude);
}
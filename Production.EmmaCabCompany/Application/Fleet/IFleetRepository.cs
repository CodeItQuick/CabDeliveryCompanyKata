namespace Production.EmmaCabCompany.Application;

public interface IFleetRepository
{
    void AddCab(string cabName, double latitude, double longitude);
    void RemoveCab(int fleetId);
}
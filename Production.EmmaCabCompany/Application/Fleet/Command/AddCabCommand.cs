namespace Production.EmmaCabCompany.Application.Fleet;

public class AddCabCommand(string cabName, double latitude, double longitude, int userId) : IEvent
{
    public string CabName { get; } = cabName;
    public double Latitude { get; } = latitude;
    public double Longitude { get; } = longitude;
    public int UserId { get; } = userId;
}
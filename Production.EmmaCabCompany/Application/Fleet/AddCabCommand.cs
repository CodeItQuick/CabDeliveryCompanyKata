namespace Production.EmmaCabCompany.Application.Fleet;

public class AddCabCommand(string cabName, double latitude, double longitude)
{
    public string CabName { get; init; } = cabName;
    public double Latitude { get; init; } = latitude;
    public double Longitude { get; init; } = longitude;
}
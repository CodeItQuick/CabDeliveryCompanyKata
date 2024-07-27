namespace Production.EmmaCabCompany;

public class Customer
{
    public readonly string? Name;
    public readonly string? StartLocation;
    public readonly string? EndLocation;
    public (double, double) PickupLocation;

    public Customer(string? customerName, string? startLocation, string? endLocation)
    {
        Name = customerName;
        StartLocation = startLocation;
        EndLocation = endLocation;
        PickupLocation = (46.5556, 63.1311);
    }

}
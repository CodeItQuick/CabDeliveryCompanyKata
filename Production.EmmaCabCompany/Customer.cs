namespace Production.EmmaCabCompany;

public class Customer
{
    public string name;
    public string startLocation;
    public string endLocation;

    public Customer(string customerName, string startLocation, string endLocation)
    {
        name = customerName;
        this.startLocation = startLocation;
        this.endLocation = endLocation;
    }
}
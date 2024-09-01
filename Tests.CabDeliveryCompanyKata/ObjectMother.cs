using Production.EmmaCabCompany;

namespace Tests.CabDeliveryCompanyKata;

public static class ObjectMother
{
    public static Customer CreateDefaultCustomer(
        string customerName = "Lisa", 
        string startLocation = "1 Fulton Drive", 
        string endLocation = "1 Destination Avenue")
    {
        return new Customer(
            customerName,
            startLocation, 
            endLocation);
    }

    public static Cab CreateDefaultCab(
        string cabName = "Evan's Cab",
        int wallet = 20,
        double latitude = 46.2382,
        double longitude = 63.1311)
    {
        return new Cab(
            cabName,
            wallet,
            latitude,
            longitude);
    }
}
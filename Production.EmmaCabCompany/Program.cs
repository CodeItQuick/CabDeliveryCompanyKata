// See https://aka.ms/new-console-template for more information
namespace Production.EmmaCabCompany;
public class Program
{
    public static void Main(string[] args)
    {
        var dispatch = new Dispatcher();
        dispatch.RequestRide(new Customer());
    }
}
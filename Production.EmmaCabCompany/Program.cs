// See https://aka.ms/new-console-template for more information
namespace Production.EmmaCabCompany;
public class Program
{
    public static void Main(string[] args)
    {
        var cabCompanyPrinter = new CabCompanyPrinter();
        var cabCompanyReader = new CabCompanyReader();

        var customerListFilename = $"customer_list_default{Guid.NewGuid()}.csv";
        var cabListFilename = $"cab_list_default{Guid.NewGuid()}.csv";
        var dispatch = new UserInterface(
            cabCompanyPrinter, 
            cabCompanyReader, new FileHandler(customerListFilename, cabListFilename));
        dispatch.Run();
    }
}
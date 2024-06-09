// See https://aka.ms/new-console-template for more information
namespace Production.EmmaCabCompany;
public class Program
{
    public static void Main(string[] args)
    {
        var cabCompanyPrinter = new CabCompanyPrinter();
        List<ICabs> cabs =
        [
            new Cabs("Evan's Cab", cabCompanyPrinter, 20),
            new Cabs("Dan's Cab", cabCompanyPrinter, 20)
        ];

        cabCompanyPrinter.WriteLine("Emma's cab company will pickup and deliver a customer.");
        var customers = new List<Customer>()
        {
            new("Darrell", "1 Fulton Drive", "1 University Avenue", 20)
        };
        EmmaCabCompany.CallCab(cabs, customers);
            
        cabCompanyPrinter.WriteLine("");
            
        cabCompanyPrinter.WriteLine("Emma's cab company will pickup and deliver a customer.");
        var customersTwo = new List<Customer>()
        {
            new("Darrell", "1 Fulton Drive", "1 University Avenue", 20),
            new("Diane", "2 Fulton Drive", "2 University Avenue", 20),
        };
        EmmaCabCompany.CallCab(cabs, customersTwo);
    }
}
// See https://aka.ms/new-console-template for more information
namespace Production.EmmaCabCompany;
public class Program
{
    public static void Main(string[] args)
    {
        List<ICabs> cabs =
        [
            new Cabs("Evan's Cab"),
            new Cabs("Dan's Cab")
        ];

        var cabCompanyPrinter = new CabCompanyPrinter();
        cabCompanyPrinter.WriteLine("Emma's cab company will pickup and deliver a customer.");
        EmmaCabCompany.CallCab(
            cabs, 
            new Customer("Darrell", "1 Fulton Drive", "1 University Avenue"),
            cabCompanyPrinter);
            
        cabCompanyPrinter.WriteLine("");
            
        cabCompanyPrinter.WriteLine("Emma's cab company will pickup and deliver a customer.");
        EmmaCabCompany.CallCab(
            cabs, 
            new Customer("Diane", "1 Fulton Drive", "1 University Avenue"),
            cabCompanyPrinter);
    }
}
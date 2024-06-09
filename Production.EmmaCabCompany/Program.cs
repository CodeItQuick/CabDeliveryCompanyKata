// See https://aka.ms/new-console-template for more information
namespace Production.EmmaCabCompany;
public class Program
{
    public static void Main(string[] args)
    {
        var cabCompanyPrinter = new CabCompanyPrinter();
        var cabCompanyWriter = new CabCompanyWriter();
        var cabsList = new List<ICabs>();
        var customers = new List<Customer>();
        
        REPL.Run(cabCompanyPrinter, cabCompanyWriter, cabsList, customers);
    }
}
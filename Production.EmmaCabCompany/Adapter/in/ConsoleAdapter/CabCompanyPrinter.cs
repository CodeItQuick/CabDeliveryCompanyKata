namespace Production.EmmaCabCompany.Adapter.@in.ConsoleAdapter;

public class CabCompanyPrinter : ICabCompanyPrinter
{
    public void WriteLine(string printStatement)
    {
        Console.WriteLine(printStatement);
    }
}
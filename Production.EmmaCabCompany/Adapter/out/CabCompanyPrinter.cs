namespace Production.EmmaCabCompany.Adapter.@out;

public class CabCompanyPrinter : ICabCompanyPrinter
{
    public void WriteLine(string printStatement)
    {
        Console.WriteLine(printStatement);
    }
}
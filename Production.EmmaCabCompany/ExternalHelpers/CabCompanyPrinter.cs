namespace Production.EmmaCabCompany;

public class CabCompanyPrinter : ICabCompanyPrinter
{
    public void WriteLine(string printStatement)
    {
        Console.WriteLine(printStatement);
    }
}
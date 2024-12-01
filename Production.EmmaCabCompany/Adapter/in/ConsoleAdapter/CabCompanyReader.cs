namespace Production.EmmaCabCompany.Adapter.@in.ConsoleAdapter;

public class CabCompanyReader : ICabCompanyReader
{
    public string? ReadLine()
    {
        return Console.ReadLine();
    }
}
namespace Production.EmmaCabCompany.Adapter.@out;

public class CabCompanyReader : ICabCompanyReader
{
    public string? ReadLine()
    {
        return Console.ReadLine();
    }
}
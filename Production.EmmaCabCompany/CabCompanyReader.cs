namespace Production.EmmaCabCompany;

public class CabCompanyReader : ICabCompanyWriter
{
    public string? ReadLine()
    {
        return Console.ReadLine();
    }
}
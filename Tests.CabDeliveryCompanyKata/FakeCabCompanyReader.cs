using Production.EmmaCabCompany;

namespace Tests.CabDeliveryCompanyKata;

public class FakeCabCompanyReader : ICabCompanyReader
{
    private int currentCommand = 0;
    public List<string> CommandList { get; set; } = new List<string>();
    public string? ReadLine()
    {
        var current = CommandList[currentCommand];
        currentCommand += 1;
        return current;
    }
}
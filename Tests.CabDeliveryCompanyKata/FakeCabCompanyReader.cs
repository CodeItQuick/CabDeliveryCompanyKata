using Production.EmmaCabCompany.Adapter.@in.ConsoleAdapter;

namespace Tests.CabDeliveryCompanyKata;

public class FakeCabCompanyReader : ICabCompanyReader
{
    private int currentCommand = 0;
    public List<string> CommandList { get; init; } = new List<string>();
    public string? ReadLine()
    {
        var current = CommandList[currentCommand];
        currentCommand += 1;
        return current;
    }
}
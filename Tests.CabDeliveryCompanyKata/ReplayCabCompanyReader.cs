using Production.EmmaCabCompany;

namespace Tests.CabDeliveryCompanyKata;

public class ReplayCabCompanyReader : ICabCompanyWriter
{
    private int currentCommand = 0;
    private List<string> CommandList { get; set; }

    public ReplayCabCompanyReader(List<string> commandList)
    {
        CommandList = commandList;
    }

    public string? ReadLine()
    {
        var current = CommandList[currentCommand];
        currentCommand += 1;
        return current;
    }
}
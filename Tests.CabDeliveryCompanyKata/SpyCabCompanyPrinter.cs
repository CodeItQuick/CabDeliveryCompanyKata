using Production.EmmaCabCompany.Adapter.@out;

namespace Tests.CabDeliveryCompanyKata;

public class SpyCabCompanyPrinter : ICabCompanyPrinter
{
    private List<string> Messages { get; set; } = new List<string>();

    public void WriteLine(string printStatement)
    {
        Messages.Add(printStatement);
    }

    public string Retrieve(int messageNumber)
    {
        return Messages[messageNumber];
    }

    public int CountMessages()
    {
        return Messages.Count;
    }

    public IEnumerable<string> List()
    {
        return Messages;
    }
}
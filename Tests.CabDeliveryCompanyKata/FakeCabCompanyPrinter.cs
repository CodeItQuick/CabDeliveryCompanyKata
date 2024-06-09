

using Production.EmmaCabCompany;

namespace Tests.CabDeliveryCompanyKata;

public class FakeCabCompanyPrinter : ICabCompanyPrinter
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
}
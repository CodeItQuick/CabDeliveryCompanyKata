using Production.EmmaCabCompany;

namespace Tests.CabDeliveryCompanyKata;

public class AcceptanceTests
{
    [Fact]
    public void TheCabCompanyCanPickupACustomerAtAnAddress()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        FakeCabCompanyReader cabCompanyReader = new FakeCabCompanyReader()
        {
            CommandList = new List<string>()
            {
                "1",
                "3",
                "5",
                "0"
            }
        };
        var customers = new List<Customer>();
        var cabsList = new List<ICabs>();
        REPL.Run(cabCompanyPrinter, cabCompanyReader, cabsList, customers);
        
        Assert.Single(customers);
        Assert.Single(cabsList);
        Assert.Equal("default 1 picked up default 1 at 1 Default Start Drive", cabCompanyPrinter.Retrieve(26));
        Assert.Equal("default 1 dropped off default 1 at 1 Default Final Drive", cabCompanyPrinter.Retrieve(27));
    }
    
}
using Production.EmmaCabCompany;

namespace Tests.CabDeliveryCompanyKata;

public class AcceptanceTests
{
    [Fact]
    public void TheCabCompanyCanPickupACustomerAtAnAddress()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        FakeCabCompanyWriter cabCompanyWriter = new FakeCabCompanyWriter()
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
        REPL.Run(cabCompanyPrinter, cabCompanyWriter, cabsList, customers);
        
        Assert.Single(customers);
        Assert.Single(cabsList);
        Assert.Equal("Evan's Cab picked up default customer 1 at start location 1.", cabCompanyPrinter.Retrieve(24));
        Assert.Equal("Evan's Cab dropped off default customer 1 at end location 1.", cabCompanyPrinter.Retrieve(25));
    }
    
}

public class FakeCabCompanyWriter : ICabCompanyWriter
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
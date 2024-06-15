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
                "6",
                "3",
                "4",
                "5",
                "0"
            }
        };
        var cabsList = new List<ICabs>();
        var mockFileReadWriter = new MockFileReadWriter();
        var dispatch = new Dispatch(cabCompanyPrinter, cabCompanyReader, mockFileReadWriter, mockFileReadWriter);
        dispatch.Run(cabsList);
        
        Assert.Single(cabsList);
        Assert.Equal("Evan's Cab picked up default customer 1 at start location 1.", cabCompanyPrinter.Retrieve(45));
        Assert.Equal("Evan's Cab dropped off default customer 1 at end location 1.", cabCompanyPrinter.Retrieve(46));
    }
    
}
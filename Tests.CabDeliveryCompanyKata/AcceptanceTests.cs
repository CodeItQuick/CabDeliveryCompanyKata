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
        var mockFileReadWriter = new MockFileReadWriter();
        var dispatch = new UserInterface(cabCompanyPrinter, cabCompanyReader, mockFileReadWriter, mockFileReadWriter);
        dispatch.Run();
        
        Assert.Equal("Evan's Cab picked up Emma at 1 Fulton Drive.", cabCompanyPrinter.Retrieve(36));
        Assert.Equal("Evan's Cab dropped off Emma at 1 Destination Lane.", cabCompanyPrinter.Retrieve(37));
    }
    
}
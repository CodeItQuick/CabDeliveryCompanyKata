using Production.EmmaCabCompany;

namespace Tests.CabDeliveryCompanyKata;

public class DomainTests
{
    [Fact]
    public void TheCabCompanyCanPickupACustomerAtAnAddress()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        List<ICabs> cabs =
        [
            new Cabs("Evan's Cab", cabCompanyPrinter, 20),
        ];

        EmmaCabCompany.CallCab(
            cabs,
            [new Customer("Darrell", "1 Fulton Drive", "1 University Avenue", 20, new MockFileReadWriter(), new FileReader())]);
        
        Assert.Equal("Evan's Cab picked up Darrell at 1 Fulton Drive", cabCompanyPrinter.Retrieve(0));
    }
    [Fact]
    public void TheCabCompanyCanDropOffACustomerAtAnAddress()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        List<ICabs> cabs =
        [
            new Cabs("Evan's Cab", cabCompanyPrinter, 20),
        ];

        EmmaCabCompany.CallCab(
            cabs,
            [new Customer("Diane", "2 Fulton Drive", "2 University Avenue", 20, new MockFileReadWriter(), new FileReader())]);
        
        Assert.Equal("Evan's Cab dropped off Diane at 2 University Avenue", cabCompanyPrinter.Retrieve(1));
    }
    [Fact]
    public void TheCabCompaniesRideHistoryIsStoredByTheCustomer()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        List<ICabs> cabs =
        [
            new Cabs("Evan's Cab", cabCompanyPrinter, 20),
        ];

        var mockFileWriter = new MockFileReadWriter();
        EmmaCabCompany.CallCab(
            cabs,
            [new Customer("Diane", "2 Fulton Drive", "2 University Avenue", 20, mockFileWriter, new FileReader())]);
        
        Assert.Equal("Evan's Cab dropped off Diane at 2 University Avenue", cabCompanyPrinter.Retrieve(1));
        Assert.Equal(1, mockFileWriter.CountLines());
        Assert.Equal("Evan's Cab", mockFileWriter.ReadLine(0));
        
    }
}
using Production.EmmaCabCompany;
namespace Tests.CabDeliveryCompanyKata;

public class AcceptanceTests
{
    [Fact]
    public void TheCabCompanyCanPickupACustomerAtAnAddress()
    {
        List<ICabs> cabs =
        [
            new Cabs("Evan's Cab"),
        ];
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();

        EmmaCabCompany.CallCab(
            cabs, 
            new Customer("Darrell", "1 Fulton Drive", "1 University Avenue"),
            cabCompanyPrinter);
        
        Assert.Equal("Emma's Cab picked up Darrell at 1 Fulton Drive", cabCompanyPrinter.Retrieve(0));
    }
    [Fact]
    public void TheCabCompanyCanDropOffACustomerAtAnAddress()
    {
        List<ICabs> cabs =
        [
            new Cabs("Evan's Cab"),
        ];
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();

        EmmaCabCompany.CallCab(
            cabs, 
            new Customer("Diane", "2 Fulton Drive", "2 University Avenue"),
            cabCompanyPrinter);
        
        Assert.Equal("Emma's Cab dropped off Diane at 2 University Avenue", cabCompanyPrinter.Retrieve(1));
    }
}
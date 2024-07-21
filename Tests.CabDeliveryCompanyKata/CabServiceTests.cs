using Production.EmmaCabCompany;
using Production.EmmaCabCompany.Domain;

namespace Tests.CabDeliveryCompanyKata;

public class CabServiceTests
{
    [Fact]
    public void CanPickupCustomer()
    {
        var fakeFileReadWriter = new FakeFileReadWriter();
        var cabService = new CabService(new DispatcherCoordinator(), fakeFileReadWriter, fakeFileReadWriter);

        var customerCabCall = cabService.CustomerCabCall("Emma");
        
        Assert.Equal("Emma", customerCabCall);
        Assert.Equal("Emma,1 Destination Lane,1 Fulton Drive,CustomerCallInProgress\n", fakeFileReadWriter.Read().First());
    }
    [Fact]
    public void CanPickupMultipleCustomers()
    {
        var fakeFileReadWriter = new FakeFileReadWriter();
        var cabService = new CabService(new DispatcherCoordinator(), fakeFileReadWriter, fakeFileReadWriter);

        var customerCabCall = cabService.CustomerCabCall("Emma");
        var customerCabCallTwo = cabService.CustomerCabCall("Lisa");
        
        Assert.Equal("Emma", customerCabCall);
        Assert.Equal("Lisa", customerCabCallTwo);
        Assert.Equal("Emma,1 Destination Lane,1 Fulton Drive,CustomerCallInProgress\n", 
            fakeFileReadWriter.Read().First());
        Assert.Equal("Lisa,1 Destination Lane,1 Fulton Drive,CustomerCallInProgress\n", 
            fakeFileReadWriter.Read().Skip(1).First());
    }
}
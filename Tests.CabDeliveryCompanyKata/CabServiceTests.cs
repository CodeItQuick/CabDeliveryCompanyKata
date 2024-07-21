using Production.EmmaCabCompany;
using Production.EmmaCabCompany.Domain;

namespace Tests.CabDeliveryCompanyKata;

public class CabServiceTests
{
    [Fact]
    public void CanAddACab()
    {
        var fakeFileReadWriter = new FakeFileReadWriter(
            "customer_list_default.csv", "cab_list_default.csv");
        var cabService = new CabService(new DispatcherCoordinator(), fakeFileReadWriter, fakeFileReadWriter);
        
        cabService.AddCab(new Cab("Evan", 20));
        
        Assert.Equal("Evan,,,\n", fakeFileReadWriter.Read("cab_list_default.csv").First());
    }
    [Fact]
    public void CanPickupCustomer()
    {
        var fakeFileReadWriter = new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv");
        var cabService = new CabService(new DispatcherCoordinator(), fakeFileReadWriter, fakeFileReadWriter);
        cabService.AddCab(new Cab("Evan", 20));
        
        var customerCabCall = cabService.CustomerCabCall("Emma");
        
        Assert.Equal("Emma", customerCabCall);
        Assert.Equal("Emma,1 Destination Lane,1 Fulton Drive,CustomerCallInProgress\n", 
            fakeFileReadWriter.Read("customer_list_default.csv").First());
    }
    [Fact]
    public void CanPickupMultipleCustomers()
    {
        var fakeFileReadWriter = new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv");
        var cabService = new CabService(new DispatcherCoordinator(), fakeFileReadWriter, fakeFileReadWriter);
        cabService.AddCab(new Cab("Evan", 20));
        
        var customerCabCall = cabService.CustomerCabCall("Emma");
        var customerCabCallTwo = cabService.CustomerCabCall("Lisa");
        
        Assert.Equal("Emma", customerCabCall);
        Assert.Equal("Lisa", customerCabCallTwo);
        Assert.Equal("Emma,1 Destination Lane,1 Fulton Drive,CustomerCallInProgress\n", 
            fakeFileReadWriter.Read("customer_list_default.csv").First());
        Assert.Equal("Lisa,1 Destination Lane,1 Fulton Drive,CustomerCallInProgress\n", 
            fakeFileReadWriter.Read("customer_list_default.csv").Skip(1).First());
    }
}
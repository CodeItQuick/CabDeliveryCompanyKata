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
        var cabService = new CabService(new DispatcherCoordinator(), fakeFileReadWriter);
        
        cabService.AddCab(new Cab("Evan", 20));
        
        Assert.Equal("Evan,,,\n", fakeFileReadWriter.Read("cab_list_default.csv").First());
    }
    [Fact]
    public void CanPickupCustomer()
    {
        var fakeFileReadWriter = new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv");
        var cabService = new CabService(new DispatcherCoordinator(), fakeFileReadWriter);
        cabService.AddCab(new Cab("Evan", 20));
        
        var customerCabCall = cabService.CustomerCabCall("Emma");
        
        Assert.Equal("Emma", customerCabCall);
        Assert.Equal("Emma,1 Fulton Drive,1 Destination Lane,CustomerCallInProgress", 
            fakeFileReadWriter.Read("customer_list_default.csv").First());
    }
    [Fact]
    public void CanPickupMultipleCustomers()
    {
        var fakeFileReadWriter = new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv");
        var cabService = new CabService(new DispatcherCoordinator(), fakeFileReadWriter);
        cabService.AddCab(new Cab("Evan", 20));
        
        var customerCabCall = cabService.CustomerCabCall("Emma");
        var customerCabCallTwo = cabService.CustomerCabCall("Lisa");
        
        Assert.Equal("Emma", customerCabCall);
        Assert.Equal("Lisa", customerCabCallTwo);
        Assert.Equal("Emma,1 Fulton Drive,1 Destination Lane,CustomerCallInProgress", 
            fakeFileReadWriter.Read("customer_list_default.csv").First());
        Assert.Equal("Lisa,1 Fulton Drive,1 Destination Lane,CustomerCallInProgress", 
            fakeFileReadWriter.Read("customer_list_default.csv").Skip(1).First());
    }
    [Fact]
    public void CanConstructPreviousState()
    {
        var fakeFileReadWriter = new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv");
        fakeFileReadWriter.Write("customer_list_default.csv",
            ["Emma,1 Destination Lane,1 Fulton Drive,CustomerCallInProgress\n"]);
        fakeFileReadWriter.Write("cab_list_default.csv", ["Evan,,,\n"]);
        var dispatcherCoordinator = new DispatcherCoordinator();
        
        var cabService = new CabService(dispatcherCoordinator, fakeFileReadWriter);
        
        Assert.Equal("Emma,1 Destination Lane,1 Fulton Drive,CustomerCallInProgress\n", fakeFileReadWriter.Read("customer_list_default.csv").First());
        Assert.Single(fakeFileReadWriter.Read("customer_list_default.csv"));
        Assert.Equal("Evan,,,\n", fakeFileReadWriter.Read("cab_list_default.csv").First());
        Assert.Single(fakeFileReadWriter.Read("cab_list_default.csv"));
    }
    [Fact]
    public void CanConstructPreviousStateAndAddAdditionalCab()
    {
        var fakeFileReadWriter = new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv");
        fakeFileReadWriter.Write("customer_list_default.csv",
            ["Emma,1 Fulton Drive,1 Destination Lane,CustomerCallInProgress\n"]);
        fakeFileReadWriter.Write("cab_list_default.csv", ["Evan,,,\n"]);
        var dispatcherCoordinator = new DispatcherCoordinator();
        var cabService = new CabService(dispatcherCoordinator, fakeFileReadWriter);
        
        cabService.AddCab(new Cab("Evan", 20));
        
        Assert.Equal("Emma,1 Fulton Drive,1 Destination Lane,CustomerCallInProgress", fakeFileReadWriter.Read("customer_list_default.csv").First());
        Assert.Single(fakeFileReadWriter.Read("customer_list_default.csv"));
        Assert.Equal("Evan,,,\n", fakeFileReadWriter.Read("cab_list_default.csv").First());
        Assert.Equal("Evan,,,\n", fakeFileReadWriter.Read("cab_list_default.csv").Skip(1).First());
        Assert.Equal(2, fakeFileReadWriter.Read("cab_list_default.csv").Count());
    }
    [Fact]
    public void CanConstructPreviousStateAndSendCabRequest()
    {
        var fakeFileReadWriter = new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv");
        fakeFileReadWriter.Write("customer_list_default.csv",
            ["Emma,1 Destination Lane,1 Fulton Drive,CustomerCallInProgress\n"]);
        fakeFileReadWriter.Write("cab_list_default.csv", ["Evan,,,\n"]);
        var dispatcherCoordinator = new DispatcherCoordinator();
        var cabService = new CabService(dispatcherCoordinator, fakeFileReadWriter);
        
        cabService.SendCabRequest();
        
        Assert.Equal("Emma,1 Destination Lane,1 Fulton Drive,CustomerCallInProgress\n", fakeFileReadWriter.Read("customer_list_default.csv").First());
        Assert.Single(fakeFileReadWriter.Read("customer_list_default.csv"));
        Assert.Equal("Evan,,,\n", fakeFileReadWriter.Read("cab_list_default.csv").First());
        Assert.Single(fakeFileReadWriter.Read("cab_list_default.csv"));
    }
    [Fact]
    public void CanConstructPreviousStateAndPickupCustomer()
    {
        var fakeFileReadWriter = new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv");
        fakeFileReadWriter.Write("customer_list_default.csv",
            ["Emma,1 Fulton Drive,1 Destination Lane,CustomerCallInProgress\n"]);
        fakeFileReadWriter.Write("cab_list_default.csv", ["Evan,,,\n"]);
        var dispatcherCoordinator = new DispatcherCoordinator();
        var cabService = new CabService(dispatcherCoordinator, fakeFileReadWriter);
        cabService.SendCabRequest();
        
        cabService.PickupCustomer();
        
        Assert.Equal("Emma,1 Destination Lane,1 Fulton Drive,Enroute", fakeFileReadWriter.Read("customer_list_default.csv").First());
        Assert.Single(fakeFileReadWriter.Read("customer_list_default.csv"));
        Assert.Equal("Evan,,,\n", fakeFileReadWriter.Read("cab_list_default.csv").First());
        Assert.Single(fakeFileReadWriter.Read("cab_list_default.csv"));
    }
    [Fact]
    public void CanConstructPreviousStateAndDropOffCustomer()
    {
        var fakeFileReadWriter = new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv");
        fakeFileReadWriter.Write("customer_list_default.csv",
            ["Emma,1 Fulton Drive,1 Destination Lane,CustomerCallInProgress\n"]);
        fakeFileReadWriter.Write("cab_list_default.csv", ["Evan,,,\n"]);
        var dispatcherCoordinator = new DispatcherCoordinator();
        var cabService = new CabService(dispatcherCoordinator, fakeFileReadWriter);
        cabService.SendCabRequest();
        cabService.PickupCustomer();
        
        cabService.DropOffCustomer();
        
        Assert.Equal("Emma,1 Fulton Drive,1 Destination Lane,Delivered", fakeFileReadWriter.Read("customer_list_default.csv").First());
        Assert.Single(fakeFileReadWriter.Read("customer_list_default.csv"));
        Assert.Equal("Evan,,,\n", fakeFileReadWriter.Read("cab_list_default.csv").First());
        Assert.Single(fakeFileReadWriter.Read("cab_list_default.csv"));
    }
    [Fact]
    public void CanConstructPreviousStateAndCancelPickup()
    {
        var fakeFileReadWriter = new FakeFileReadWriter("customer_list_default.csv", "cab_list_default.csv");
        fakeFileReadWriter.Write("customer_list_default.csv",
            ["Emma,1 Fulton Drive,1 Destination Lane,CustomerCallInProgress\n"]);
        fakeFileReadWriter.Write("cab_list_default.csv", ["Evan,,,\n"]);
        var dispatcherCoordinator = new DispatcherCoordinator();
        var cabService = new CabService(dispatcherCoordinator, fakeFileReadWriter);
        
        cabService.CancelPickup();
        
        Assert.Equal("Emma,1 Fulton Drive,1 Destination Lane,CancelledCall", fakeFileReadWriter.Read("customer_list_default.csv").First());
        Assert.Single(fakeFileReadWriter.Read("customer_list_default.csv"));
        Assert.Equal("Evan,,,\n", fakeFileReadWriter.Read("cab_list_default.csv").First());
        Assert.Single(fakeFileReadWriter.Read("cab_list_default.csv"));
    }
}
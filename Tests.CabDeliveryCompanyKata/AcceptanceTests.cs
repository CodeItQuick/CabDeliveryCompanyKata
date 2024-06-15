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
                "0"
            }
        };
        var mockFileReadWriter = new MockFileReadWriter();
        var userInterface = new UserInterface(cabCompanyPrinter, cabCompanyReader, mockFileReadWriter, mockFileReadWriter);
        userInterface.Run();
        
        Assert.Contains("Evan's Cab picked up Emma at 1 Fulton Drive.", cabCompanyPrinter.List());
        Assert.Contains("Evan's Cab dropped off Emma at 1 Destination Lane.", cabCompanyPrinter.List());
    }
    [Fact]
    public void TheCabCompanyReportsFailureIfCannotPickupCustomerDueToNoCalls()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        FakeCabCompanyReader cabCompanyReader = new FakeCabCompanyReader()
        {
            CommandList = new List<string>()
            {
                "1",
                "3",
                "0"
            }
        };
        var mockFileReadWriter = new MockFileReadWriter();
        var userInterface = new UserInterface(cabCompanyPrinter, cabCompanyReader, mockFileReadWriter, mockFileReadWriter);
        userInterface.Run();
        
        Assert.Contains("There are currently no customer's waiting for cabs.", cabCompanyPrinter.List());
    }
    [Fact]
    public void TheCabCompanyReportsFailureIfCannotDropoffCustomerDueToNoCustomerRequests()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        FakeCabCompanyReader cabCompanyReader = new FakeCabCompanyReader()
        {
            CommandList = new List<string>()
            {
                "4",
                "0"
            }
        };
        var mockFileReadWriter = new MockFileReadWriter();
        var userInterface = new UserInterface(cabCompanyPrinter, cabCompanyReader, mockFileReadWriter, mockFileReadWriter);
        userInterface.Run();
        
        Assert.Contains("There are currently no customer's assigned to cabs.", cabCompanyPrinter.List());
    }
    [Fact]
    public void TheCabCompanyReportsFailureIfCannotDropoffCustomerDueToNoRequestedCabs()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        FakeCabCompanyReader cabCompanyReader = new FakeCabCompanyReader()
        {
            CommandList = new List<string>()
            {
                "1",
                "4",
                "0"
            }
        };
        var mockFileReadWriter = new MockFileReadWriter();
        var userInterface = new UserInterface(cabCompanyPrinter, cabCompanyReader, mockFileReadWriter, mockFileReadWriter);
        userInterface.Run();
        
        Assert.Contains("There are currently no customer's assigned to cabs.", cabCompanyPrinter.List());
    }
    [Fact]
    public void TheCabCompanyReportsFailureIfNoCabsAvailable()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        FakeCabCompanyReader cabCompanyReader = new FakeCabCompanyReader()
        {
            CommandList = new List<string>()
            {
                "6",
                "3",
                "0"
            }
        };
        var mockFileReadWriter = new MockFileReadWriter();
        var userInterface = new UserInterface(cabCompanyPrinter, cabCompanyReader, mockFileReadWriter, mockFileReadWriter);
        userInterface.Run();
        
        Assert.Contains("There are currently no cabs in the fleet.", cabCompanyPrinter.List());
    }
    [Fact]
    public void TheCabCompanyCanPickupTwoCustomerAtAnAddress()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        FakeCabCompanyReader cabCompanyReader = new FakeCabCompanyReader()
        {
            CommandList = new List<string>()
            {
                "1",
                "6",
                "6",
                "3",
                "4",
                "3",
                "4",
                "0"
            }
        };
        var mockFileReadWriter = new MockFileReadWriter();
        var userInterface = new UserInterface(cabCompanyPrinter, cabCompanyReader, mockFileReadWriter, mockFileReadWriter);
        userInterface.Run();
        
        Assert.Contains("Evan's Cab picked up Emma at 1 Fulton Drive.", cabCompanyPrinter.List());
        Assert.Contains("Evan's Cab dropped off Emma at 1 Destination Lane.", cabCompanyPrinter.List());
        Assert.Contains("Evan's Cab picked up Lisa at 1 Fulton Drive.", cabCompanyPrinter.List());
        Assert.Contains("Evan's Cab dropped off Lisa at 1 Destination Lane.", cabCompanyPrinter.List());
    }

    [Fact]
    public void TheCabCompanyReportsFailureWhenItCannotMakeAPickupDueToNoAvailableCabs()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        FakeCabCompanyReader cabCompanyReader = new FakeCabCompanyReader()
        {
            CommandList = new List<string>()
            {
                "1",
                "6",
                "6",
                "3",
                "3",
                "0"
            }
        };
        var mockFileReadWriter = new MockFileReadWriter();
        var userInterface = new UserInterface(cabCompanyPrinter, cabCompanyReader, mockFileReadWriter, mockFileReadWriter);
        userInterface.Run();
        
        Assert.Contains("Dispatch failed to pickup Lisa as there are no available cabs.", 
            cabCompanyPrinter.List());
    }

    [Fact]
    public void TheCabCompanyCanPickupTwoCustomerInAMessyOrderingAtAnAddress()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        FakeCabCompanyReader cabCompanyReader = new FakeCabCompanyReader()
        {
            CommandList = new List<string>()
            {
                "1",
                "1",
                "6",
                "6",
                "3",
                "3",
                "4",
                "4",
                "0"
            }
        };
        var mockFileReadWriter = new MockFileReadWriter();
        var userInterface = new UserInterface(cabCompanyPrinter, cabCompanyReader, mockFileReadWriter, mockFileReadWriter);
        userInterface.Run();
        
        Assert.Contains("Evan's Cab picked up Emma at 1 Fulton Drive.", 
            cabCompanyPrinter.List());
        Assert.Contains("Evan's Cab dropped off Emma at 1 Destination Lane.", 
            cabCompanyPrinter.List());
        Assert.Contains("Evan's Cab picked up Lisa at 1 Fulton Drive.", 
            cabCompanyPrinter.List());
        Assert.Contains("Evan's Cab dropped off Lisa at 1 Destination Lane.", 
            cabCompanyPrinter.List());
    }
}
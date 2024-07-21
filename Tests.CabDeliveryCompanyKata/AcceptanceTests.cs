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
                "7",
                "3",
                "4",
                "5",
                "0"
            }
        };
        var customerListFilename = $"customer_list_default{Guid.NewGuid()}.csv";
        var cabListFilename = $"cab_list_default{Guid.NewGuid()}.csv";
        var userInterface = new UserInterface(
            cabCompanyPrinter, 
            cabCompanyReader, 
            new FileHandler(customerListFilename, cabListFilename));
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
        var customerListFilename = $"customer_list_default{Guid.NewGuid()}.csv";
        var cabListFilename = $"cab_list_default{Guid.NewGuid()}.csv";
        var userInterface = new UserInterface(
            cabCompanyPrinter, 
            cabCompanyReader, new FileHandler(customerListFilename, cabListFilename));
        userInterface.Run();
        
        Assert.Contains("There are currently no customer's waiting for cabs.", cabCompanyPrinter.List());
    }
    [Fact]
    public void TheCabCompanyReportsFailureIfCannotDropOffCustomerDueToNoCustomerRequests()
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
        var customerListFilename = $"customer_list_default{Guid.NewGuid()}.csv";
        var cabListFilename = $"cab_list_default{Guid.NewGuid()}.csv";
        var userInterface = new UserInterface(
            cabCompanyPrinter, 
            cabCompanyReader, 
            new FileHandler(customerListFilename, cabListFilename));
        userInterface.Run();
        
        Assert.Contains("There are currently no customer's assigned to cabs.", cabCompanyPrinter.List());
    }
    [Fact]
    public void CannotRemoveCabOnceCustomerPickedUp()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        FakeCabCompanyReader cabCompanyReader = new FakeCabCompanyReader()
        {
            CommandList = new List<string>()
            {
                "1",
                "7",
                "3",
                "4",
                "2",
                "0"
            }
        };
        var customerListFilename = $"customer_list_default{Guid.NewGuid()}.csv";
        var cabListFilename = $"cab_list_default{Guid.NewGuid()}.csv";
        var userInterface = new UserInterface(
            cabCompanyPrinter, 
            cabCompanyReader, 
            new FileHandler(customerListFilename, cabListFilename));
        userInterface.Run();
        
        Assert.Contains("Cab cannot be removed until passenger dropped off.", cabCompanyPrinter.List());
    }
    [Fact]
    public void TheCabCompanyReportsFailureIfCannotPickupCustomerDueToNoCustomerRequests()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        FakeCabCompanyReader cabCompanyReader = new FakeCabCompanyReader()
        {
            CommandList = new List<string>()
            {
                "3",
                "0"
            }
        };
        var customerListFilename = $"customer_list_default{Guid.NewGuid()}.csv";
        var cabListFilename = $"cab_list_default{Guid.NewGuid()}.csv";
        var userInterface = new UserInterface(
            cabCompanyPrinter, 
            cabCompanyReader, new FileHandler(customerListFilename, cabListFilename));
        userInterface.Run();
        
        Assert.Contains("There are currently no cabs in the fleet.", cabCompanyPrinter.List());
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
        var customerListFilename = $"customer_list_default{Guid.NewGuid()}.csv";
        var cabListFilename = $"cab_list_default{Guid.NewGuid()}.csv";
        var userInterface = new UserInterface(
            cabCompanyPrinter, 
            cabCompanyReader, 
            new FileHandler(customerListFilename, cabListFilename));
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
                "7",
                "3",
                "0"
            }
        };
        var customerListFilename = $"customer_list_default{Guid.NewGuid()}.csv";
        var cabListFilename = $"cab_list_default{Guid.NewGuid()}.csv";
        var userInterface = new UserInterface(
            cabCompanyPrinter, 
            cabCompanyReader, new FileHandler(customerListFilename, cabListFilename));
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
                "7",
                "7",
                "3",
                "4",
                "5",
                "3",
                "4",
                "5",
                "0"
            }
        };
        var customerListFilename = $"customer_list_default{Guid.NewGuid()}.csv";
        var cabListFilename = $"cab_list_default{Guid.NewGuid()}.csv";
        var userInterface = new UserInterface(
            cabCompanyPrinter, 
            cabCompanyReader, 
            new FileHandler(customerListFilename, cabListFilename));
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
                "7",
                "7",
                "3",
                "3",
                "0"
            }
        };
        var customerListFilename = $"customer_list_default{Guid.NewGuid()}.csv";
        var cabListFilename = $"cab_list_default{Guid.NewGuid()}.csv";
        var userInterface = new UserInterface(
            cabCompanyPrinter, 
            cabCompanyReader, new FileHandler(customerListFilename, cabListFilename));
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
                "7",
                "7",
                "3",
                "3",
                "4",
                "4",
                "5",
                "5",
                "0"
            }
        };
        var customerListFilename = $"customer_list_default{Guid.NewGuid()}.csv";
        var cabListFilename = $"cab_list_default{Guid.NewGuid()}.csv";
        var userInterface = new UserInterface(
            cabCompanyPrinter, 
            cabCompanyReader, new FileHandler(customerListFilename, cabListFilename));
        userInterface.Run();
        
        Assert.Contains("Evan's Cab picked up Lisa at 1 Fulton Drive.", 
            cabCompanyPrinter.List());
        Assert.Contains("Evan's Cab dropped off Lisa at 1 Destination Lane.", 
            cabCompanyPrinter.List());
        Assert.Contains("Evan's Cab picked up Emma at 1 Fulton Drive.", 
            cabCompanyPrinter.List());
        Assert.Contains("Evan's Cab dropped off Emma at 1 Destination Lane.", 
            cabCompanyPrinter.List());
    }
    [Fact]
    public void CancellingCabWithoutWaitingPickupDisplaysError()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        FakeCabCompanyReader cabCompanyReader = new FakeCabCompanyReader()
        {
            CommandList = new List<string>()
            {
                "6",
                "0"
            }
        };
        var customerListFilename = $"customer_list_default{Guid.NewGuid()}.csv";
        var cabListFilename = $"cab_list_default{Guid.NewGuid()}.csv";
        var userInterface = new UserInterface(
            cabCompanyPrinter, 
            cabCompanyReader, new FileHandler(customerListFilename, cabListFilename));
        userInterface.Run();
        
        Assert.Contains("No customers are waiting for pickup. Cannot cancel cab.", 
            cabCompanyPrinter.List());
    }
    [Fact]
    public void PersistsStateAfterCallForCabDelivery()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        var customerListFilename = $"customer_list_default_repeatable.csv";
        var cabListFilename = $"cab_list_default_repeatable.csv";
        var fakeFileReadWriter = new FakeFileReadWriter(customerListFilename, cabListFilename);
        fakeFileReadWriter.WriteCabList(["Evan,,,"]);
        fakeFileReadWriter.WriteCustomerList(["Emma,1 Destination Lane,1 Fulton Drive,CustomerCallInProgress"]);
        var userInterfaceSecondRun = new UserInterface(
            cabCompanyPrinter, 
            new FakeCabCompanyReader()
            {
                CommandList =
                [
                    "3",
                    "4",
                    "5",
                    "0"
                ]
            }, 
            fakeFileReadWriter);
        
        userInterfaceSecondRun.Run();
        
        Assert.Contains("Evan's Cab picked up Emma at 1 Fulton Drive.", cabCompanyPrinter.List());
        Assert.Contains("Evan's Cab dropped off Emma at 1 Destination Lane.", cabCompanyPrinter.List());
    }
    [Fact(Skip = "Future test")]
    public void PersistsAllStateAfterCallForCabDelivery()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        var commandList = new List<string>()
        {
            "1",
            "7",
            "3",
            "4",
            "0",
            "5",
            "0"
        };
        FakeCabCompanyReader cabCompanyReader = new FakeCabCompanyReader()
        {
            CommandList = commandList
        };
        var customerListFilename = $"customer_list_default{Guid.NewGuid()}.csv";
        var cabListFilename = $"cab_list_default{Guid.NewGuid()}.csv";
        var userInterface = new UserInterface(
            cabCompanyPrinter, 
            cabCompanyReader, 
            new FakeFileReadWriter(customerListFilename, cabListFilename));
        userInterface.Run();
        var userInterfaceSecondRun = new UserInterface(
            cabCompanyPrinter, 
            new FakeCabCompanyReader()
            {
                CommandList = commandList.Skip(5).ToList()
            }, new FileHandler(customerListFilename, cabListFilename));
        
        userInterfaceSecondRun.Run();
        
        Assert.Contains("Evan's Cab picked up Emma at 1 Fulton Drive.", cabCompanyPrinter.List());
        Assert.Contains("Evan's Cab dropped off Emma at 1 Destination Lane.", cabCompanyPrinter.List());
    }
}
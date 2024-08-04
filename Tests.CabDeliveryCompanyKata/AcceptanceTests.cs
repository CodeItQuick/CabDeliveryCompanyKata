using Production.EmmaCabCompany.Adapter.@in;
using Production.EmmaCabCompany.Adapter.@out;

namespace Tests.CabDeliveryCompanyKata;

public class AcceptanceTests
{
    [Fact]
    public void TheCabCompanyCanPickupACustomerAtAnAddress()
    {
        SpyCabCompanyPrinter cabCompanyPrinter = new SpyCabCompanyPrinter();
        FakeCabCompanyReader cabCompanyReader = new FakeCabCompanyReader()
        {
            CommandList = new List<string>()
            {
                "1",
                "7",
                "Emma",
                "1 Fulton Drive",
                "1 Destination Lane",
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
    public void TheCabCompanyCanRePromptOnBadCustomerName()
    {
        SpyCabCompanyPrinter cabCompanyPrinter = new SpyCabCompanyPrinter();
        FakeCabCompanyReader cabCompanyReader = new FakeCabCompanyReader()
        {
            CommandList = new List<string>()
            {
                "1",
                "7",
                "",
                "Emma",
                "1 Fulton Drive",
                "1 Destination Lane",
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
        SpyCabCompanyPrinter cabCompanyPrinter = new SpyCabCompanyPrinter();
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
        
        Assert.Contains("This is not a valid option.", cabCompanyPrinter.List());
    }
    [Fact]
    public void TheCabCompanyReportsFailureIfCannotDropOffCustomerDueToNoCustomerRequests()
    {
        SpyCabCompanyPrinter cabCompanyPrinter = new SpyCabCompanyPrinter();
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
        
        Assert.Contains("This is not a valid option.", cabCompanyPrinter.List());
    }
    [Fact]
    public void CannotRemoveCabOnceCustomerPickedUp()
    {
        SpyCabCompanyPrinter cabCompanyPrinter = new SpyCabCompanyPrinter();
        FakeCabCompanyReader cabCompanyReader = new FakeCabCompanyReader()
        {
            CommandList = new List<string>()
            {
                "1",
                "7",
                "Emma",
                "1 Fulton Drive",
            "1 Destination Lane",
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
        SpyCabCompanyPrinter cabCompanyPrinter = new SpyCabCompanyPrinter();
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
        
        Assert.Contains("This is not a valid option.", cabCompanyPrinter.List());
    }
    [Fact]
    public void TheCabCompanyReportsFailureIfCannotDropoffCustomerDueToNoRequestedCabs()
    {
        SpyCabCompanyPrinter cabCompanyPrinter = new SpyCabCompanyPrinter();
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
        
        Assert.Contains("This is not a valid option.", cabCompanyPrinter.List());
    }
    [Fact]
    public void TheCabCompanyReportsFailureIfNoCabsAvailable()
    {
        SpyCabCompanyPrinter cabCompanyPrinter = new SpyCabCompanyPrinter();
        FakeCabCompanyReader cabCompanyReader = new FakeCabCompanyReader()
        {
            CommandList = new List<string>()
            {
                "7",
                "Emma",
                "1 Fulton Drive",
                "1 Destination Lane",
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
        
        Assert.Contains("This is not a valid option.", cabCompanyPrinter.List());
    }
    [Fact]
    public void TheCabCompanyCanPickupTwoCustomerAtAnAddress()
    {
        SpyCabCompanyPrinter cabCompanyPrinter = new SpyCabCompanyPrinter();
        FakeCabCompanyReader cabCompanyReader = new FakeCabCompanyReader()
        {
            CommandList = new List<string>()
            {
                "1",
                "7",
                "Emma",
                "1 Fulton Drive",
            "1 Destination Lane",
                "7",
                "Lisa",
                "1 Fulton Drive",
                "1 Destination Lane",
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
        SpyCabCompanyPrinter cabCompanyPrinter = new SpyCabCompanyPrinter();
        FakeCabCompanyReader cabCompanyReader = new FakeCabCompanyReader()
        {
            CommandList = new List<string>()
            {
                "1",
                "7",
                "Emma",
                "1 Fulton Drive",
                "1 Destination Lane",
                "7",
                "Lisa",
                "1 Fulton Drive",
                "1 Destination Lane",
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
        SpyCabCompanyPrinter cabCompanyPrinter = new SpyCabCompanyPrinter();
        FakeCabCompanyReader cabCompanyReader = new FakeCabCompanyReader()
        {
            CommandList = new List<string>()
            {
                "1",
                "1",
                "7",
                "Emma",
                "1 Fulton Drive",
                "1 Destination Lane",
                "7",
                "Lisa",
                "Walmart",
                "1 Destination Lane",
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
        
        Assert.Contains("Evan's Cab picked up Lisa at Walmart.", 
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
        SpyCabCompanyPrinter cabCompanyPrinter = new SpyCabCompanyPrinter();
        FakeCabCompanyReader cabCompanyReader = new FakeCabCompanyReader()
        {
            CommandList =
            [
                "6",
                "0"
            ]
        };
        var customerListFilename = $"customer_list_default{Guid.NewGuid()}.csv";
        var cabListFilename = $"cab_list_default{Guid.NewGuid()}.csv";
        var userInterface = new UserInterface(
            cabCompanyPrinter, 
            cabCompanyReader, new FileHandler(customerListFilename, cabListFilename));
        userInterface.Run();
        
        Assert.Contains("This is not a valid option.", 
            cabCompanyPrinter.List());
    }
    [Fact]
    public void PersistsStateAfterCallForCabDelivery()
    {
        SpyCabCompanyPrinter cabCompanyPrinter = new SpyCabCompanyPrinter();
        var customerListFilename = $"customer_list_default_repeatable.csv";
        var cabListFilename = $"cab_list_default_repeatable.csv";
        var fakeFileReadWriter = new FakeFileReadWriter(customerListFilename, cabListFilename);
        fakeFileReadWriter.WriteCabList(["Evan's Cab,,,"]);
        fakeFileReadWriter.WriteCustomerList(["Emma,1 Fulton Drive,1 Destination Lane,CustomerCallInProgress"]);
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
    [Fact]
    public void PersistsAllStateAfterCallForCabDelivery()
    {
        SpyCabCompanyPrinter cabCompanyPrinter = new SpyCabCompanyPrinter();
        var commandList = new List<string>()
        {
            "1",
            "7",
            "Emma",
            "Bowling Alley",
            "1 Destination Lane",
            "3",
            "4",
            "0",
        };
        FakeCabCompanyReader cabCompanyReader = new FakeCabCompanyReader()
        {
            CommandList = commandList
        };
        var customerListFilename = $"customer_list_persisted_state.csv";
        var cabListFilename = $"cab_list_persisted_state.csv";
        var fakeFileReadWriter = new FakeFileReadWriter(customerListFilename, cabListFilename);
        var userInterface = new UserInterface(
            cabCompanyPrinter, 
            cabCompanyReader, 
            fakeFileReadWriter);
        userInterface.Run();
        Assert.Contains("Evan's Cab picked up Emma at Bowling Alley.", cabCompanyPrinter.List());
        var userInterfaceSecondRun = new UserInterface(
            cabCompanyPrinter, 
            new FakeCabCompanyReader()
            {
                CommandList =
                [
                    "5",
                    "0"
                ]
            }, 
            fakeFileReadWriter);
        
        userInterfaceSecondRun.Run();

        Assert.Contains("Emma,Bowling Alley,1 Destination Lane,Delivered,46.23496,-63.12495", fakeFileReadWriter.Read(customerListFilename));
        Assert.Contains(
            "Evan's Cab dropped off Emma at 1 Destination Lane.", 
            cabCompanyPrinter.List());
    }
}
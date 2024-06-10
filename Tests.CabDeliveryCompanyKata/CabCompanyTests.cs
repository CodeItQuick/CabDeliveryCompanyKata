using Production.EmmaCabCompany;

namespace Tests.CabDeliveryCompanyKata;

public class CabCompanyTests
{
    [Fact]
    public void TheCabCompanyCanPickupACustomerAtAnAddress()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        List<ICabs> cabs =
        [
            new Cabs("Evan's Cab", cabCompanyPrinter, 20),
        ];
        List<Customer> customers = [new Customer("Darrell", "1 Fulton Drive", "1 University Avenue", 20)];

        EmmaCabCompany.CallCab(cabs, customers);
        
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
        List<Customer> customers = [new Customer("Diane", "2 Fulton Drive", "2 University Avenue", 20)];

        EmmaCabCompany.CallCab(cabs, customers);
        
        Assert.Equal("Evan's Cab dropped off Diane at 2 University Avenue", cabCompanyPrinter.Retrieve(1));
    }
    [Fact]
    public void TheCabCompanyHasMultipleCabbiesAvailable()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        List<ICabs> cabs =
        [
            new Cabs("Evan's Cab", cabCompanyPrinter, 20),
            new Cabs("Dan's Cab", cabCompanyPrinter, 20),
        ];

        List<Customer> customers = [new Customer("Diane", "2 Fulton Drive", "2 University Avenue", 20)];

        EmmaCabCompany.CallCab(cabs, customers);
        
        Assert.Equal(2, cabCompanyPrinter.CountMessages());
    }
    [Fact]
    public void TheCabCompanyCanPickupACoupleFaresAtATime()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        List<ICabs> cabs =
        [
            new Cabs("Evan's Cab", cabCompanyPrinter, 20),
            new Cabs("Dan's Cab", cabCompanyPrinter, 20),
        ];
        List<Customer> customers =
        [
            new Customer("Diane", "1 Fulton Drive", "1 University Avenue", 20),
            new Customer("Darrell", "2 Fulton Drive", "2 University Avenue", 20)
        ];

        EmmaCabCompany.CallCab(
            cabs, customers 
            );
        
        Assert.Equal(4, cabCompanyPrinter.CountMessages());
    }
    [Fact]
    public void TheCabCompanyCanPickupACoupleFaresInARow()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        List<ICabs> cabs =
        [
            new Cabs("Evan's Cab", cabCompanyPrinter, 20),
            new Cabs("Dan's Cab", cabCompanyPrinter, 20),
        ];
        List<Customer> customers =
        [
            new("Diane", "1 Fulton Drive", "1 University Avenue", 20)
        ];
        List<Customer> customersSecondCall =
        [
            new("Diane", "2 Fulton Drive", "2 University Avenue", 20)
        ];

        EmmaCabCompany.CallCab(
            cabs, customers 
            );
        EmmaCabCompany.CallCab(
            cabs, customersSecondCall
        );
        
        Assert.Equal(4, cabCompanyPrinter.CountMessages());
    }
    [Fact]
    public void TheCabCompanyChargesTheCustomerAFare()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        List<ICabs> cabs =
        [
            new Cabs("Evan's Cab", cabCompanyPrinter, 20),
        ];
        List<Customer> customers = [
            new("Diane", "2 Fulton Drive", "2 University Avenue", 20)
        ];

        EmmaCabCompany.CallCab(cabs, customers);
        
        Assert.Equal(15, customers.First().Wallet);
        Assert.Equal(25, cabs.First().Wallet);
    }
    [Fact]
    public void TheCabCompanyChargesTwoCustomerCoupleFares()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        List<ICabs> cabs =
        [
            new Cabs("Evan's Cab", cabCompanyPrinter, 40),
        ];
        List<Customer> customers =
        [
            new("Diane", "2 Fulton Drive", "2 University Avenue", 20),
            new("Darrell", "1 Fulton Drive", "1 University Avenue", 20)
        ];

        EmmaCabCompany.CallCab(cabs, customers);
        
        Assert.Equal(15, customers.First().Wallet);
        Assert.Equal(15, customers.Skip(1).First().Wallet);
        Assert.Equal(50, cabs.First().Wallet);
    }
    [Fact]
    public void TheCabCompanyChargesTwoCustomerCoupleFaresAfterTwoCallsMade()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        List<ICabs> cabs =
        [
            new Cabs("Evan's Cab", cabCompanyPrinter, 20),
            new Cabs("Dan's Cab", cabCompanyPrinter, 20),
        ];
        List<Customer> customers =
        [
            new("Diane", "2 Fulton Drive", "2 University Avenue", 20),
            new("Darrell", "1 Fulton Drive", "1 University Avenue", 20)
        ];

        EmmaCabCompany.CallCab(cabs, customers);
        
        Assert.Equal(15, customers.First().Wallet);
        Assert.Equal(15, customers.Skip(1).First().Wallet);
        Assert.Equal(25, cabs.First().Wallet);
        Assert.Equal(25, cabs.Skip(1).First().Wallet);
    }
    [Fact]
    public void TheCustomerHasRideHistoryAvailable()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        List<ICabs> cabs =
        [
            new Cabs("Evan's Cab", cabCompanyPrinter, 20),
        ];
        List<Customer> customers =
        [
            new("Diane", "2 Fulton Drive", "2 University Avenue", 20)
        ];

        EmmaCabCompany.CallCab(cabs, customers);
        
        Assert.Equal("Evan's Cab", customers.First().RideHistory.NameOfCabsTaken.First());
    }
    [Fact]
    public void TheCustomerHasRideHistoryAvailableForMultipleCalls()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        List<ICabs> cabs =
        [
            new Cabs("Evan's Cab", cabCompanyPrinter, 20),
            new Cabs("Dan's Cab", cabCompanyPrinter, 20),
        ];
        List<Customer> customers =
        [
            new("Diane", "2 Fulton Drive", "2 University Avenue", 20)
        ];

        EmmaCabCompany.CallCab(cabs, customers);
        EmmaCabCompany.CallCab(cabs, customers);
        
        Assert.Equal("Evan's Cab", customers.First().RideHistory.NameOfCabsTaken.First());
        Assert.Equal("Dan's Cab", customers.First().RideHistory.NameOfCabsTaken.Skip(1).First());
    }
    [Fact]
    public void TheCabCompanyCanHandleMakingChange()
    {
        FakeCabCompanyPrinter cabCompanyPrinter = new FakeCabCompanyPrinter();
        List<ICabs> cabs =
        [
            new Cabs("Evan's Cab", cabCompanyPrinter, 30),
        ];
        List<Customer> customers =
        [
            new("Diane", "2 Fulton Drive", "2 University Avenue", 15),
            new("Diane", "2 Fulton Drive", "2 University Avenue", 30),
        ];

        EmmaCabCompany.CallCab(cabs, customers);
        
        Assert.Equal(4, cabCompanyPrinter.CountMessages());
    }
}
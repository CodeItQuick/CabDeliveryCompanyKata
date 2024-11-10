using Production.EmmaCabCompany.Adapter.@in;
using Production.EmmaCabCompany.Domain;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;

public class CabFileRepository(IFileHandler fileHandler)
{
    public Dictionary<Customer, CustomerStatus> LoadedCustomerDirectory()
    {
        var customerList = fileHandler.ReadCustomerList();
        var customerDirectory = CustomerList.CreateCustomerState(customerList);
        return customerDirectory;
    }

    public Fleet.Fleet LoadedFleetState()
    {
        var cabList = fileHandler.ReadReadCabList();
        var newFleet = new Fleet.Fleet();
        newFleet.CreateFleet(cabList, LoadedCustomerDirectory());
        return newFleet;
    }

    public void WriteCustomerList(string[] exportedCustomers)
    {
        fileHandler.WriteCustomerList(exportedCustomers);
    }

    public void WriteCabList(string[] cabList)
    {
        fileHandler.WriteCabList(cabList);
    }
}
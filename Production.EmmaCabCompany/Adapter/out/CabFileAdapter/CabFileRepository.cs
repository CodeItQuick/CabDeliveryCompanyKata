using Production.EmmaCabCompany.Adapter.@in;
using Production.EmmaCabCompany.Domain;

namespace Production.EmmaCabCompany.Adapter.@out.CabFileAdapter;

public class CabFileRepository(IFileHandler fileHandler)
{
    public Dictionary<Customer, CustomerStatus> LoadedCustomerDirectory()
    {
        var customerList = fileHandler.ReadCustomerList();
        var customerDirectory = CustomerList.CreateCustomerState(customerList);
        return customerDirectory;
    }

    public Fleet LoadedFleetState()
    {
        var cabList = fileHandler.ReadReadCabList();
        var newFleet = new Fleet();
        newFleet.CreateFleet(cabList);
        return newFleet;
    }

    public CustomerList LoadedCustomerListState()
    {
        var customerList = fileHandler.ReadCustomerList();
        var newFleet = new CustomerList();
        newFleet.CreateCustomerList(customerList);
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
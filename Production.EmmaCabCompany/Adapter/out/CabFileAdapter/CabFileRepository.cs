using Production.EmmaCabCompany.Domain;

namespace Production.EmmaCabCompany.Adapter.@in;

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
        var loadedFleetState = new Fleet(cabList);
        return loadedFleetState;
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
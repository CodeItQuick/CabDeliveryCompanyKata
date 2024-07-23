using Production.EmmaCabCompany.Domain;
using Production.EmmaCabCompany.Service;

namespace Production.EmmaCabCompany.Adapter;

public class CabFileRepository(IFileHandler fileHandler)
{
    public Dictionary<Customer, CustomerStatus> LoadedCustomerDirectory()
    {
        var customerList = fileHandler.ReadCustomerList();
        var customerDirectory = CustomerList.CreateCustomerState(customerList);
        return customerDirectory;
    }

    public List<Cab> LoadedFleetState()
    {
        var cabList = fileHandler.ReadReadCabList();
        var loadedFleetState = Fleet.CreateCabState(cabList);
        return loadedFleetState;
    }
}
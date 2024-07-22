using Production.EmmaCabCompany.Domain;
using Production.EmmaCabCompany.Service;

namespace Production.EmmaCabCompany.Adapter;

public class CabFileRepository
{
    public static Dictionary<Customer, CustomerStatus> LoadedCustomerDirectory(IFileHandler fileHandler)
    {
        var customerList = fileHandler.ReadCustomerList();
        var customerDirectory = CustomerList.CreateCustomerState(customerList);
        return customerDirectory;
    }

    public static List<Cab> LoadedFleetState(IFileHandler fileHandler)
    {
        var cabList = fileHandler.ReadReadCabList();
        var loadedFleetState = Fleet.CreateCabState(cabList);
        return loadedFleetState;
    }
}
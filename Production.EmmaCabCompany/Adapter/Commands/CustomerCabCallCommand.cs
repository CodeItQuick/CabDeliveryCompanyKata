namespace Production.EmmaCabCompany.Commands;

public static class CustomerCabCallCommand
{
    public static string Select(List<string> customerNames, ref int numCustomersServed, List<Customer> customersCallInProgress, ICabCompanyPrinter cabCompanyPrinter)
    {
        var customerName = customerNames[numCustomersServed];
        numCustomersServed++;
        var customer = new Customer(customerName, "1 Fulton Drive", "1 Destination Lane");
        customersCallInProgress.Add(customer);
        return $"Received customer ride request from {customerName}";
    }
}
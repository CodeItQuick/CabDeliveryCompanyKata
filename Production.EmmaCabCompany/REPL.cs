namespace Production.EmmaCabCompany;

public class REPL
{
    public static void Run(ICabCompanyPrinter cabCompanyPrinter, ICabCompanyReader cabCompanyReader, List<ICabs> cabsList, List<Customer> customers)
    {
        const int randomStartNumber = 10;
        int selection = randomStartNumber;
        bool isChosen = true;
        while (selection != 0 && isChosen)
        {
            cabCompanyPrinter.WriteLine("Please choose a selection from the list: ");
            cabCompanyPrinter.WriteLine("0. Exit");
            cabCompanyPrinter.WriteLine("1. Add cabbie to fleet");
            cabCompanyPrinter.WriteLine("2. Remove cabbie from fleet");
            cabCompanyPrinter.WriteLine("3. Add customer to call list");
            cabCompanyPrinter.WriteLine("4. Remove customer to call list");
            cabCompanyPrinter.WriteLine("5. Deploy cabbie fleet");
            var lineEntered = cabCompanyReader.ReadLine();
        
            cabCompanyPrinter.WriteLine($"You selected: {lineEntered}");
            isChosen = Int32.TryParse(lineEntered, out selection);
            if (isChosen)
            {
                if (selection == 1)
                {
                    cabsList.Add(new Cabs($"default {cabsList.Count + 1}", cabCompanyPrinter, 20));
                }
                if (selection == 2)
                {
                    if (cabsList.Any())
                    {
                        cabsList.RemoveAt(cabsList.Count - 1);
                    }
                }
                if (selection == 3)
                {
                    customers.Add(
                        new Customer($"default {customers.Count + 1}", $"{customers.Count + 1} Default Start Drive", $"{customers.Count + 1} Default Final Drive", 20));
                    cabCompanyPrinter.WriteLine("Customer List:");
                    foreach (var customer in customers)
                    {
                        cabCompanyPrinter.WriteLine(customer.CustomerName);
                    }
                }
                if (selection == 4)
                {
                    if (cabsList.Any())
                    {
                        customers.RemoveAt(customers.Count - 1);
                    }
                    cabCompanyPrinter.WriteLine("Customer List:");
                    foreach (var customer in customers)
                    {
                        cabCompanyPrinter.WriteLine(customer.CustomerName);
                    }
                }
                if (selection == 5)
                {
                    EmmaCabCompany.CallCab(cabsList, customers);
                }
            }
            
        }
    }
}
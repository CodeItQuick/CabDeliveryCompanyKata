namespace Production.EmmaCabCompany;

public class REPL
{
    public static void Run(CabCompanyPrinter cabCompanyPrinter, CabCompanyWriter cabCompanyWriter, List<ICabs> cabsList, List<Customer> customers)
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
            var lineEntered = cabCompanyWriter.ReadLine();
        
            cabCompanyPrinter.WriteLine($"You selected: {lineEntered}");
            isChosen = Int32.TryParse(lineEntered, out selection);
            if (!isChosen) continue;
            switch (selection)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }
        }
    }
}
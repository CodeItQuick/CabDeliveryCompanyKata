namespace Production.EmmaCabCompany.Commands;

public class RemoveCabCommand
{
    public static void Select(Dispatch dispatch, ICabCompanyPrinter cabCompanyPrinter)
    {
        if (!dispatch.NoCabsInFleet())
        {
            var success = dispatch.RemoveCab();
            if (success)
            {
                cabCompanyPrinter.WriteLine("Last cab removed from cab fleet.");
            }
            else
            {
                cabCompanyPrinter.WriteLine("Cab cannot be removed until passenger dropped off.");
            }
        }
    }
}
namespace Production.EmmaCabCompany.Commands;

public class RemoveCabCommand
{
    public static string Select(Dispatch dispatch, ICabCompanyPrinter cabCompanyPrinter)
    {
        if (!dispatch.NoCabsInFleet())
        {
            var success = dispatch.RemoveCab();
            if (success)
            {
                return "Last cab removed from cab fleet.";
            }

            return "Cab cannot be removed until passenger dropped off.";
        }

        return "No cabs in fleet currently";
    }
}
namespace Production.EmmaCabCompany;

public static class AddCabCommand
{
    public static void Select(Dispatch dispatch, ICabCompanyPrinter cabCompanyPrinter)
    {
        var cabName = "Evan's Cab";
        var addCab = dispatch.AddCab(new Cab(cabName, 20));
        if (addCab)
        {
            cabCompanyPrinter.WriteLine("Added Evan's Cab to fleet");
        }
    }
}
namespace Production.EmmaCabCompany;

public static class AddCabCommand
{
    public static string Select(Dispatch dispatch)
    {
        var cabName = "Evan's Cab";
        var addCab = dispatch.AddCab(new Cab(cabName, 20));
        if (addCab)
        {
            return "Added Evan's Cab to fleet";
        }

        return "Failed to add Evan's Cab to fleet.";
    }
}
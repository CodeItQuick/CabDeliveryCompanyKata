namespace Production.EmmaCabCompany.Domain.Menu;

public class MenuRequested
{
    public int? Id { get; set; }

    public MenuRequested(int? id)
    {
        Id = id;
    }
}
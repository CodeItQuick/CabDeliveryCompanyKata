namespace Production.EmmaCabCompany.Application.Menu;

public interface IMenuRepository
{
    public Adapter.OutAdapter.CabFileAdapter.Menu.Menu GetById(int customerListId);
}
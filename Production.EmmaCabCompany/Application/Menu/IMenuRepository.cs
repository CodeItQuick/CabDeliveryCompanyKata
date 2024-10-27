using Production.EmmaCabCompany.Domain.Menu;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;

public interface IMenuRepository
{
    public Menu GetById(int customerListId);
}
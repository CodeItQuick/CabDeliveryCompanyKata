using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Application.CustomerList.Commands.Command;

namespace Production.EmmaCabCompany.Application.CustomerList.Commands.Handler;

public interface ICustomerCabRequestedCommandHandler : ICommandHandler<CustomerCabRequested>;

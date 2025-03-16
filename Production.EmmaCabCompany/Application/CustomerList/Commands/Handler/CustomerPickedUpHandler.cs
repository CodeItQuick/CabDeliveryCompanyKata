using Production.EmmaCabCompany.Application.CustomerList.Commands.Command;

namespace Production.EmmaCabCompany.Application.CustomerList.Commands.Handler;

public interface ICustomerPickedUpCommandHandler : ICommandHandler<CustomerPickedUp>;
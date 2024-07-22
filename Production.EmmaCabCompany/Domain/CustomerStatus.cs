namespace Production.EmmaCabCompany.Domain;

public enum CustomerStatus
{
    WaitingPickup,
    Enroute,
    Delivered,
    CustomerCallInProgress,
    CancelledCall
}
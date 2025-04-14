namespace Production.EmmaCabCompany.Domain.CustomerList;

public class PatronList
{
    public int Id { get; set; } = 1;
    public List<Patron> Patrons { get; init; } = new();

    public static PatronList CreateCustomerList(int id, List<Patron> customers)
    {
        return new PatronList() { Id = id, Patrons = customers};
    }

    public void CustomerCabCall(Patron patron)
    {
        patron.Status = PatronStatus.CustomerCallInProgress;
        Patrons.Add(patron);
    }
    public void RideRequest()
    {
        if (Patrons.All(x => x.Status != PatronStatus.CustomerCallInProgress))
        {
            return;
        }
        
        Patrons.FirstOrDefault(x => x.Status == PatronStatus.CustomerCallInProgress)!.Status = 
            PatronStatus.WaitingPickup;
    }

    public void PickupCustomer()
    {
        if (Patrons.Any(x => x.Status == PatronStatus.WaitingPickup))
        {
            var customerWaiting = Patrons.FirstOrDefault(x => x.Status == PatronStatus.WaitingPickup)!;
            customerWaiting.Status = PatronStatus.Enroute;
        }
    }

    public void CancelPickup()
    {
        if (Patrons.All(x => 
                x.Status != PatronStatus.WaitingPickup && x.Status != PatronStatus.CustomerCallInProgress))
        {
            throw new SystemException("No customers are waiting for pickup. Cannot cancel cab.");
        }

        if (Patrons.Any(x => x.Status == PatronStatus.CustomerCallInProgress))
        {
            var customerToChange = Patrons.FirstOrDefault(x => x.Status == PatronStatus.CustomerCallInProgress);
            customerToChange!.Status = PatronStatus.CancelledCall;
        }
    }

    public void CustomerDelivered()
    {
        if (Patrons.All(x => x.Status != PatronStatus.Enroute))
        {
            return;
        }
        
        var customerWaiting = Patrons.FirstOrDefault(x => x.Status == PatronStatus.Enroute)!;
        customerWaiting.Status = PatronStatus.Delivered;
    }
}
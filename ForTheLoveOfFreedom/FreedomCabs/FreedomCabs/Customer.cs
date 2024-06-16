namespace FreedomCabs
{
    public class Customer : ICustomer
    {
        public bool HasRequestedRide => false;

        public void RequestRide()
        {
            throw new NotImplementedException();
        }
    }
}

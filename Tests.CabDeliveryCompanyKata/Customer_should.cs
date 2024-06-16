using Production.EmmaCabCompany;

namespace Tests.CabDeliveryCompanyKata
{
    public class Customer_should
    {
        [Fact]
        public void by_default_not_have_request_ride()
        {
            Customer customer = new Customer();
            
            Assert.False(customer.CurrentStatus());
        }
        [Fact]
        public void request_ride()
        {
            Customer customer = new Customer();
            var dispatch = new Dispatcher();

            var hasRequested = dispatch.RequestRide(customer);

            Assert.False(customer.CurrentStatus());
        }
    }
}
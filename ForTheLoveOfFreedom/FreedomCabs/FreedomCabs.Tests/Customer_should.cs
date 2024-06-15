using Shouldly;

namespace FreedomCabs.Tests
{
    public class Customer_should
    {
        [Fact]
        public void by_default_not_have_request_ride()
        {
            ICustomer sut = new Customer();
            sut.HasRequestedRide.ShouldBeFalse();
        }
        [Fact]
        public void request_ride()
        {
            ICustomer sut = new Customer();
            sut.RequestRide();
            sut.HasRequestedRide.ShouldBeTrue();
        }
    }
}
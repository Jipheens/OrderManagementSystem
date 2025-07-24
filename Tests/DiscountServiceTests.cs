using OrderManagementSystem.Services;
using Xunit;

namespace OrderManagementSystem.Tests
{
    public class DiscountServiceTests
    {
        private readonly DiscountService _service = new();

        [Fact]
        public void VIP_Customer_Over_500_Gets_20Percent_Discount()
        {
            var customer = new Customer { Segment = CustomerSegment.VIP };
            var order = new Order { TotalAmount = 600m };
            var result = _service.ApplyDiscount(customer, order);

            Assert.Equal(480m, result);
        }
    }

}

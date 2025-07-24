namespace OrderManagementSystem.Services
{
    public interface IDiscountService
    {
        decimal ApplyDiscount(Customer customer, Order order);
    }

    public class DiscountService : IDiscountService
    {
        public decimal ApplyDiscount(Customer customer, Order order)
        {
            decimal discount = customer.Segment switch
            {
                CustomerSegment.Premium => 0.10m,
                CustomerSegment.VIP => order.TotalAmount > 500 ? 0.20m : 0.15m,
                _ => 0m
            };

            return order.TotalAmount * (1 - discount);
        }
    }
}


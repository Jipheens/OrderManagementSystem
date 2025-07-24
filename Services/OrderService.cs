using OrderManagementSystem.Interfaces;

namespace OrderManagementSystem.Services
{
    public class OrderService : IOrderService
    {
        private readonly List<Order> _orders = new(); // Replace with DbContext in real app
        private readonly IDiscountService _discountService;

        public OrderService(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        public Order CreateOrder(Customer customer, decimal totalAmount)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = customer.Id,
                TotalAmount = totalAmount,
                DiscountedAmount = _discountService.ApplyDiscount(customer, new Order { TotalAmount = totalAmount }),
                Status = OrderStatus.Created,
                CreatedAt = DateTime.UtcNow
            };

            _orders.Add(order);
            return order;
        }

        public bool UpdateStatus(Guid orderId, OrderStatus newStatus)
        {
            var order = _orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null) return false;

            if (IsValidTransition(order.Status, newStatus))
            {
                order.Status = newStatus;
                if (newStatus == OrderStatus.Fulfilled)
                    order.FulfilledAt = DateTime.UtcNow;

                return true;
            }

            return false;
        }

        private bool IsValidTransition(OrderStatus current, OrderStatus next) =>
            (current, next) switch
            {
                (OrderStatus.Created, OrderStatus.Processing) => true,
                (OrderStatus.Processing, OrderStatus.Fulfilled) => true,
                (_, OrderStatus.Cancelled) => true,
                _ => false
            };

        public IEnumerable<Order> GetAll() => _orders;

        public (double avgValue, double avgFulfillmentTime) GetAnalytics()
        {
            var fulfilled = _orders.Where(o => o.FulfilledAt.HasValue).ToList();
            if (!fulfilled.Any()) return (0, 0);

            double avgValue = fulfilled.Average(o => (double)o.DiscountedAmount);
            double avgTime = fulfilled.Average(o => (o.FulfilledAt.Value - o.CreatedAt).TotalMinutes);

            return (avgValue, avgTime);
        }
    }

}

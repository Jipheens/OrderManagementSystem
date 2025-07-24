namespace OrderManagementSystem
{
    public enum CustomerSegment
    {
        Regular,
        Premium,
        VIP
    }

    public enum OrderStatus
    {
        Created,
        Processing,
        Fulfilled,
        Cancelled
    }

    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CustomerSegment Segment { get; set; }
        public List<Order> Orders { get; set; } = new();
    }

    public class Order
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountedAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? FulfilledAt { get; set; }
    }
}
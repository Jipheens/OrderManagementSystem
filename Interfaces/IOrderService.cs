namespace OrderManagementSystem.Interfaces
{
    public interface IOrderService
    {
        Order CreateOrder(Customer customer, decimal totalAmount);
        bool UpdateStatus(Guid orderId, OrderStatus newStatus);
        IEnumerable<Order> GetAll();
        (double avgValue, double avgFulfillmentTime) GetAnalytics();
    }
}

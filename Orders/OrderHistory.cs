namespace Virtual_Trading_Simulator_Project.Orders;
using Virtual_Trading_Simulator_Project.Tickers;
using Virtual_Trading_Simulator_Project.Users;

public class OrderHistory
{
    private readonly List<Order> _orders = new(); 
    private readonly object _lock = new();

    public void AddOrder(Order order)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));

        lock (_lock)
        {
            _orders.Add(order);
        }
    }

    public IReadOnlyList<Order> GetOrders()
    {
        lock (_lock)
        {
            return _orders.AsReadOnly();
        }
    }

    public IReadOnlyList<OrderHistory> GetByTrader(Trader trader)
    {
        if (trader == null) throw new ArgumentNullException(nameof(trader));

        lock (_lock)
        {
           return _orders
                .Where(o => o.Trader.Id == trader.Id)
                .Select(o => new OrderHistory { _orders = new List<Order> { o } })
                .ToList()
                .AsReadOnly();
        }
    }

    
}
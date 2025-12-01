using Virtual_Trading_Simulator_Project.Users.Holdings;
using Virtual_Trading_Simulator_Project.Orders;
using Virtual_Trading_Simulator_Project.Statistics;
using Virtual_Trading_Simulator_Project.Tickers;

namespace Virtual_Trading_Simulator_Project.Users;


public class Trader
{
    public string Username {get; set;}
    
    private string _password;
    private double _balance;
    private readonly HoldingManager _holdings;
    public string Role {get; } = "Trader";
    private readonly OrderHistory _orderHistory;
    public readonly TraderStatistics Statistics;

    public Trader(string username, string password, double initialBalance = 100)
    {
        if (initialBalance < 0)
            throw new ArgumentException("Initial balance cannot be negative");
        Username = username;
        _password = password;
        _balance = initialBalance;
        _holdings = new HoldingManager();
        _orderHistory = new OrderHistory();
        Statistics = new TraderStatistics(this);
    }
    
    public bool PlaceOrder(Order order)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));

        order.PlaceOrder();
        _orderHistory.AddOrder(order);
        
        return true;
    }
    
    public double GetBalance()
    {
        return _balance;
    }

    public bool UpdateBalance(double change)
    {
        // Balance should not go under 0, may need to be changed if different types of trading are added
        if (_balance + change < 0)
        {
            return false;
        }
        
        _balance += change;
        return true;
    }

    public HoldingManager GetHoldings()
    {
        return _holdings;
    }

    public IReadOnlyList<Order> GetOrderHistory()
    {
        return _orderHistory.GetOrders();
    }
}
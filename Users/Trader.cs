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

        if (!order.Validate())
        {
            return false;
        }

        double price = order.Ticker.GetPrice();
        double tradeValue = price * order.Quantity;

        if (string.Equals(order.OrderType, "BUY", StringComparison.OrdinalIgnoreCase))
        {
            if (_balance < tradeValue)
            {
                return false; // Insufficient balance
            }

            _balance -= tradeValue;
            AddHolding(order.Ticker, order.Quantity, price);
        }
        else if (string.Equals(order.OrderType, "SELL", StringComparison.OrdinalIgnoreCase))
        {
            if (!RemoveHolding(order.Ticker, order.Quantity))
            {
                return false; // Insufficient holdings
            }

            _balance += tradeValue;
        }
        else
        {
            return false; // Invalid order type
        }

        order.PlaceOrder();
        _orderHistory.Add(order);
        return true;
    }
    
    public int GetBalance()
    {
        return (int)_balance;
    }

    public void UpdateBalance(double change)
    {
        _balance += change;
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
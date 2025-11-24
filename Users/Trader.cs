namespace Virtual_Trading_Simulator_Project.Users;
using Virtual_Trading_Simulator_Project.Orders;
using Virtual_Trading_Simulator_Project.Tickers;
using Virtual_Trading_Simulator_Project.Holdings;

public class Trader
{
    public int Id {get; set;}
    public string Username {get; set;}
    
    private int _password;
    private double _balance;
    private readonly Dictionary<Ticker, List<Holding>> _holdings;
    public string Role {get; } = "Trader";
    private readonly OrderHistory _orderHistory;

    public Trader(int id, string username, int password, double initialBalance)
    {
        Id = id;
        Username = username;
        _password = password;
        _balance = initialBalance;
        _holdings = new Dictionary<Ticker, List<Holding>>();
        _orderHistory = new OrderHistory();
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

    public void UpdateBalance(int change)
    {
        _balance += change;
    }

    public Dictionary<Ticker, List<Holding>> GetHoldings()
    {
        return _holdings;
    }

}
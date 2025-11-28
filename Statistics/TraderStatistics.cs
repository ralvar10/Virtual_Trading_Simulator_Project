using Virtual_Trading_Simulator_Project.Orders;
using Virtual_Trading_Simulator_Project.Users;

namespace Virtual_Trading_Simulator_Project.Statistics;

public class TraderStatistics : IStatistics
{
    private readonly Trader _trader;
    private readonly double _initialBalance;
    private double _highestBalance;
    private double _overallGains;
    private double _averageTradePrice;

    public TraderStatistics(Trader trader)
    {
        _trader = trader;
        _initialBalance = trader.GetBalance();
        _highestBalance = 0;
    }
    
    public void UpdateStatistics()
    {
        if (_trader.GetBalance() > _highestBalance)
            _highestBalance = _trader.GetBalance();
        
        _overallGains = _trader.GetBalance() - _initialBalance;

        List<Order> orderHistory = _trader.GetOrderHistory().ToList();
        int count = 0;
        double total = 0;

        foreach (Order order in orderHistory)
        {
            if (order.Status == OrderStatus.Filled && order is SellOrder)
            {
                count++;
                total += order.Value;
            }
        }
        if (count > 0)
            _averageTradePrice = total / count;
        else
            _averageTradePrice = 0;
    }

    public void PrintStatistics()
    {
        Console.WriteLine($"{_trader.Username} Statistics:");
        
        Console.WriteLine($"Balance - Initial: {_initialBalance} | High: {_highestBalance} | Current: " +
                          $"{_trader.GetBalance()}");
        
        Console.Write("Overall Gains: ");
        if (_overallGains < 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{_overallGains}");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"+{_overallGains}"); 
        }
        Console.ResetColor();
        
        Console.WriteLine($"Average Sell Trade Value: {_averageTradePrice}");
    }
}
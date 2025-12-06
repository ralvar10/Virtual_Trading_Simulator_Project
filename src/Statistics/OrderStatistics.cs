using Virtual_Trading_Simulator_Project.Orders;

namespace Virtual_Trading_Simulator_Project.Statistics;

public class OrderStatistics : IStatistics
{
    private readonly Order _order;

    public OrderStatistics(Order order)
    {
        _order = order;
    }

    public void UpdateStatistics()
    {
    }

    public void PrintStatistics()
    {
        var strategies = string.Join(", ", _order.GetStrategies());
        
        string gainLossStr = "-";
        if (_order is SellOrder && _order.Status == OrderStatus.Filled)
        {
            gainLossStr = _order.Gain >= 0 ? $"+${_order.Gain:F2}" : $"-${Math.Abs(_order.Gain):F2}";
        }
        
        Console.WriteLine($"{_order.Time:MM/dd/yy HH:mm:ss,-20} {_order.OrderType,-5} {_order.Security.Symbol,-8} " +
                          $"{_order.Quantity,-8:F2} ${_order.Value,-11:F2} {gainLossStr,-12} {_order.Status,-10} {strategies,-15}");
    }
}
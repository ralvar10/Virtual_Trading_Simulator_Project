namespace Virtual_Trading_Simulator_Project.Orders.TradeStrategies;

public class LimitStrategy : ITradeStrategy
{
    public string StrategyName { get; }
    
    public double? LimitPrice { get; }
    
    public LimitStrategy(double? limitPrice)
    {
        if (limitPrice == null || limitPrice <= 0)
            throw new ArgumentException("Limit price must be positive");
        
        LimitPrice = limitPrice;
        this.StrategyName = "Limit";
    }
    
    public bool ShouldExecute(Order order)
    {
        double currentPrice = order.Security.GetPrice();
        
        if (order is BuyOrder)
        {
            return currentPrice <= LimitPrice;
        }
        
        if (order is SellOrder)
        {
            return currentPrice >= LimitPrice;
        }
        
        return false;
    }
}
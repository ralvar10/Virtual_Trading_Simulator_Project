namespace Virtual_Trading_Simulator_Project.Orders.TradeStrategies;

public class MarketStrategy : ITradeStrategy
{
    public string StrategyName { get; }

    public MarketStrategy()
    {
        StrategyName = "Market";
    }
    
    public bool ShouldExecute(Order order)
    {
        // Market orders always immediately place
        return true;
    }
}
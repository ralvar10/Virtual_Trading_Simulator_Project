namespace Virtual_Trading_Simulator_Project.Orders.TradeStrategies;

public interface ITradeStrategy
{
    public string StrategyName { get; }
    
    public bool ShouldExecute(Order order);
}
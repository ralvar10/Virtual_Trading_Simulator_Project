using Virtual_Trading_Simulator_Project.Orders.AccountingStrategies;
using Virtual_Trading_Simulator_Project.Orders.TradeStrategies;
using Virtual_Trading_Simulator_Project.Statistics;
using Virtual_Trading_Simulator_Project.Tickers;
using Virtual_Trading_Simulator_Project.Users;

namespace Virtual_Trading_Simulator_Project.Orders;


public abstract class Order
{
    public readonly string Id;
    public readonly string OrderType;
    public readonly Trader Trader;
    public readonly int Quantity;
    public readonly double Value;
    public readonly Ticker Security;
    public readonly IStatistics OrderStats;
    
    public readonly ITradeStrategy TradeStrategy;
    public readonly IAccountingStrategy AccountingStrategy;
    
    public DateTime Time {get;}
    public OrderStatus Status {get;}

    public abstract bool Validate();

    public abstract void PlaceOrder();
}
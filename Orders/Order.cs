using Virtual_Trading_Simulator_Project.Orders.TradeStrategies;
using Virtual_Trading_Simulator_Project.Statistics;
using Virtual_Trading_Simulator_Project.Tickers;
using Virtual_Trading_Simulator_Project.Users;

namespace Virtual_Trading_Simulator_Project.Orders;

public abstract class Order
{
    public Trader Trader {get;}
    public double Quantity {get;}
    public double Value {get;}
    public Ticker Security {get;}
    public OrderStatistics OrderStats {get;}
    
    public ITradeStrategy TradeStrategy {get;}
    
    public DateTime Time {get;}
    public OrderStatus Status {get; protected set;}

    protected Order(Trader trader, double quantity, Ticker security, ITradeStrategy tradeStrategy)
    {
        Trader = trader;
        
        if (quantity > 0)
            Quantity = quantity;
        else
            throw new ArgumentException("Quantity must be greater than zero");
        
        Security = security;
        TradeStrategy = tradeStrategy;

        OrderStats = new  OrderStatistics(this); 
        Time = DateTime.Now;
        Status = OrderStatus.Pending;
    }

    public abstract bool Validate();

    public abstract Task PlaceOrder();

    public void CancelOrder()
    {
        if (Status == OrderStatus.Pending)
        {
            Status = OrderStatus.Canceled;
        }
    }
}
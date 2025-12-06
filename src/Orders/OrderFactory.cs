using Virtual_Trading_Simulator_Project.Orders.AccountingStrategies;
using Virtual_Trading_Simulator_Project.Orders.TradeStrategies;
using Virtual_Trading_Simulator_Project.Tickers;
using Virtual_Trading_Simulator_Project.Users;

namespace Virtual_Trading_Simulator_Project.Orders;

public class OrderFactory
{
    private static OrderFactory? _instance;

    private OrderFactory(){}
    
    public static OrderFactory GetFactory()
    {
        if (_instance == null)
            _instance = new OrderFactory();
        
        return _instance;
    }

    public Order CreateOrder(Trader trader, double quantity, Ticker security, string orderType, 
        string tradeStrategy = "market", string accountingStrategy = "fifo", double? tradeStratParam = null)
    {
        ITradeStrategy trStrategy;
        IAccountingStrategy? accStrategy;
        
        switch (tradeStrategy.ToLowerInvariant())
        {
            case "market":
                trStrategy = new MarketStrategy();
                break;
            case "limit":
                trStrategy = new LimitStrategy(tradeStratParam);
                break;
            default:
                throw new ArgumentException("Unknown trade strategy");
        }
        
        switch (accountingStrategy.ToLowerInvariant())
        {
            case "fifo":
                accStrategy = new FifoStrategy();
                break;
            case "lifo":
                accStrategy = new LifoStrategy();
                break;
            default:
                if (orderType.ToLowerInvariant() == "buy")
                {
                    accStrategy = null;
                    break;
                }
                throw new ArgumentException("Unknown accounting strategy");
        }

        switch (orderType.ToLowerInvariant())
        {
            case  "buy":
                return new BuyOrder(trader, quantity, security, trStrategy);
            case "sell":
                return new SellOrder(trader, quantity, security, trStrategy, accStrategy);
            default:
                throw new ArgumentException("Unknown order type");
        }
    }
}
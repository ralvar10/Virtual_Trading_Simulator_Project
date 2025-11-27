using Virtual_Trading_Simulator_Project.Orders.AccountingStrategies;
using Virtual_Trading_Simulator_Project.Orders.TradeStrategies;
using Virtual_Trading_Simulator_Project.Tickers;
using Virtual_Trading_Simulator_Project.Users;

namespace Virtual_Trading_Simulator_Project.Orders;

public class SellOrder : Order
{
    public IAccountingStrategy AccountingStrategy {get;}
    
    public SellOrder(string id, Trader trader, double quantity, Ticker security, ITradeStrategy tradeStrategy,
        IAccountingStrategy accountingStrategy) : base(id, trader, quantity, security, tradeStrategy)
    {
        AccountingStrategy = accountingStrategy;
    }
}
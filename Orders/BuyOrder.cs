using Virtual_Trading_Simulator_Project.Orders.TradeStrategies;
using Virtual_Trading_Simulator_Project.Tickers;
using Virtual_Trading_Simulator_Project.Users;

namespace Virtual_Trading_Simulator_Project.Orders;

public class BuyOrder : Order
{
    public BuyOrder(Trader trader, double quantity, Ticker security, ITradeStrategy tradeStrategy)
        : base(trader, quantity, security, tradeStrategy){}
}
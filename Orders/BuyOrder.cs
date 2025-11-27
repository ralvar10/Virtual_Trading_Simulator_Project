namespace Virtual_Trading_Simulator_Project.Orders;

public class BuyOrder : Order
{
    public BuyOrder(string id, Trader trader, int quantity, Ticker security, ITradeStrategy tradeStrategy)
        : base(id, trader, quantity, security, tradeStrategy){}
}
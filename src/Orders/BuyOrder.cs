using Virtual_Trading_Simulator_Project.Orders.TradeStrategies;
using Virtual_Trading_Simulator_Project.Tickers;
using Virtual_Trading_Simulator_Project.Users;

namespace Virtual_Trading_Simulator_Project.Orders;

public class BuyOrder : Order
{
    public BuyOrder(Trader trader, double quantity, Ticker security, ITradeStrategy tradeStrategy)
        : base(trader, quantity, security, tradeStrategy)
    {
        OrderType = "Buy";
    }
    

    public override bool Validate()
    {
        double requiredBalance = Math.Round(Security.GetPrice() * Quantity, 2);
        
        if (Trader.GetBalance() < requiredBalance)
        {
            Console.WriteLine($"Insufficient balance. Required: ${requiredBalance:F2}, Available: ${Trader.GetBalance():F2}");
            return false;
        }

        return true;
    }

    public override async Task PlaceOrder()
    {
        while (!TradeStrategy.ShouldExecute(this) && Status == OrderStatus.Pending)
        {
            await Task.Delay(500);
        }
        
        if (Status == OrderStatus.Canceled)
        {
            Console.WriteLine("Order canceled!");
            return;
        }
        
        if (Status != OrderStatus.Pending)
        {
            throw new InvalidOperationException("Order cannot be placed again");
        }
        
        if (Validate())
        {
            try
            {
                double executionPrice = Security.GetPrice();
                double totalCost = Math.Round(executionPrice * Quantity, 2);
                
                Value = totalCost;
                
                Trader.UpdateBalance(-totalCost);
                
                Trader.GetHoldings().AddHolding(Security, Quantity, DateTime.Now, executionPrice);
                
                Status = OrderStatus.Filled;
                
                Console.WriteLine($"Buy order filled: {Quantity} shares of {Security.Symbol} at ${executionPrice:F2}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Order execution failed: {ex.Message}");
                Status = OrderStatus.Failed;
            }
        }
        else
        {
            Status = OrderStatus.Failed;
        }
    }
    
    public override List<string> GetStrategies()
    {
        return new List<string>{TradeStrategy.StrategyName};
    }
}
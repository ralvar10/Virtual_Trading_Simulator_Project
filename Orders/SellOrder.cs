using Virtual_Trading_Simulator_Project.Orders.AccountingStrategies;
using Virtual_Trading_Simulator_Project.Orders.TradeStrategies;
using Virtual_Trading_Simulator_Project.Tickers;
using Virtual_Trading_Simulator_Project.Users;
using Virtual_Trading_Simulator_Project.Users.Holdings;

namespace Virtual_Trading_Simulator_Project.Orders;

public class SellOrder : Order
{
    public IAccountingStrategy AccountingStrategy {get;}
    
    public SellOrder(Trader trader, double quantity, Ticker security, ITradeStrategy tradeStrategy,
        IAccountingStrategy accountingStrategy) : base(trader, quantity, security, tradeStrategy)
    {
        AccountingStrategy = accountingStrategy;
    }
    
    public override bool Validate()
    {
        try
        {
            List<Holding> holdingsWithSymbol = Trader.GetHoldings().SearchBySymbol(Security.Symbol);
            List<Holding> holdings = AccountingStrategy.SelectHoldings(Quantity, holdingsWithSymbol);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
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
        
        if ( Status != OrderStatus.Pending)
        {
            Console.WriteLine("Order cannot be placed again!");
            return;
        }
        
        if (Validate())
        {
            double numRemoved = 0;
            List<Holding> holdingsWithSymbol = Trader.GetHoldings().SearchBySymbol(Security.Symbol);
            List<Holding> holdings = new List<Holding>();

            try
            {
                holdings = AccountingStrategy.SelectHoldings(Quantity, holdingsWithSymbol);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
                Status = OrderStatus.Failed;
                return;
            }
            
            double price = Security.GetPrice();

            Value = price * Quantity;
            
            foreach (Holding holding in holdings)
            {
                double toRemove = Math.Min(Quantity - numRemoved,  holding.Quantity);
                Trader.UpdateBalance(price * toRemove);
                Trader.GetHoldings().DecrementHolding(holding, toRemove);
                numRemoved += toRemove;
                
                if (numRemoved >= Quantity)
                {
                    break;
                }
            }
            
            Status = OrderStatus.Filled;
            Console.WriteLine($"Sell order filled: {Quantity} shares of {Security.Symbol} at ${Value}");
        }
        else
        {
            Status = OrderStatus.Failed;
            Console.WriteLine($"ERROR Sell Order failed: {Quantity} shares of {Security.Symbol} at ${Value}");
        }
    }
}
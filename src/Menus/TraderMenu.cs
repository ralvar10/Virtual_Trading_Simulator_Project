using Virtual_Trading_Simulator_Project.Orders;
using Virtual_Trading_Simulator_Project.Tickers;
using Virtual_Trading_Simulator_Project.Tickers.TickerRepositories;
using Virtual_Trading_Simulator_Project.TickHandlers;
using Virtual_Trading_Simulator_Project.Users;

namespace Virtual_Trading_Simulator_Project.Menus;

public class TraderMenuService
{
    private readonly ITickerRepository _tickerRepo;
    private readonly ITickHandler _tickHandler;
    private readonly OrderFactory _orderFactory;

    public TraderMenuService(
        ITickerRepository tickerRepo,
        ITickHandler tickHandler,
        OrderFactory orderFactory)
    {
        _tickerRepo = tickerRepo;
        _tickHandler = tickHandler;
        _orderFactory = orderFactory;
    }

    public bool ShowTraderMenu(Trader trader)
    {
        Console.WriteLine("\n=== Trader Menu ===");
        Console.WriteLine($"Balance: ${trader.GetBalance():F2}");
        Console.WriteLine("1. View Available Tickers");
        Console.WriteLine("2. Place Order");
        Console.WriteLine("3. Manage Pending Orders");
        Console.WriteLine("4. View Order History");
        Console.WriteLine("5. View Current Holdings");
        Console.WriteLine("6. View Statistics");
        Console.WriteLine("7. Logout");
        Console.Write("Select option: ");

        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                DisplayTickers();
                return false;
            case "2":
                PlaceOrder(trader);
                return false;
            case "3":
                ManagePendingOrders(trader);
                return false;
            case "4":
                DisplayOrderHistory(trader);
                return false;
            case "5":
                DisplayCurrentHoldings(trader);
                return false;
            case "6":
                DisplayUserStats(trader);
                return false;
            case "7":
                return true; // tell MenuManager to log out
            default:
                Console.WriteLine("Invalid option!");
                return false;
        }
    }
    
    private void DisplayTickers()
    {
        var tickers = _tickerRepo.GetTickers();
    
        if (tickers.Count == 0)
        {
            Console.WriteLine("No tickers available.");
            return;
        }
    
        Console.WriteLine("\n=== Available Tickers ===");
    
        foreach (var ticker in tickers)
        {
            ticker.Statistics.PrintStatistics();
        }
    
        Console.WriteLine($"Total Tickers: {tickers.Count}");
    }

    private void DisplayOrderHistory(Trader trader)
    {
        var orders = trader.GetOrderHistory();
    
        if (orders.Count == 0)
        {
            Console.WriteLine("No orders found.");
            return;
        }
    
        Console.WriteLine("\n=== Order History ===");
        Console.WriteLine($"{"Time",-20} {"Type",-5} {"Symbol",-8} {"Qty",-8} {"Value",-12} {"Gain/Loss",-12} {"Status",-10} {"Strategy",-15}");
        Console.WriteLine(new string('-', 100));
    
        foreach (var order in orders)
        {
            order.OrderStats.PrintStatistics();
        }
    
        Console.WriteLine(new string('-', 100));
    }

    private void DisplayUserStats(Trader trader)
    {
        trader.Statistics.PrintStatistics();
    }

    private void DisplayCurrentHoldings(Trader trader)
    {
        trader.GetHoldings().Stats.PrintStatistics();
    }

    private void PlaceOrder(Trader trader)
    {
        try
        {
            Console.Clear();
            DisplayTickers();
            
            Console.Write("\nEnter ticker symbol: ");
            string? symbol = Console.ReadLine()?.ToUpper();
            
            if (string.IsNullOrEmpty(symbol))
            {
                Console.Clear();
                Console.WriteLine("Invalid ticker symbol!");
                return;
            }
            
            Ticker? ticker = _tickerRepo.SearchBySymbol(symbol);
            if (ticker == null)
            {
                Console.Clear();
                Console.WriteLine($"'{symbol}' not found!");
                return;
            }
            
            Console.Write("Order type (Buy/Sell): ");
            string? orderType = Console.ReadLine()?.ToLower();
            
            if (orderType != "buy" && orderType != "sell")
            {
                Console.Clear();
                Console.WriteLine("Invalid order type! Must be 'Buy' or 'Sell'");
                return;
            }
            
            Console.Write("Quantity: ");
            if (!double.TryParse(Console.ReadLine(), out double quantity) || quantity <= 0)
            {
                Console.Clear();
                Console.WriteLine("Invalid quantity! Must be a positive number.");
                return;
            }
            
            Console.Write("Trade strategy (Market/Limit) (default: Market): ");
            string? tradeStrategy = Console.ReadLine()?.ToLower();
            if (string.IsNullOrEmpty(tradeStrategy))
            {
                tradeStrategy = "market";
            }
            
            double? limitPrice = null;
            if (tradeStrategy == "limit")
            {
                Console.Write("Limit price: ");
                if (!double.TryParse(Console.ReadLine(), out double price) || price <= 0)
                {
                    Console.WriteLine("Invalid limit price!");
                    return;
                }
                limitPrice = price;
            }
            
            string? accountingStrategy = "fifo";
            if (orderType == "sell")
            {
                Console.Write("Accounting strategy (FIFO/LIFO) (default: FIFO): ");
                accountingStrategy = Console.ReadLine()?.ToLower();
                if (string.IsNullOrEmpty(accountingStrategy))
                {
                    accountingStrategy = "fifo";
                }
                
                if (accountingStrategy != "fifo" && accountingStrategy != "lifo")
                {
                    Console.WriteLine("Invalid accounting strategy! Using FIFO.");
                    accountingStrategy = "fifo";
                }
            }
            
            Order order = _orderFactory.CreateOrder(trader, (int)quantity, ticker, orderType, tradeStrategy, 
                accountingStrategy, limitPrice);
            
            _tickHandler.PauseTicks();
            
            Console.WriteLine($"\n=== Order Summary ===");
            Console.WriteLine($"Type: {orderType.ToUpper()}");
            Console.WriteLine($"Symbol: {symbol}");
            Console.WriteLine($"Quantity: {quantity}");
            Console.WriteLine($"Current price: ${ticker.GetPrice()}");
            Console.WriteLine($"Estimated value: ${ticker.GetPrice() * quantity}");
            Console.WriteLine($"Strategy: {tradeStrategy.ToUpper()}");
            if (limitPrice.HasValue)
            {
                Console.WriteLine($"Limit price: ${limitPrice.Value}");
            }
            if (orderType == "sell")
            {
                Console.WriteLine($"Accounting: {accountingStrategy.ToUpper()}");
            }
            Console.WriteLine("=====================");
            
            Console.Write("\nConfirm order? (y/n): ");
            string? confirm = Console.ReadLine()?.ToLower();
            
            if (confirm == "y")
            {
                bool success = trader.PlaceOrder(order);
                
                if (success)
                {
                    Console.WriteLine("\nOrder placed successfully!");
                    
                    if (tradeStrategy == "market")
                    {
                        Console.WriteLine("Market order has been executed.");
                    }
                    else
                    {
                        Console.WriteLine($"Limit order is pending. Will execute when price {(orderType == "buy" ? "drops to or below" : "rises to or above")} ${limitPrice}");
                    }
                }
                else
                {
                    Console.WriteLine("\nOrder failed to place. Please check the error messages above.");
                }
            }
            else
            {
                Console.WriteLine("\nOrder cancelled.");
            }
            
            _tickHandler.StartTicks();
        }
        catch (Exception e)
        {
            _tickHandler.StartTicks();
            Console.WriteLine($"\nError placing order: {e.Message}");
        }
    }

    private void ManagePendingOrders(Trader trader)
    {
        _tickHandler.PauseTicks();
        var allOrders = trader.GetOrderHistory();
        List<Order> pendingOrders = allOrders.Where(o => o.Status == OrderStatus.Pending).ToList();
        
        if (pendingOrders.Count == 0)
        {
            Console.WriteLine("\nNo pending orders.");
            _tickHandler.StartTicks();
            return;
        }
        
        Console.WriteLine("\n=== Pending Orders ===");
        Console.WriteLine($"{"#",-4} {"Time",-20} {"Type",-5} {"Symbol",-8} {"Qty",-8} {"Strategy",-15}");
        Console.WriteLine(new string('-', 75));
        
        for (int i = 0; i < pendingOrders.Count; i++)
        {
            var order = pendingOrders[i];
            var strategies = string.Join(", ", order.GetStrategies());
            Console.WriteLine($"{i + 1,-4} {order.Time,-20} {order.OrderType,-5} {order.Security.Symbol,-8} {order.Quantity,-8} {strategies,-15}");
        }
        
        Console.WriteLine(new string('-', 75));
        Console.Write("\nEnter order number to cancel (or 0 to go back): ");
        
        if (int.TryParse(Console.ReadLine(), out int orderNum) && orderNum > 0 && orderNum <= pendingOrders.Count)
        {
            pendingOrders[orderNum - 1].CancelOrder();
            Console.WriteLine("Order cancelled successfully!");
        }
        else if (orderNum != 0)
        {
            Console.WriteLine("Invalid order number!");
        }
        _tickHandler.StartTicks();
    }
}
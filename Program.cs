using Virtual_Trading_Simulator_Project.FileHandlers;
using Virtual_Trading_Simulator_Project.Orders;
using Virtual_Trading_Simulator_Project.Tickers.TickerRepositories;
using Virtual_Trading_Simulator_Project.TickHandlers;
using Virtual_Trading_Simulator_Project.Users;

public class Program
{
    private List<User> _users;
    private User? _loggedInUser;
    private TickerFileHandler _tickerFileHandler;
    private PortfolioFileHandler _portfolioFileHandler;
    private ITickerRepository _tickerRepo;
    private ITickHandler _tickHandler;
    private OrderFactory _orderFactory;
    
    public static void Main(string[] args)
    {
       
    }

    public Program()
    {
    _users = new List<User>(); 
    _loggedInUser = null;
    _tickerFileHandler = new TickerFileHandler();
    _portfolioFileHandler  = new PortfolioFileHandler();
    _tickerRepo = StockMarket.GetRepository();
    _tickHandler = StockTickHandler.GetInstance();
    _orderFactory =  OrderFactory.GetFactory();
    }

    private void LoadFromFiles()
    {
        throw new NotImplementedException();
    }

    private User? Login()
    {
        Console.WriteLine("Please enter your: ");
        Console.WriteLine("Username: ");
        string? username = Console.ReadLine();
        while (string.IsNullOrEmpty(username))
        {
            username = Console.ReadLine();
            if (string.IsNullOrEmpty(username))
            {
                Console.WriteLine("Please Enter a Valid Username!");
            }
            
        }
        
        Console.WriteLine("Password: ");
        string? password = Console.ReadLine();
        if (string.IsNullOrEmpty(password))
        {
            Console.WriteLine("Please Enter a Valid Password!");
        }
        
        User? foundUser = _users.Find(u => u.Username == username);

        if (foundUser == null)
        {
            Console.WriteLine($"Username or password is incorrect!");
            return null;
        }

        if (foundUser.Login(password))
        {
            Console.Clear();
            Console.WriteLine($"Successfully logged in as {foundUser.Username}!");
            return foundUser;
        }
        else
        {
            Console.WriteLine("Username or password is incorrect!");
            return null;
        }
    }

    private void LogOut()
    {
        Console.WriteLine($"Logged Out: {_loggedInUser?.Username}");
        _loggedInUser = null;
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
        
    private void DisplayUserStats(Trader trader)
    {
        trader.Statistics.PrintStatistics();
    }

    private void DisplayCurrentHoldings(Trader trader)
    {
        
    }

    private void PlaceOrder()
    {
        
    }

    private void ManagePendingOrders()
    {
        
    }

    private void ManageTickers()
    {
        
    }

    private void UpdateTickSpeed()
    {
        
    } 
}
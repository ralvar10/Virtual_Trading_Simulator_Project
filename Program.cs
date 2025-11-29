using Virtual_Trading_Simulator_Project.FileHandlers;
using Virtual_Trading_Simulator_Project.Orders;
using Virtual_Trading_Simulator_Project.Tickers.TickerRepositories;
using Virtual_Trading_Simulator_Project.TickHandlers;
using Virtual_Trading_Simulator_Project.Users;

public class Program
{
    private List<User> _users;
    private User _loggedInUser;
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

    private void DisplayOrderHistory()
    {
        
    }

    private void DisplayTickers()
    {
        
    }
        
    private void DisplayUserStats()
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
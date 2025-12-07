using Virtual_Trading_Simulator_Project.FileHandlers;
using Virtual_Trading_Simulator_Project.Menus;
using Virtual_Trading_Simulator_Project.Orders;
using Virtual_Trading_Simulator_Project.Tickers.TickerRepositories;
using Virtual_Trading_Simulator_Project.TickHandlers;
using Virtual_Trading_Simulator_Project.Users;

namespace Virtual_Trading_Simulator_Project.Menus;

public class MenuManager
{
    private readonly List<User> _users;
    private User? _loggedInUser;
    private readonly TickerFileHandler _tickerFileHandler;
    private readonly UserFileHandler _userFileHandler;
    private readonly ITickHandler _tickHandler;

    private readonly AdminMenu _adminMenuService;
    private readonly TraderMenuService _traderMenuService;

    public MenuManager(
        List<User> users,
        ITickerRepository tickerRepo,
        ITickHandler tickHandler,
        OrderFactory orderFactory,
        TickerFileHandler tickerFileHandler,
        UserFileHandler userFileHandler)
    {
        _users = users;
        _loggedInUser = null;
        _tickerFileHandler = tickerFileHandler;
        _userFileHandler = userFileHandler;
        _tickHandler = tickHandler;

        _adminMenuService = new AdminMenu(_users, tickerRepo, _tickHandler);
        _traderMenuService = new TraderMenuService(tickerRepo, _tickHandler, orderFactory);
    }

    public void Run()
    {
        LoadFromFiles();
        bool running = true;

        _tickHandler.StartTicks();

        Console.WriteLine("=== Virtual Trading Simulator ===");

        while (running)
        {
            if (_loggedInUser == null)
            {
                running = ShowLoginMenu();
            }
            else if (_loggedInUser is Admin admin)
            {
                bool shouldLogout = _adminMenuService.ShowAdminMenu(admin);
                if (shouldLogout)
                {
                    
                    LogOut();
                }
            }
            else if (_loggedInUser is Trader trader)
            {
                bool shouldLogout = _traderMenuService.ShowTraderMenu(trader);
                if (shouldLogout)
                {
                    LogOut();
                }
            }
        }

        _tickHandler.StopTicks();
        SaveToFiles();
        Console.WriteLine("Goodbye!");
    }



    private bool ShowLoginMenu()
    {
        Console.WriteLine("\n1. Login");
        Console.WriteLine("2. Exit");
        Console.Write("Select option: ");

        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                _loggedInUser = Login();
                return true;
            case "2":
                return false;
            default:
                Console.WriteLine("Invalid option!");
                return true;
        }
    }

    private void LoadFromFiles()
    {
        _tickerFileHandler.LoadFromFile();
        _userFileHandler.LoadFromFile();
    }

    private void SaveToFiles()
    {
        _tickerFileHandler.WriteToFile();
        _userFileHandler.WriteToFile();
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
        _loggedInUser?.LogOut();
        _loggedInUser = null;
    }
}
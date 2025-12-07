using Virtual_Trading_Simulator_Project.Tickers;
using Virtual_Trading_Simulator_Project.Tickers.TickerRepositories;
using Virtual_Trading_Simulator_Project.TickHandlers;
using Virtual_Trading_Simulator_Project.Users;

namespace Virtual_Trading_Simulator_Project.Menus;

public class AdminMenu
{
    private readonly List<User> _users;
    private readonly ITickerRepository _tickerRepo;
    private readonly ITickHandler _tickHandler;

    public AdminMenu(List<User> users, ITickerRepository tickerRepo, ITickHandler tickHandler)
    {
        _users = users;
        _tickerRepo = tickerRepo;
        _tickHandler = tickHandler;
    }

    public bool ShowAdminMenu(Admin admin)
    {
        Console.WriteLine("\n=== Admin Menu ===");
        Console.WriteLine("1. View All Tickers");
        Console.WriteLine("2. Manage Tickers");
        Console.WriteLine("3. Update Tick Speed");
        Console.WriteLine("4. Create Trader Account");
        Console.WriteLine("5. Logout");
        Console.Write("Select option: ");

        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                DisplayTickers();
                return false;
            case "2":
                ManageTickers(admin);
                return false;
            case "3":
                UpdateTickSpeed(admin);
                return false;
            case "4":
                CreateTrader(admin);
                return false;
            case "5":
                // signal to MenuManager to log out
                return true;
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

    private void CreateTrader(Admin admin)
    {
        try
        {
            Console.Write("\nEnter username for new trader: ");
            string? username = Console.ReadLine();
            
            if (string.IsNullOrEmpty(username))
            {
                Console.WriteLine("Invalid username!");
                return;
            }
            
            if (_users.Any(u => u.Username == username))
            {
                Console.WriteLine($"Username '{username}' already exists!");
                return;
            }
            
            Console.Write("Enter password: ");
            string? password = Console.ReadLine();
            
            if (string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Invalid password!");
                return;
            }
            
            Console.Write("Enter initial balance: ");
            if (!double.TryParse(Console.ReadLine(), out double balance) || balance < 0)
            {
                Console.WriteLine("Invalid balance!");
                return;
            }
            
            Trader newTrader = admin.CreateTrader(username, password, balance);
            _users.Add(newTrader);
            
            Console.WriteLine($"Trader account '{username}' created successfully with balance ${balance:F2}!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating trader: {ex.Message}");
        }
    }

    private void ManageTickers(Admin admin)
    {
        Console.WriteLine("\n=== Manage Tickers ===");
        Console.WriteLine("1. Add Ticker");
        Console.WriteLine("2. Remove Ticker");
        Console.WriteLine("3. Update Ticker Price");
        Console.WriteLine("4. Update Ticker Volatility");
        Console.WriteLine("5. Back");
        Console.Write("Select option: ");
        
        string? choice = Console.ReadLine();
        
        switch (choice)
        {
            case "1": // Adding ticker
                try
                {
                    Console.Write("\nEnter ticker symbol: ");
                    string? symbol = Console.ReadLine()?.ToUpper();
                    
                    if (string.IsNullOrEmpty(symbol))
                    {
                        Console.WriteLine("Invalid ticker symbol!");
                        break;
                    }
                    
                    Console.Write("Enter company name: ");
                    string? name = Console.ReadLine();
                    
                    if (string.IsNullOrEmpty(name))
                    {
                        Console.WriteLine("Invalid company name!");
                        break;
                    }
                    
                    Console.Write("Enter initial price: ");
                    if (!double.TryParse(Console.ReadLine(), out double price) || price <= 0)
                    {
                        Console.WriteLine("Invalid price!");
                        break;
                    }
                    
                    Console.Write("Enter volatility preset (Low/Medium/High) (default: Medium): ");
                    string? volatilityPreset = Console.ReadLine()?.ToLower();
                    if (string.IsNullOrEmpty(volatilityPreset))
                    {
                        volatilityPreset = "medium";
                    }
                    

                    Ticker newTicker;
                    switch (volatilityPreset)
                    {
                        case "low":
                            newTicker = new Ticker(symbol, name, price, 
                                new VolatilityParameters(0.01, 0.05, 0.02, 0));
                            break;
                        case "high":
                            newTicker = new Ticker(symbol, name, price,
                                new VolatilityParameters(0.05, 0.25, 0.1, 0));
                            break;
                        case "medium":
                        default:
                            newTicker = new Ticker(symbol, name, price);
                            break;
                    }
                    
                    if (admin.AddTicker(newTicker, _tickerRepo))
                    {
                        Console.WriteLine($"\nTicker '{symbol}' added successfully!");
                        Console.WriteLine($"  Company: {name}");
                        Console.WriteLine($"  Price: ${price:F2}");
                        Console.WriteLine($"  Volatility: {volatilityPreset}");
                    }
                    else
                    {
                        Console.WriteLine($"\nFailed to add ticker '{symbol}'.");
                        Console.WriteLine("  The symbol may already exist or be invalid.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding ticker: {ex.Message}");
                }
                break;
                
            case "2": // Remove Ticker
                DisplayTickers();
                
                Console.Write("\nEnter ticker symbol to remove: ");
                string? removeSymbol = Console.ReadLine()?.ToUpper();
                
                if (string.IsNullOrEmpty(removeSymbol))
                {
                    Console.WriteLine("Invalid ticker symbol!");
                    break;
                }
                
                Console.Write($"Are you sure you want to remove '{removeSymbol}'? (y/n): ");
                string? confirm = Console.ReadLine()?.ToLower();
                
                if (confirm == "y")
                {
                    if (admin.RemoveTicker(removeSymbol, _tickerRepo))
                    {
                        Console.WriteLine($"\nTicker '{removeSymbol}' removed successfully!");
                    }
                    else
                    {
                        Console.WriteLine($"\nFailed to remove ticker '{removeSymbol}'.");
                        Console.WriteLine("  The ticker may not exist.");
                    }
                }
                else
                {
                    Console.WriteLine("Removal cancelled.");
                }
                break;
                
            case "3": // Update price
                UpdateTickerPrice(admin);
                break;
            case "4": // Update volatility
                UpdateTickerVolatility(admin);
                break;
            case "5": // Exit
                break;
            default:
                Console.WriteLine("Invalid option!");
                break;
        }
    }

    private void UpdateTickerPrice(Admin admin)
    {
        DisplayTickers();
        
        Console.Write("\nEnter ticker symbol: ");
        string? symbol = Console.ReadLine()?.ToUpper();
        
        if (string.IsNullOrEmpty(symbol))
        {
            Console.WriteLine("Invalid ticker symbol!");
            return;
        }
        
        Ticker? ticker = _tickerRepo.SearchBySymbol(symbol);
        if (ticker == null)
        {
            Console.WriteLine($"Ticker '{symbol}' not found!");
            return;
        }
        
        Console.WriteLine($"Current price: ${ticker.GetPrice():F2}");
        Console.Write("Enter new price: ");
        
        if (!double.TryParse(Console.ReadLine(), out double newPrice) || newPrice <= 0)
        {
            Console.WriteLine("Invalid price!");
            return;
        }
        
        bool result = admin.UpdatePrice(newPrice, symbol, _tickerRepo);
        
        if (result)
        {
            Console.WriteLine($"\nPrice updated successfully!");
        }
        else
        {
            Console.WriteLine("\nFailed to update price.");
        }
    }

    private void UpdateTickerVolatility(Admin admin)
    {
        DisplayTickers();
        
        Console.Write("\nEnter ticker symbol: ");
        string? symbol = Console.ReadLine()?.ToUpper();
        
        if (string.IsNullOrEmpty(symbol))
        {
            Console.WriteLine("Invalid ticker symbol!");
            return;
        }
        
        Ticker? ticker = _tickerRepo.SearchBySymbol(symbol);
        if (ticker == null)
        {
            Console.WriteLine($"Ticker '{symbol}' not found!");
            return;
        }
        
        var currentVol = ticker.GetVolatility();
        Console.WriteLine($"\nCurrent Volatility Parameters:");
        Console.WriteLine($"  Min: {currentVol.MinVolatility:F3}");
        Console.WriteLine($"  Max: {currentVol.MaxVolatility:F3}");
        Console.WriteLine($"  Current: {currentVol.CurrentVolatility:F3}");
        Console.WriteLine($"  Sentiment: {currentVol.Sentiment:F3}");
        
        Console.WriteLine("\nEnter new volatility parameters:");
        
        Console.Write("Min volatility (0.001-0.999): ");
        if (!double.TryParse(Console.ReadLine(), out double min) || min <= 0 || min >= 1)
        {
            Console.WriteLine("Invalid min volatility!");
            return;
        }
        
        Console.Write("Max volatility (0.001-0.999): ");
        if (!double.TryParse(Console.ReadLine(), out double max) || max <= 0 || max >= 1)
        {
            Console.WriteLine("Invalid max volatility!");
            return;
        }
        
        Console.Write("Current volatility (0.001-0.999): ");
        if (!double.TryParse(Console.ReadLine(), out double current) || current <= 0 || current >= 1)
        {
            Console.WriteLine("Invalid current volatility!");
            return;
        }
        
        Console.Write("Sentiment (-1.0 to 1.0): ");
        if (!double.TryParse(Console.ReadLine(), out double sentiment) || sentiment < -1 || sentiment > 1)
        {
            Console.WriteLine("Invalid sentiment!");
            return;
        }
        
        var newParams = new VolatilityParameters(min, max, current, sentiment);
        
        if (admin.UpdateVolatility(newParams, symbol, _tickerRepo))
        {
            Console.WriteLine($"\nVolatility updated successfully for {symbol}!");
        }
        else
        {
            Console.WriteLine($"\nFailed to update volatility.");
        }
    }

    private void UpdateTickSpeed(Admin admin)
    {
        Console.WriteLine($"\nCurrent tick rate: {_tickHandler.GetTickRate()}ms");
        Console.Write("Enter new tick rate (ms, minimum 500): ");
        
        if (int.TryParse(Console.ReadLine(), out int newSpeed))
        {
            if (admin.UpdateTickSpeed(newSpeed, _tickHandler))
            {
                Console.WriteLine($"Tick speed updated to {newSpeed}ms");
            }
            else
            {
                Console.WriteLine("Invalid tick speed! Must be > 500ms");
            }
        }
        else
        {
            Console.WriteLine("Invalid input!");
        }
    }
}
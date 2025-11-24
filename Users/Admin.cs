namespace Virtual_Trading_Simulator_Project.Users;
using Virtual_Trading_Simulator_Project.Orders;
using Virtual_Trading_Simulator_Project.Tickers;


public class Admin
{
    public int Id {get; set;}
    public string Username {get; }
    private int _password;
    public string Role {get; } = "Admin";
    private static int _nextTraderId = 1;
    public static double TickSpeed {get; set;} = 1.0;

    public Admin(int id, string username, int password)
    {
        Id = id;
        Username = username ?? throw new ArgumentNullException(nameof(username));
        _password = password;
    }

    
    public Trader CreateTrader(int id, string username, int password, double initialBalance)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be null or empty.", nameof(username));
        }

        if (initialBalance < 0)
        {
            throw new ArgumentException("Initial balance cannot be negative.", nameof(initialBalance));
        }

        int traderId = _nextTraderId++;
        int defaultPassword = 0; // Default password set to 0

        return new Trader(traderId, username, defaultPassword, initialBalance);
    }

    public bool RemoveTicker(string symbol, ITickerRepository tickerRepository)
    {
        if (string.IsNullOrWhiteSpace(symbol))
        {
            throw new ArgumentException("Ticker symbol cannot be null or empty.", nameof(symbol));
        }

        if (tickerRepository == null)
        {
            throw new ArgumentNullException(nameof(tickerRepository));
        }

        var ticker = tickerRepository.GetTickerBySymbol(symbol);
        if (ticker == null)
        {
            return false; // Ticker not found
        }

        return tickerRepository.RemoveTicker(ticker);
    }

    public bool updateTickSpeed(double newSpeed)
    {
        if (newSpeed <= 0)
        {
            return false; // Invalid tick speed
        }

        TickSpeed = newSpeed;
        return true;
    }
}
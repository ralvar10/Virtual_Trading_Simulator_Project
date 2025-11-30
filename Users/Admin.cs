namespace Virtual_Trading_Simulator_Project.Users;
using Virtual_Trading_Simulator_Project.Orders;
using Virtual_Trading_Simulator_Project.Tickers;


public class Admin
{
    public string Username {get; }
    private string _password;
    public string Role {get; } = "Admin";

    public Admin(string username, string password)
    {
        Username = username ?? throw new ArgumentNullException(nameof(username));
        _password = password;
    }

    
    public Trader CreateTrader(string username, string password, double initialBalance)
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

        return new Trader(username, defaultPassword, initialBalance);
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

    public bool updateTickSpeed(int newSpeed)
    {
        if (newSpeed <= 500)
        {
            return false; // Invalid tick speed
        }

        TickSpeed = newSpeed;
        return true;
    }
}
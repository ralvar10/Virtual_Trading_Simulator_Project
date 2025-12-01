using Virtual_Trading_Simulator_Project.Tickers.TickerRepositories;
using Virtual_Trading_Simulator_Project.TickHandlers;
using Virtual_Trading_Simulator_Project.Orders;
using Virtual_Trading_Simulator_Project.Tickers;

namespace Virtual_Trading_Simulator_Project.Users;


public class Admin : User
{
    public string Username {get; }
    private string _password;
    public string Role {get; } = "Admin";

    public Admin(string username, string password) : base(username, password){}
    
    public Trader CreateTrader(string username, string password, double initialBalance)
    {
        return new Trader(username, password, initialBalance);
    }

    public bool AddTicker(Ticker ticker, ITickerRepository repository)
    {
        Ticker? exists = repository.SearchBySymbol(ticker.Symbol);
        
        if (exists != null)
        {
            return false; 
        }
        
        return repository.AddTicker(ticker);
    }
    
    public bool RemoveTicker(string symbol, ITickerRepository tickerRepository)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("Ticker symbol cannot be null or empty.", nameof(symbol));

        if (tickerRepository == null)
            throw new ArgumentNullException(nameof(tickerRepository));

        return tickerRepository.RemoveTicker(symbol);
    }

    public bool updateTickSpeed(int newSpeed, ITickHandler tickHandler)
    {
        if (newSpeed <= 500)
        {
            return false; // Invalid tick speed
        }

        return tickHandler.UpdateTickRate(newSpeed);
    }
}
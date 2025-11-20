using Virtual_Trading_Simulator_Project.Statistics;

namespace Virtual_Trading_Simulator_Project.Tickers;

public abstract class Ticker
{
    public readonly string Symbol;
    public readonly string Name;
    private double _price;
    private VolatilityParameters _volatility;
    public IStatistics TickerStatistics;
    
    private readonly object _priceLock = new object();
    private readonly object _volatilityLock = new object();

    public VolatilityParameters GetVolatility()
    {
        lock (_volatilityLock)
        {
            return _volatility;
        }
    }
    
    
    public VolatilityParameters UpdateVolatility()
    {
        lock (_volatilityLock)
        {
            
        }
    }

    public void SetVolatility(VolatilityParameters volatility)
    {
        lock (_volatilityLock)
        {
            _volatility = volatility;
        }
    }

    public double GetPrice()
    {
        lock (_priceLock)
        {
            return _price;
        }
    }

    public void SetPrice(double price)
    {
        lock (_priceLock)
        {
            _price = price;
        }
    }
    
    public double UpdatePrice()
    {
        lock (_priceLock)
        {
            
        }
    }
}
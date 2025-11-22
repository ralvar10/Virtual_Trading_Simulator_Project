using Virtual_Trading_Simulator_Project.Statistics;

namespace Virtual_Trading_Simulator_Project.Tickers;

public abstract class Ticker
{
    public readonly string Symbol;
    public readonly string Name;
    private double _price;
    private VolatilityParameters _volatility;
    public TickerStatistics Statistics;
    
    private readonly object _priceLock = new object();
    private readonly object _volatilityLock = new object();

    protected Ticker(string symbol, string name, double initialPrice)
    {
        Symbol = symbol;
        Name = name;
        _price = initialPrice;
        _volatility = new VolatilityParameters();
        Statistics = new TickerStatistics(this);
    }
    
    protected Ticker(string symbol, string name, double initialPrice, VolatilityParameters volatility)
    {
        Symbol = symbol;
        Name = name;
        _price = initialPrice;
        _volatility = new VolatilityParameters();
        Statistics = new TickerStatistics(this);
    }
    
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
            double targetVolatility = _volatility.MinVolatility + 
                (_volatility.MaxVolatility - _volatility.MinVolatility) * (0.5 + _volatility.Sentiment * 0.3);
            
            _volatility.CurrentVolatility += (targetVolatility - _volatility.CurrentVolatility) * 0.1;
            
            _volatility.CurrentVolatility = Math.Clamp(
                _volatility.CurrentVolatility, _volatility.MinVolatility, _volatility.MaxVolatility);
            
            return _volatility;
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
            Random random = new Random();

            double randomValue = random.NextDouble();
            
            // Allows ticker to have movements up or down with a bias towards sentiment 
            double sentimentBias = (_volatility.Sentiment + 1) / 2; 
            
            bool isPositive = randomValue < sentimentBias;

            double movementMagnitude = random.NextDouble() * 0.05;
            
            double volatilityScale = _volatility.CurrentVolatility / 0.02; 
            movementMagnitude *= volatilityScale;
            
            double changePercent = movementMagnitude;
            
            if (!isPositive)
            {
                changePercent = -changePercent;
            }
            
            _price = _price * (1 + changePercent);
            
            if (_price < 0.01)
            {
                _price = 0.01;
            }
            
            Statistics.UpdateStatistics();
            
            return _price;
        }
    }
}
using Virtual_Trading_Simulator_Project.Statistics;

namespace Virtual_Trading_Simulator_Project.Tickers;

public abstract class Ticker
{
    public readonly string Symbol;
    public readonly string Name;
    private double _price;
    private VolatilityParameters _volatility;
    public IStatistics TickerStatistics;

    public VolatilityParameters GetVolatility()
    {
        
    }
    
    
    public VolatilityParameters UpdateVolatility()
    {
        
    }

    public void SetVolatility(VolatilityParameters volatility)
    {
        
    }

    public double GetPrice()
    {
        
    }

    public void SetPrice(double price)
    {
        
    }
    
    public double UpdatePrice()
    {
        
    }
}
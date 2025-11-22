namespace Virtual_Trading_Simulator_Project.Tickers;

public abstract class TickerFactory
{
    public Ticker CreateTicker(string symbol, string name, double initialPrice, string VolatilityPreset)
    {
        switch (VolatilityPreset.ToLower())
        {
            case "high":
                return new Ticker(symbol, name, initialPrice,
                    new VolatilityParameters(-.25, .25, 0, 0));
            case "medium":
                return new Ticker(symbol, name, initialPrice);
            case "low":
                return new Ticker(symbol, name, initialPrice,
                    new VolatilityParameters(-.05, .05, 0, 0));
            default:
                /*
                 Consensus seems to be that this is the most clear error, although I don't know if it makes the 
                 most sense in this context.
                 */
                throw new NotImplementedException("Unknown volatility preset: " + VolatilityPreset);
        }
    }
    
    public Ticker CreateTicker(string symbol, string name, double initialPrice)
    {
        return CreateTicker(symbol, name, initialPrice, "medium");
    }
}
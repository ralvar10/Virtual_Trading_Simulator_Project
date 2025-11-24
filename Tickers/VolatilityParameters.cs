namespace Virtual_Trading_Simulator_Project.Tickers;

public struct VolatilityParameters
{
    public double MaxVolatility;
    public double MinVolatility;
    public double CurrentVolatility;
    public double Sentiment;

    public VolatilityParameters()
    {
        MaxVolatility = .1;
        MinVolatility = .03;
        CurrentVolatility = 0;
        Sentiment = 0;
    }
    
    public VolatilityParameters(double min, double max, double current, double sentiment)
    {
        MinVolatility = min;
        MaxVolatility = max;
        CurrentVolatility = current;
        Sentiment = sentiment;
    }
}
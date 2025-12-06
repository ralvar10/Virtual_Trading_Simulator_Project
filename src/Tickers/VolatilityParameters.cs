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
        MinVolatility = Math.Max(min, .001);
        MaxVolatility = Math.Min(max, .99);
        CurrentVolatility = Math.Max(current, .001);
        Sentiment = Math.Max(Math.Min(sentiment, .999), -.999);
    }
}
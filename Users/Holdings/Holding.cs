using Virtual_Trading_Simulator_Project.Statistics;
using Virtual_Trading_Simulator_Project.Tickers;

namespace Virtual_Trading_Simulator_Project.Users.Holdings;

public struct Holding
{
    public DateTime Time;
    public double InitialCost; // Per security as to allow quantity to be decremented without issue
    public double Quantity; // Made quantity a double to allow for potential partial trades of securities
    public readonly Ticker HoldingTicker;

    public Holding(Ticker ticker, double quantity)
    {
        HoldingTicker = ticker;
        Quantity = quantity;
        
        Time = DateTime.Now;
        InitialCost = HoldingTicker.GetPrice(); 
    }
}
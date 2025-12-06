using Virtual_Trading_Simulator_Project.Statistics;
using Virtual_Trading_Simulator_Project.Tickers;

namespace Virtual_Trading_Simulator_Project.Users.Holdings;

public class Holding
{
    public DateTime Time;
    public double InitialCost; // Per security as to allow quantity to be decremented without issue
    public double Quantity; // Made quantity a double to allow for potential partial trades of securities
    public readonly Ticker HoldingTicker;

    public Holding(Ticker ticker, double quantity, DateTime time, double initialCost)
    {
        HoldingTicker = ticker;
        Quantity = quantity;
        Time = time;
        InitialCost = initialCost; 
    }
    
    // Used to speed up and increase the accuracy of Holding comparisons 
    public override bool Equals(object? obj)
    {
        if (obj is not Holding holding)
        {
            return false;
        }
        
        // Math.Abs necessary to help with precision errors
        return Time == holding.Time && HoldingTicker.Symbol == holding.HoldingTicker.Symbol && 
               Math.Abs(Quantity - holding.Quantity) < .001 && Math.Abs(InitialCost - holding.InitialCost) < .001;
    }
    
    // Needs to be overriden if equals is overriden
    public override int GetHashCode()
    {
        // Combines time and Symbol to make a hashcode
        return HashCode.Combine(Time, HoldingTicker.Symbol);
    }
}
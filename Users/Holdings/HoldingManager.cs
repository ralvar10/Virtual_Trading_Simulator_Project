using Virtual_Trading_Simulator_Project.Statistics;
using Virtual_Trading_Simulator_Project.Tickers;

namespace Virtual_Trading_Simulator_Project.Users.Holdings;

public class HoldingManager
{
    private Dictionary<String, List<Holding>> _holdings;
    public HoldingStatistics Stats;
    
    public HoldingManager()
    {
        _holdings = new Dictionary<String, List<Holding>>();
    }

    public Dictionary<String, List<Holding>> AllHoldings()
    {
        return _holdings;
    }

    public List<Holding> SearchBySymbol(String ticker)
    {
        if (_holdings.ContainsKey(ticker))
            return _holdings[ticker];
        
        return new List<Holding>();
    }

    public void AddHolding(Ticker ticker, double quantity, DateTime time, double initialCost)
    {
        if (!_holdings.ContainsKey(ticker.Symbol))
        {
            _holdings[ticker.Symbol] = new List<Holding>();
        }

        _holdings[ticker.Symbol].Add(new Holding(ticker, quantity, time, initialCost));
    }

    public void RemoveHolding(Holding holding)
    {
        if (_holdings.ContainsKey(holding.HoldingTicker.Symbol))
        {
            _holdings[holding.HoldingTicker.Symbol].Remove(holding);
        }
    }

    public bool DecrementHolding(Holding holding, double amount)
    {
        if (!_holdings.ContainsKey(holding.HoldingTicker.Symbol))
            return false;

        List<Holding> holdings = _holdings[holding.HoldingTicker.Symbol];
        
        // For loop needed since parameter holding would be a copy
        for (int i = 0; i < holdings.Count; i++)
        {
    
            if (holdings[i].Time == holding.Time)
            {
                var current = holdings[i];
                    
                if (current.Quantity > amount)
                {
                    current.Quantity -= amount;
                    holdings[i] = current; 
                        
                    return true;
                }
                else if (current.Quantity == amount)
                {
                    holdings.RemoveAt(i);
                    return true;
                }
                else
                {
                    return false; 
                }
            }
        }

        return false;
    }

}
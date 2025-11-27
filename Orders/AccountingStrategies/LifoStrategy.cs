using Virtual_Trading_Simulator_Project.Users.Holdings;

namespace Virtual_Trading_Simulator_Project.Orders.AccountingStrategies;

public class LifoStrategy : IAccountingStrategy
{
    public List<Holding> SelectHoldings(double amount, List<Holding> holdings)
    {
        if (holdings == null || holdings.Count == 0)
            throw new ArgumentException("Holdings list cannot be null or empty");
        
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive");
        
        List<Holding> sortedHoldings = holdings.OrderByDescending(h => h.Time).ToList();
        List<Holding> selectedHoldings = new List<Holding>();
        double remaining = amount;

        foreach (var holding in sortedHoldings)
        {
            if (remaining <= 0)
                break;
            
            selectedHoldings.Add(holding);
            remaining -= holding.Quantity;
            
        }
        
        if (remaining > 0)
        {
            throw new InvalidOperationException($"Insufficient Holdings for Transaction");
        }
        
        return selectedHoldings;
    }
}
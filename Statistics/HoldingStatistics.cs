using Virtual_Trading_Simulator_Project.Users.Holdings;

namespace Virtual_Trading_Simulator_Project.Statistics;

public class HoldingStatistics : IStatistics
{
    private readonly HoldingManager _holdingManager;

    public HoldingStatistics(HoldingManager holdingManager)
    {
        _holdingManager = holdingManager;
    }

    public void UpdateStatistics(){}

    public void PrintStatistics()
    {
        Dictionary<string, List<Holding>> allHoldings = _holdingManager.AllHoldings();

        if (allHoldings.Count == 0)
        {
            Console.WriteLine("\nNo holdings found.\n");
            return;
        }

        Console.WriteLine("\n=== Current Holdings ===");
        Console.WriteLine($"{"Symbol",-8} {"Name",-25} {"Shares",-10} {"Avg Cost",-12} {"Current",-12} {"Value",-12} {"Gain/Loss",-15}");
        Console.WriteLine(new string('-', 100));

        double totalCost = 0;
        double totalValue = 0;

        foreach (KeyValuePair<string, List<Holding>> entry in allHoldings.OrderBy(key => key.Key))
        {
            string symbol = entry.Key;
            List<Holding> holdings = entry.Value;

            if (holdings.Count == 0)
                continue;

            double totalQuantity = holdings.Sum(h => h.Quantity);
            double totalCostBasis = holdings.Sum(h => h.InitialCost * h.Quantity);
            double avgCostBasis = totalCostBasis / totalQuantity;
            double currentPrice = holdings[0].HoldingTicker.GetPrice();
            double currentVal = currentPrice * totalQuantity;
            double gainLoss = currentVal - totalCostBasis;

            totalCost += totalCostBasis;
            totalValue += currentVal;

            Console.Write($"{symbol,-8} {holdings[0].HoldingTicker.Name,-25} {totalQuantity,-10} ${avgCostBasis,-11} ${currentPrice,-11} ${currentVal,-11} ");

            if (gainLoss >= 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"+${gainLoss,-14}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"-${Math.Abs(gainLoss),-14}");
            }
            Console.ResetColor();
        }

        Console.WriteLine(new string('-', 100));

        double gain = totalValue - totalCost;
        Console.Write($"Portfolio Total: ${totalValue} | Cost Basis: ${totalCost} | Total Gain/Loss: ");

        if (gain >= 0)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"+${gain}");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"-${Math.Abs(gain)}");
        }
        Console.ResetColor();
        Console.WriteLine();
    }
}
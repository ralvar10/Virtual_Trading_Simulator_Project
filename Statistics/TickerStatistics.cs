using Virtual_Trading_Simulator_Project.Tickers;

namespace Virtual_Trading_Simulator_Project.Statistics;

public class TickerStatistics : IStatistics
{
    private readonly Ticker _ticker;
    private List<double> _recentMovements, _recentValues; 
    private double _maxValue, _minValue, _lastValue;

    private readonly object _statsLock = new object();
    
    public TickerStatistics(Ticker ticker)
    {
        _ticker = ticker;
        
        Double currentValue = _ticker.GetPrice();
        
        _recentMovements = new List<double>(10);
        _recentValues = new List<double>(10);
        _maxValue = currentValue;
        _minValue = currentValue;
        _lastValue = currentValue;
    }

    public void UpdateStatistics()
    {
        lock (_statsLock)
        {
            Double currentValue = _ticker.GetPrice();

            if (currentValue > _maxValue)
            {
                _maxValue = currentValue;
            }

            if (currentValue < _minValue)
            {
                _minValue = currentValue;
            }

            _recentMovements.Add(Math.Round((currentValue - _lastValue) / _lastValue, 3));

            if (_recentMovements.Count > 10)
            {
                _recentMovements.RemoveAt(0);
            }
        }
    }

    public void PrintStatistics()
    {
        lock (_statsLock)
        {
            Console.WriteLine($"{_ticker.Name} ({_ticker.Symbol}) Statitics:");
            Console.WriteLine($"All Time Min: {_minValue} | Max: {_maxValue}");
            Console.WriteLine($"Average Movement: {_recentMovements.Average()}");
            Console.Write("Recent Movements: [");
            
            for (int i = 0 ; i < _recentMovements.Count; i++)
            {
                if (i != 0)
                {
                    Console.Write(" | ");
                }
                if (_recentMovements[i] < 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{_recentMovements[i]}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"{_recentMovements[i]}");
                }

                Console.ResetColor();
            }
            Console.Write("]");
        }
    }
}
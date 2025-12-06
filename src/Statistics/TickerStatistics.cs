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
            
            _recentValues.Add(currentValue);

            if (_recentValues.Count > 10)
            {
                _recentValues.RemoveAt(0);
            }
            
            _lastValue = currentValue;
        }
    }

    public void PrintStatistics()
    {
        lock (_statsLock)
        {
            Console.WriteLine($"\n{_ticker.Name} ({_ticker.Symbol})");
            Console.WriteLine($"Current: ${_ticker.GetPrice():F2} | All Time Min: ${_minValue:F2} | Max: ${_maxValue:F2}");
        
            Console.Write("Recent Values: [");
            for (int i = 0; i < _recentValues.Count; i++)
            {
                if (i != 0) Console.Write(" | ");
            
                if (i < _recentMovements.Count && _recentMovements[i] < 0)
                    Console.ForegroundColor = ConsoleColor.Red;
                else if (i < _recentMovements.Count && _recentMovements[i] > 0)
                    Console.ForegroundColor = ConsoleColor.Green;
                else
                    Console.ForegroundColor = ConsoleColor.Gray;
            
                Console.Write($"${_recentValues[i]:F2}");
                Console.ResetColor();
            }
            Console.Write("]");
        
            Console.Write("  Movements: [");
            for (int i = 0; i < _recentMovements.Count; i++)
            {
                if (i != 0) Console.Write(" | ");
            
                if (_recentMovements[i] < 0)
                    Console.ForegroundColor = ConsoleColor.Red;
                else if (_recentMovements[i] > 0)
                    Console.ForegroundColor = ConsoleColor.Green;
                else
                    Console.ForegroundColor = ConsoleColor.Gray;
            
                Console.Write($"{(_recentMovements[i] >= 0 ? "+" : "")}{(_recentMovements[i] * 100):F2}%");
                Console.ResetColor();
            }
            Console.WriteLine("]\n");
        }
    }
}
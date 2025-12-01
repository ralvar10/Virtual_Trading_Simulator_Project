using System.Text;
using Virtual_Trading_Simulator_Project.Tickers;
using Virtual_Trading_Simulator_Project.Tickers.TickerRepositories;

namespace Virtual_Trading_Simulator_Project.FileHandlers;

public class TickerFileHandler : IFileHandler
{
    private static TickerFileHandler? _instance;
    private readonly ITickerRepository _tickerRepository;
    
    private TickerFileHandler(ITickerRepository tickerRepository)
    {
        _tickerRepository = tickerRepository;
    }
    
    public static TickerFileHandler GetInstance(ITickerRepository repository)
    {
        if (_instance == null)
        {
            _instance = new TickerFileHandler(repository);
        }
        return _instance;
    }
    
    public IFileHandler GetFileHandler()
    {
        return this;
    }

    public bool LoadFromFile(string fileName)
    {
        try
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"No ticker file found at {fileName}. Starting with empty ticker list.");
                return false;
            }

            string[] lines = File.ReadAllLines(fileName);
            int loadedCount = 0;

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                try
                {
                    string[] parts = line.Split('|');

                    if (parts.Length != 7)
                    {
                        continue;
                    }
                    
                    string symbol = parts[0].Trim();
                    string name = parts[1].Trim();
                    double price = double.Parse(parts[2]);
                    double minVol = double.Parse(parts[3]);
                    double maxVol = double.Parse(parts[4]);
                    double currentVol = double.Parse(parts[5]);
                    double sentiment = double.Parse(parts[6]);
                    
                    var volatility = new VolatilityParameters(minVol, maxVol, currentVol, sentiment);
                    var ticker = new Ticker(symbol, name, price, volatility);
                    
                    if (_tickerRepository.AddTicker(ticker))
                    {
                        loadedCount++;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing line '{line}': {ex.Message}");
                }
            }

            Console.WriteLine($"Loaded {loadedCount} tickers from {fileName}");
            return loadedCount > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading tickers: {ex.Message}");
            return false;
        }
    }

    public bool WriteToFile(string fileName)
    {
        try
        {
            var tickers = _tickerRepository.GetTickers();
            
            StringBuilder sb = new StringBuilder();

            foreach (var ticker in tickers)
            {
                var volatility = ticker.GetVolatility();
                
                // Format: SYMBOL|Company Name|150.50|0.03|0.10|0.05|0.0
                string line = string.Join("|",
                    ticker.Symbol,
                    ticker.Name,
                    ticker.GetPrice().ToString("F2"),
                    volatility.MinVolatility.ToString("F4"),
                    volatility.MaxVolatility.ToString("F4"),
                    volatility.CurrentVolatility.ToString("F4"),
                    volatility.Sentiment.ToString("F4")
                );
                
                sb.AppendLine(line);
            }

            // Write to file
            File.WriteAllText(fileName, sb.ToString());
            
            Console.WriteLine($"Saved {tickers.Count} tickers to {fileName}");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving tickers: {ex.Message}");
            return false;
        }
    }
}
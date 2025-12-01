using Virtual_Trading_Simulator_Project.Tickers.TickerRepositories;
using Virtual_Trading_Simulator_Project.Tickers;

namespace Virtual_Trading_Simulator_Project.TickHandlers;

public class StockTickHandler : ITickHandler
{
    private static ITickHandler? _instance;
    private int _tickRate; // In milliseconds
    private bool _running;
    private const int MinTickRate = 500; 
    private CancellationTokenSource _cancellationTokenSource; // Used to end any extra threads on command

    private StockTickHandler()
    {
        _tickRate = 5000; // Default tick rate of 5 seconds 
        _running = false;
    }
    
    public static ITickHandler GetInstance()
    {
        if (_instance == null)
        {
                _instance = new StockTickHandler();
        }
        return _instance;
    }

    public bool UpdateTickRate(int tickRate)
    {
        if (tickRate > MinTickRate) // Minimum tick rate is somewhat trivial, half a second seems fair
        {
            _tickRate = tickRate;
            return true;
        }
        else
        {
            throw new ArgumentException($"Tick rate must be lower than {MinTickRate}");
        }
    }

    public int GetTickRate()
    {
        return _tickRate;
    }

    private void Tick()
    {
        // Gets all tickers
        List<Ticker> tickers = StockMarket.GetRepository().GetTickers();
    
        // Starts threads to work on updating each ticker 
        Parallel.ForEach(tickers, ticker =>
        {
            ticker.UpdatePrice();
            ticker.UpdateVolatility();
        });
    }

    public void PauseTicks()
    {
        // Sets running to false and kills threads
        _running = false;
        _cancellationTokenSource?.Cancel();
    }

    public void StartTicks()
    {
        // If running do nothing
        if (_running)
            return;
        
        // Otherwise set running to true and create a new CancellationTokenSource
        _running = true;
        _cancellationTokenSource = new CancellationTokenSource();
        
        // Create a thread to continually tick based on the TickRate until a CancellationToken is received
        Task.Run(async () =>
        {
            while (_running && !_cancellationTokenSource.Token.IsCancellationRequested)
            {
                Tick();
                await Task.Delay(_tickRate, _cancellationTokenSource.Token);
            }
        }, _cancellationTokenSource.Token);
    }
    
    public void StopTicks()
    {
        // If not already running do nothing
        if (!_running)
            return;
        
        // If running set running to false, end all threads, and get rid of all resources
        _running = false;
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
    }
}
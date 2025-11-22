namespace Virtual_Trading_Simulator_Project.Tickers.TickerRepositories;

public class StockMarket : ITickerRepository
{
    private List< Ticker > _tickers;
    private static ITickerRepository? _instance;

    public ITickerRepository GetRepository()
    {
        if (_instance == null)
        {
            _instance = new StockMarket();
            _tickers = new List<Ticker>();
        }
        return _instance;
    }

    public List<Ticker> GetTickers()
    {
        return _tickers;
    }

    public Ticker? SearchBySymbol(string symbol)
    {
        return _tickers.FirstOrDefault(t => t.Symbol == symbol);
    }

    public bool AddTicker(Ticker ticker)
    {
        if (!_tickers.Contains(ticker))
        {
            _tickers.Add(ticker);
            return true;
        }
        
        return false;
    }

    public bool RemoveTicker(string symbol)
    {
        Ticker? ticker = SearchBySymbol(symbol);

        if (ticker == null)
            return false;
        
        return _tickers.Remove(ticker);
    }
}
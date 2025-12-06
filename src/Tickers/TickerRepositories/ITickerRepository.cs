namespace Virtual_Trading_Simulator_Project.Tickers.TickerRepositories;

public interface ITickerRepository
{
    public static abstract ITickerRepository GetRepository();
    
    public List<Ticker> GetTickers();
    public Ticker? SearchBySymbol(string symbol); 
    public bool AddTicker(Ticker ticker);
    public bool RemoveTicker(string symbol);
}
using Virtual_Trading_Simulator_Project.FileHandlers;
using Virtual_Trading_Simulator_Project.Menus;
using Virtual_Trading_Simulator_Project.Orders;
using Virtual_Trading_Simulator_Project.Tickers.TickerRepositories;
using Virtual_Trading_Simulator_Project.TickHandlers;
using Virtual_Trading_Simulator_Project.Users;

namespace Virtual_Trading_Simulator_Project;

public class Program
{
    public static void Main(string[] args)
    {
        var users = new List<User>();
        ITickerRepository tickerRepo = StockMarket.GetRepository();
        ITickHandler tickHandler = StockTickHandler.GetInstance();
        OrderFactory orderFactory = OrderFactory.GetFactory();
        TickerFileHandler tickerFileHandler = TickerFileHandler.GetInstance(tickerRepo);
        UserFileHandler userFileHandler = UserFileHandler.GetInstance(users, tickerRepo);

        var menuManager = new MenuManager(
            users,
            tickerRepo,
            tickHandler,
            orderFactory,
            tickerFileHandler,
            userFileHandler);

        menuManager.Run();
    }
}
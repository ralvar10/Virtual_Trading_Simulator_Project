using System.Text;
using Virtual_Trading_Simulator_Project.Tickers.TickerRepositories;
using Virtual_Trading_Simulator_Project.Users;
using Virtual_Trading_Simulator_Project.Users.Holdings;

namespace Virtual_Trading_Simulator_Project.FileHandlers;

public class UserFileHandler : IFileHandler
{
    private static UserFileHandler? _instance;
    private readonly List<User> _users;
    private readonly ITickerRepository _tickerRepository;

    private UserFileHandler(List<User> users, ITickerRepository tickerRepository)
    {
        _users = users ?? throw new ArgumentNullException(nameof(users));
        _tickerRepository = tickerRepository ?? throw new ArgumentNullException(nameof(tickerRepository));
    }

    public static UserFileHandler GetInstance(List<User> users, ITickerRepository tickerRepository)
    {
        if (_instance == null)
        {
            _instance = new UserFileHandler(users, tickerRepository);
        }
        return _instance;
    }

    public IFileHandler GetFileHandler()
    {
        return this;
    }

    public bool LoadFromFile(string? fileName)
    {
        if (fileName == null)
            return false;
        
        try
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"No user file found at {fileName}.");
                return false;
            }

            string[] lines = File.ReadAllLines(fileName);
            _users.Clear();

            string currentSection = "";
            int userCount = 0;
            int holdingCount = 0;

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    currentSection = line.Trim();
                    continue;
                }

                try
                {
                    if (currentSection == "[USER]")
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length < 3) continue;

                        string userType = parts[0].Trim();
                        string username = parts[1].Trim();
                        string password = parts[2].Trim();

                        if (userType == "Admin")
                        {
                            _users.Add(new Admin(username, password));
                            userCount++;
                        }
                        else if (userType == "Trader" && parts.Length >= 4)
                        {
                            double balance = double.Parse(parts[3]);
                            _users.Add(new Trader(username, password, balance));
                            userCount++;
                        }
                    }
                    else if (currentSection == "[HOLDINGS]")
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length != 5) continue;

                        string username = parts[0].Trim();
                        string symbol = parts[1].Trim();
                        double quantity = double.Parse(parts[2]);
                        double initialCost = double.Parse(parts[3]);
                        DateTime purchaseTime = DateTime.Parse(parts[4]);

                        var trader = _users.OfType<Trader>().FirstOrDefault(t => t.Username == username);
                        var ticker = _tickerRepository.SearchBySymbol(symbol);
                        
                        if (trader != null && ticker != null)
                        {
                            trader.GetHoldings().AddHolding(ticker, quantity, purchaseTime, initialCost);
                            holdingCount++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing line: {ex.Message}");
                }
            }
            
            return userCount > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading users: {ex.Message}");
            return false;
        }
    }

    public bool WriteToFile(string fileName)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[USER]");

            int userCount = 0;
            foreach (var user in _users)
            {
                if (user is Admin admin)
                {
                    sb.AppendLine($"Admin|{admin.Username}|{admin.GetPassword()}");
                    userCount++;
                }
                else if (user is Trader trader)
                {
                    sb.AppendLine($"Trader|{trader.Username}|{trader.GetPassword()}|{trader.GetBalance():F2}");
                    userCount++;
                }
            }
            
            sb.AppendLine("[HOLDINGS]");

            int holdingCount = 0;
            foreach (var trader in _users.OfType<Trader>())
            {
                foreach (var entry in trader.GetHoldings().AllHoldings())
                {
                    foreach (var holding in entry.Value)
                    {
                        sb.AppendLine($"{trader.Username}|{entry.Key}|{holding.Quantity:F4}|{holding.InitialCost:F2}|{holding.Time:yyyy-MM-dd HH:mm:ss}");
                        holdingCount++;
                    }
                }
            }

            File.WriteAllText(fileName, sb.ToString());
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving users: {ex.Message}");
            return false;
        }
    }
}
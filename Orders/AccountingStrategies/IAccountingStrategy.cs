using Virtual_Trading_Simulator_Project.Users.Holdings;

namespace Virtual_Trading_Simulator_Project.Orders.AccountingStrategies;

public interface IAccountingStrategy
{
    public string StrategyName { get; }
    public List<Holding> SelectHoldings(double amount, List<Holding> holdings);
}
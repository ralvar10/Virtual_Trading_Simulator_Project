namespace Virtual_Trading_Simulator_Project.TickHandlers;

public interface ITickHandler
{
    private static ITickHandler? _instance;

    public static abstract ITickHandler GetInstance();
    public bool UpdateTickRate();
    public double GetTickRate();
    public void PauseTicks();
    public void ResumeTicks();
}
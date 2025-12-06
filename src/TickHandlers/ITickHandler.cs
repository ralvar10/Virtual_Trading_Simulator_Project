namespace Virtual_Trading_Simulator_Project.TickHandlers;

public interface ITickHandler
{
    public static abstract ITickHandler GetInstance();
    public bool UpdateTickRate(int tickRate);
    public int GetTickRate();
    public void PauseTicks();
    public void StartTicks();
    public void StopTicks();
}
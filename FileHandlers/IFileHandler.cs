namespace Virtual_Trading_Simulator_Project.FileHandlers;

public interface IFileHandler
{
    public IFileHandler GetFileHandler();
    
    public bool LoadFromFile(string fileName);
    
    public bool WriteToFile(string fileName);
}
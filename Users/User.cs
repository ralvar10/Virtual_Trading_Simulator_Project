namespace Virtual_Trading_Simulator_Project.Users;

public abstract class User
{
    private string _password;
    
    public int Id; 
    public string Username;

    protected User(string username, string password, int id) 
    {
        if (username.Length < 1 || password.Length < 1)
            throw new ArgumentException("Username or password is invalid");
        
        _password = password;
        Username = username;
        Id = id;
    }

    public bool Login(string password)
    {
        return password == _password;
    }
}
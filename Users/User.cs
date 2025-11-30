namespace Virtual_Trading_Simulator_Project.Users;

public abstract class User
{
    private readonly string _password;
    
    public readonly string Username;

    protected User(string username, string password, int id) 
    {
        if (string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(password) || 
            username.Length < 3 || password.Length < 3)
            throw new ArgumentException("Username or password is invalid");
        
        _password = password;
        Username = username;
    }

    public bool Login(string password)
    {
        return password == _password;
    }
}
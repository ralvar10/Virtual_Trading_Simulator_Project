namespace Virtual_Trading_Simulator_Project.Users;

public abstract class User
{
    private string _password;
    
    public int Id; 
    public string Username;

    protected User(string username, string password, int id) 
    {
        if (string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(password) || 
            username.Length < 3 || password.Length < 3)
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
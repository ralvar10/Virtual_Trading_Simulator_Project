namespace Virtual_Trading_Simulator_Project.Users;

public abstract class User
{
    private readonly string _password;
    public bool IsLoggedIn { get; private set; } = false;
    public readonly string Username;

    protected User(string username, string password) 
    {
        if (string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(password) || 
            username.Length < 3 || password.Length < 3)
            throw new ArgumentException("Username or password is invalid");
        
        _password = password;
        Username = username;
    }

    public bool Login(string? password)
    {
        if (password == null)
            return false;
        if (password.Equals(_password))
        {
            IsLoggedIn = true;
            return true;
        }
        return false;
    }

    public void LogOut()
    {
        IsLoggedIn = false;
    }

    public string GetPassword()
    {
        return _password;
    }
}
namespace Model;

public class UserData
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string UserPassword { get; set; }
    public string Salt { get; set; }
}
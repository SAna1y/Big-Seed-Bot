namespace Big_Seed_Bot.Api_Handler.GelbooruWrapper;

public class Authenticator
{
    public string Key { get; private set; }
    public string UserId { get; private set; }

    public Authenticator(string key, string userId)
    {
        Key = key;
        UserId = userId;
    }
}
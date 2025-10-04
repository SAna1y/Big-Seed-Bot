using System.Reflection;
using Big_Seed_Bot.Api_Handler.GelbooruWrapper;
using Big_Seed_Bot.Commands;
using Big_Seed_Bot.Utils;
using DisCatSharp;
using DisCatSharp.CommandsNext;
using DisCatSharp.Enums;

namespace Big_Seed_Bot;

internal class Program
{
    private static readonly string? Token = DotEnvReader.Read( Path.GetFullPath(".env"))?["TOKEN"];
    private static readonly string? APIKey = DotEnvReader.Read( Path.GetFullPath(".env"))?["GELBOORUKEY"];
    private static readonly string? UserID = DotEnvReader.Read( Path.GetFullPath(".env"))?["GELBOORUID"];
    
    
    private static void Main(string[] args)
    {
        if (Token is null || APIKey is null || UserID is null) 
            Console.WriteLine("A key was not provided. Set up your .env file."); 
        Authenticator gelbooruAuth = new Authenticator(APIKey, UserID);
        Wrapper wrapper = new Wrapper(gelbooruAuth);
        
        MainAsync().GetAwaiter().GetResult();
    }
    private static async Task MainAsync()
    {
        DiscordClient discord = new DiscordClient(new DiscordConfiguration()
        {
            Token = Token,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContent
        });
        
        CommandsSetup(discord);
        await discord.ConnectAsync();
        discord.RegisterEventHandlers(Assembly.GetExecutingAssembly());
        
        await Task.Delay(-1);
    }

    private static void CommandsSetup(DiscordClient discord)
    {
        CommandsNextExtension commands = discord.UseCommandsNext(new CommandsNextConfiguration() {
            StringPrefixes = ["."]
        });
        
        commands.RegisterCommands<UserCommandModule>();
        commands.RegisterCommands<ApiCommandModule>();
    }
}
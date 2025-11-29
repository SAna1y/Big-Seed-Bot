using Big_Seed_Bot.Api_Handler.GelbooruWrapper;
using Big_Seed_Bot.Commands;
using Big_Seed_Bot.Commands.CommandUtils;
using Big_Seed_Bot.Utils;
using DisCatSharp;
using DisCatSharp.CommandsNext;
using DisCatSharp.Entities;
using DisCatSharp.Enums;
using DisCatSharp.EventArgs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Big_Seed_Bot;

internal class Program
{
    private static readonly Dictionary<string, string>? _env = DotEnvReader.Read(Path.GetFullPath(".env"));
    private static readonly string Token = _env?["TOKEN"];
    private static readonly string APIKey = _env?["GELBOORUKEY"];
    private static readonly string UserID = _env?["GELBOORUID"];

    public static Random Rng = new Random();
    public static ILogger<BaseDiscordClient> logger;
    
    private static void Main(string[] args)
    {
        if (Token is null || APIKey is null || UserID is null)
        {
            Console.WriteLine("A key was not provided. Set up your .env file."); 
            return;
        }
        
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
        Authenticator gelbooruAuth = new Authenticator(APIKey, UserID);
        
        ServiceProvider services = new ServiceCollection()
            .AddSingleton<CommandService>()
            .AddSingleton(gelbooruAuth)
            .BuildServiceProvider();
        
        CommandsSetup(discord, services);
        
        discord.ComponentInteractionCreated += DiscordOnComponentInteractionCreated;
        await discord.ConnectAsync();
        await discord.UpdateStatusAsync(activity: new DiscordActivity("activity", ActivityType.Custom) {Name = "planting flowers"});
        logger = discord.Logger;
        
        await Task.Delay(-1);
    }

    private static void CommandsSetup(DiscordClient discord, ServiceProvider services)
    {
        CommandsNextExtension commands = discord.UseCommandsNext(new CommandsNextConfiguration() {
            StringPrefixes = ["."],
            ServiceProvider = services
        });
        
        commands.RegisterCommands<UserCommandModule>();
        commands.RegisterCommands<GelbooruCommandModule>();
        commands.RegisterCommands<NhentaiCommandModule>();
    }
    private static async Task DiscordOnComponentInteractionCreated(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        await EventHandlerUtil.RaiseDiscordButtonPressedEvent(sender, e);
    }
}
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

namespace Big_Seed_Bot;

internal class Program
{
    private static readonly Dictionary<string, string>? _env = DotEnvReader.Read(Path.GetFullPath(".env"));
    private static readonly string? Token = _env?["TOKEN"];
    private static readonly string? APIKey = _env?["GELBOORUKEY"];
    private static readonly string? UserID = _env?["GELBOORUID"];

    public static Random rng = new Random();
    public static Authenticator _gelbooruAuth {get; private set;}
    
    private static void Main(string[] args)
    {
        if (Token is null || APIKey is null || UserID is null)
        {
            Console.WriteLine("A key was not provided. Set up your .env file."); 
            return;
        }
        _gelbooruAuth = new Authenticator(APIKey, UserID);
        
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
        
        ServiceProvider services = new ServiceCollection()
            .AddSingleton<CommandService>()
            .BuildServiceProvider();
        
        CommandsSetup(discord, services);
        
        discord.ComponentInteractionCreated += DiscordOnComponentInteractionCreated;
        await discord.ConnectAsync();
        await discord.UpdateStatusAsync(activity: new DiscordActivity("activity", ActivityType.Custom) {Name = "planting flowers"});
        
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
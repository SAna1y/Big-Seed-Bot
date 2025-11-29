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

using static Big_Seed_Bot.Utils.DotEnvReader;

namespace Big_Seed_Bot;

internal class Program
{
    public static Random Rng = new Random();
    public static ILogger<BaseDiscordClient> logger;
    
    private static void Main(string[] args)
    {
        
        MainAsync().GetAwaiter().GetResult();
    }
    private static async Task MainAsync()
    {
        DiscordClient discord = new DiscordClient(new DiscordConfiguration()
        {
            Token = Env.GetEnvironmentVariable("TOKEN"),
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
        commands.RegisterCommands<PokemonCommandModule>();
    }
    private static async Task DiscordOnComponentInteractionCreated(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        await EventHandlerUtil.RaiseDiscordButtonPressedEvent(sender, e);
    }
}
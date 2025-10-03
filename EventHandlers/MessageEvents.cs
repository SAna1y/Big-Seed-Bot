using DisCatSharp;
using DisCatSharp.Enums;
using DisCatSharp.EventArgs;

namespace Big_Seed_Bot.EventHandlers;

[EventHandler]
public class MessageEvents
{
    [Event(DiscordEvent.MessageCreated)] 
    public static async Task Ping(DiscordClient s, MessageCreateEventArgs e) { 
        if (e.Message.Content.StartsWith("ping")) 
            await e.Channel.SendMessageAsync("pong"); 
    }
}
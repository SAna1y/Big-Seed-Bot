using DisCatSharp;
using DisCatSharp.EventArgs;

namespace Big_Seed_Bot.Utils;

public static class EventHandlerUtil
{
    public delegate Task DiscordButtonPressedHandler(DiscordClient sender, ComponentInteractionCreateEventArgs e);
    public static event DiscordButtonPressedHandler DiscordButtonPressed;

    public static Task RaiseDiscordButtonPressedEvent(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        DiscordButtonPressed?.Invoke(sender, e);
        return Task.CompletedTask;
    }
}
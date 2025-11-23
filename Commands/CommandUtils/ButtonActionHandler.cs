using DisCatSharp;
using DisCatSharp.Entities;
using DisCatSharp.EventArgs;

namespace Big_Seed_Bot.Commands.CommandUtils;

public class ButtonActionHandler
{
    public delegate Task ButtonActionDelegate(DiscordClient sender, ComponentInteractionCreateEventArgs e);
    public readonly Dictionary<string, ButtonActionDelegate> ButtonActions = new Dictionary<string, ButtonActionDelegate>();
    
    public ButtonActionHandler(params (DiscordButtonComponent, ButtonActionDelegate)[] buttonActions)
    {
        foreach ((DiscordButtonComponent button, ButtonActionDelegate) buttonAction in buttonActions)
        {
            if (string.IsNullOrWhiteSpace(buttonAction.button.CustomId)) continue;
            ButtonActions.Add(buttonAction.button.CustomId, buttonAction.Item2);
        }
    }
}
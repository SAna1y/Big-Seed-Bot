using Big_Seed_Bot.Api_Handler.GelbooruWrapper;
using Big_Seed_Bot.Api_Handler.Wrappers.Gelbooru;
using Big_Seed_Bot.Api_Handler.Wrappers.Responses;
using Big_Seed_Bot.Api_Handler.Wrappers.Responses.GelbooruResponses;
using Big_Seed_Bot.Commands.CommandUtils;
using Big_Seed_Bot.Utils;
using DisCatSharp;
using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;
using DisCatSharp.Entities;
using DisCatSharp.Enums;
using DisCatSharp.EventArgs;

namespace Big_Seed_Bot.Commands;

public class GelbooruCommandModule : BaseCommandModule
{
    public Authenticator _authenticator {private get; set; }
    private GelbooruClient _client;
    
    private readonly DiscordButtonComponent _deleteGelbooruPostButton = new DiscordButtonComponent(ButtonStyle.Danger, "deleteButton", "DELETE");
    private readonly ButtonActionHandler _buttonActionHandler;
    public CommandService CommandService {private get; set; }


    public GelbooruCommandModule()
    {
        _client = new GelbooruClient(_authenticator!);
        EventHandlerUtil.DiscordButtonPressed += OnButtonPressed;
        _buttonActionHandler = new ButtonActionHandler((_deleteGelbooruPostButton, OnDeleteButtonPressed));
    }

    [Command("goon")]
    public async Task GelbooruGetPostCommand(CommandContext ctx, params string[] searchText)
    {
        
        Response<GelbooruPostRoot> result = await _client.GetRandomPost(searchText);
        
        if (result.ApiResponse?.Posts is null)
        {
            await ctx.Channel.SendMessageAsync("error: " + result.Error);
            return;
        }

        DiscordLinkButtonComponent linkButton = new DiscordLinkButtonComponent(result.ApiResponse.GetUrl(), "gyat");
        
        DiscordMessageBuilder messageBuilder = new DiscordMessageBuilder()
            .WithContent(result.ApiResponse.GetFileUrl())
            .AddComponents(linkButton, _deleteGelbooruPostButton);
        
        
        CommandService.AddService(await messageBuilder.SendAsync(ctx.Channel), CommandService.BuildResponseContext(ctx, result.ApiResponse));
    }
    
    private async Task OnButtonPressed(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        if (!_buttonActionHandler.ButtonActions.TryGetValue(e.Id, out ButtonActionHandler.ButtonActionDelegate? function)) return;
        await function(sender, e);
    }

    private async Task OnDeleteButtonPressed(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        CommandService.ResponseContext current = CommandService.GetResponseContext(e.Message);
        if (current.Context is not null && current.Context.User.Id != e.User.Id) return;

        if (current.Context is not null && current.Context.Client.Guilds.TryGetValue(e.Guild.Id, out DiscordGuild? guild) && guild.Permissions is not null)
        {
            if (((Permissions)guild.Permissions).HasPermission(Permissions.ManageMessages))
                await current.Context.Message.DeleteAsync();
        }
        
        await e.Message.DeleteAsync();
        await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
    }
}
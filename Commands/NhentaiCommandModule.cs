using Big_Seed_Bot.Api_Handler.Wrappers.Nhentai;
using Big_Seed_Bot.Api_Handler.Wrappers.Responses;
using Big_Seed_Bot.Api_Handler.Wrappers.Responses.NhentaiResponses;
using Big_Seed_Bot.Commands.CommandUtils;
using Big_Seed_Bot.Utils;
using DisCatSharp;
using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;
using DisCatSharp.Entities;
using DisCatSharp.Enums;
using DisCatSharp.EventArgs;


namespace Big_Seed_Bot.Commands;

public class NhentaiCommandModule : BaseCommandModule
{
    private NhentaiClient _client = new NhentaiClient();

    private readonly DiscordButtonComponent _backwardButton =
        new DiscordButtonComponent(ButtonStyle.Primary, "backwardButton", emoji: new DiscordComponentEmoji("👈"));

    private readonly DiscordButtonComponent _forwardButton =
        new DiscordButtonComponent(ButtonStyle.Primary, "forwardButton", emoji: new DiscordComponentEmoji("👉"));

    private readonly DiscordButtonComponent _readButton =
        new DiscordButtonComponent(ButtonStyle.Success, "readButton", "Read");
    
    private readonly ButtonActionHandler _buttonActionHandler;
    public CommandService CommandService {private get; set; }

    public NhentaiCommandModule()
    {
        EventHandlerUtil.DiscordButtonPressed += OnDiscordButtonPressed;
        
        _buttonActionHandler = new ButtonActionHandler((_readButton, OnReadButtonPressed),
            (_forwardButton, OnTurnerButtonPressed),
            (_backwardButton, OnTurnerButtonPressed));
    }

    [Command("nhentai")]
    public async Task NhentaiGet(CommandContext ctx, int id)
    {
        Response<NhentaiPost> result = await _client.GetPostById(id);

        if (result.ApiResponse is null)
        {
            await ctx.Channel.SendMessageAsync(result.Error);
            return;
        }

        await ctx.Channel.SendMessageAsync(result.ApiResponse.GetUrl());
    }

    [Command("nsearch")]
    public async Task NhentaiSearch(CommandContext ctx, [RemainingText] string query)
    {
        Response<NhentaiPost> result = await _client.GetRandomPostBySearch(query);
        if (result.ApiResponse is null)
        {
            await ctx.Channel.SendMessageAsync(result.Error);
            return;
        }

        DiscordMessageBuilder builder = new DiscordMessageBuilder()
            .AddComponents(_readButton)
            .WithContent(result.ApiResponse.GetUrl());

        CommandService.AddService(await builder.SendAsync(ctx.Channel), CommandService.BuildResponseContext(ctx, result.ApiResponse, 1));
    }

    [Command("read")]
    public async Task NhentaiRead(CommandContext ctx, int id, int desiredPageNumber = 1)
    {
        Response<NhentaiPost> result = await _client.GetPostById(id);

        if (result.ApiResponse is null)
        {
            await ctx.Channel.SendMessageAsync(result.Error);
            return;
        }

        Task<DiscordEmbed> embedTask = await GetEmbedByPage(result.ApiResponse, desiredPageNumber);

        if (embedTask.Exception is not null)
        {
            await ctx.Channel.SendMessageAsync(embedTask.Exception.Message);
            return;
        }

        DiscordEmbed embed = embedTask.Result;

        DiscordMessageBuilder messageBuilder = new DiscordMessageBuilder()
            .AddEmbed(embed)
            .AddComponents(
                _backwardButton,
                _forwardButton
            );

        DiscordMessage message = await messageBuilder.SendAsync(ctx.Channel);
        
        CommandService.AddService(message, CommandService.BuildResponseContext(ctx, result.ApiResponse, desiredPageNumber));
    }

    private async Task<Task<DiscordEmbed>> GetEmbedByPage(NhentaiPost post, int desiredPageNumber)
    {
        desiredPageNumber = CheckPageNumber(post, desiredPageNumber);
        ImageType? desiredFormat = post.GetImageTypeOfPage(desiredPageNumber);

        if (desiredFormat is null)
        {
            return Task.FromException<DiscordEmbed>(new Exception("Wrong page number"));
        }

        Response<NhentaiImage> result = await _client.GetImage(post.MediaId ?? "", desiredPageNumber, desiredFormat);

        if (result.ApiResponse is null)
        {
            return Task.FromException<DiscordEmbed>(new Exception("no response"));
        }

        DiscordEmbedBuilder embed = new DiscordEmbedBuilder()
            .WithTitle(post.Title?.Pretty ?? "")
            .WithImageUrl(result.ApiResponse.GetUrl())
            .WithDescription(post.GetUrl())
            .AddField(new DiscordEmbedField("Page:", desiredPageNumber.ToString()));

        return Task.FromResult(embed.Build());
    }
    
    private async Task OnDiscordButtonPressed(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        if (!_buttonActionHandler.ButtonActions.TryGetValue(e.Id, out ButtonActionHandler.ButtonActionDelegate? function)) return;
        await function(sender, e);
    }

    private async Task OnReadButtonPressed(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        CommandService.ResponseContext current = CommandService.GetResponseContext(e.Message);
        if (current.Post is not NhentaiPost post) return;

        await NhentaiRead(current.Context, int.TryParse(post.Id!.ToString(), out int id) ? id : 0);
        await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
            new DiscordInteractionResponseBuilder().WithContent("Have fun :3"));
    }
    
    private async Task OnTurnerButtonPressed(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        CommandService.ResponseContext current = CommandService.GetResponseContext(e.Message);
        if (current.Post is not NhentaiPost post) return;
        
        int currentPageNumber = current.Page;
        int nextPageNumber = e.Id == "forwardButton" ? ++currentPageNumber : --currentPageNumber;

        Task<DiscordEmbed> embedTask = await GetEmbedByPage(post, nextPageNumber);

        if (embedTask.Exception is not null)
        {
            await e.Channel.SendMessageAsync(embedTask.Exception.Message);
            return;
        }

        DiscordEmbed embed = embedTask.Result;

        DiscordInteractionResponseBuilder responseBuilder = new DiscordInteractionResponseBuilder()
            .AddEmbed(embed)
            .AddComponents(
                _backwardButton,
                _forwardButton
            );

        CommandService.AddService(e.Message, CommandService.BuildResponseContext(current.Context, post, CheckPageNumber(post, nextPageNumber)));
        await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, responseBuilder);
    }

    ///returns the desired page number, 1 if it's over the max, and the last if it's less than 1
    /// otherwise doesn't change
    private int CheckPageNumber(NhentaiPost post, int desiredPageNumber)
    {
        if (desiredPageNumber > post.NumberOfPages) return 1;

        return desiredPageNumber < 1 ? post.NumberOfPages : desiredPageNumber;
    }
}

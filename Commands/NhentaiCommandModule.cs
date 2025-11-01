using Big_Seed_Bot.Api_Handler.Wrappers.Nhentai;
using Big_Seed_Bot.Api_Handler.Wrappers.Responses;
using Big_Seed_Bot.Api_Handler.Wrappers.Responses.NhentaiResponses;
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

    private DiscordButtonComponent _forwardButton = new DiscordButtonComponent(ButtonStyle.Primary, "backwardButton",
        emoji: new DiscordComponentEmoji("👈"));
    private DiscordButtonComponent _backwardButton =new DiscordButtonComponent(ButtonStyle.Primary, "forwardButton",
        emoji: new DiscordComponentEmoji("👉"));
    
    private static readonly Dictionary<DiscordMessage, Reading> ActiveReadings = new Dictionary<DiscordMessage, Reading>();

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
    public async Task NhentaiSearch(CommandContext ctx, params string[] query)
    {
        Response<NhentaiPost> result = await _client.GetRandomPostBySearch(string.Join(" ", query));
        if (result.ApiResponse is null) 
        {
            await ctx.Channel.SendMessageAsync(result.Error);
            return;
        }
        
        await ctx.Channel.SendMessageAsync(result.ApiResponse.GetUrl());
    }

    [Command("read")]
    public async Task NhentaiRead(CommandContext ctx, int id, int desiredPageNumber = 1)
    {
        ctx.Client.ComponentInteractionCreated -= ClientOnComponentInteractionCreated;
        Response<NhentaiPost> postResult = await _client.GetPostById(id);

        if (postResult.ApiResponse is null)
        {
            await ctx.Channel.SendMessageAsync(postResult.Error);
            return;
        }

        Task<DiscordEmbed> embedTask = await GetEmbedByPage(postResult.ApiResponse, desiredPageNumber);

        if (embedTask.Exception is not null)
        {
            await ctx.Channel.SendMessageAsync(embedTask.Exception.Message);
            return;
        }

        DiscordEmbed embed = embedTask.Result;

        DiscordMessageBuilder messageBuilder = new DiscordMessageBuilder()
            .AddEmbed(embed)
            .AddComponents(
                _forwardButton,
                _backwardButton
            );

        DiscordMessage message = await messageBuilder.SendAsync(ctx.Channel);
        AddReading(message, new Reading(postResult.ApiResponse, desiredPageNumber));

        ctx.Client.ComponentInteractionCreated += ClientOnComponentInteractionCreated;
    }

    private async Task ClientOnComponentInteractionCreated(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        Reading currentReading = ActiveReadings[e.Message];
        int currentPageNumber = currentReading.Page;
        int nextPageNumber = e.Id == "forwardButton" ? ++currentPageNumber : --currentPageNumber;

        Task<DiscordEmbed> embedTask = await GetEmbedByPage(currentReading.Post, nextPageNumber);

        if (embedTask.Exception is not null)
        {
            await e.Channel.SendMessageAsync(embedTask.Exception.Message);
            return;
        }

        DiscordEmbed embed = embedTask.Result;

        DiscordInteractionResponseBuilder responseBuilder = new DiscordInteractionResponseBuilder()
            .AddEmbed(embed)
            .AddComponents(
                _forwardButton,
                _backwardButton
            );
            
        ActiveReadings[e.Message] = new Reading(currentReading.Post, CheckPageNumber(currentReading.Post, nextPageNumber));
        await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, responseBuilder);
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

    ///returns the desired page number, 1 if it's over the max, and the last if it's less than 1
    /// otherwise doesn't change
    private int CheckPageNumber(NhentaiPost post, int desiredPageNumber)
    {
        if (desiredPageNumber > post.NumberOfPages) return 1;

        return desiredPageNumber < 1 ? post.NumberOfPages : desiredPageNumber;
    }

    private void AddReading(DiscordMessage message, Reading reading)
    {
        if (ActiveReadings.TryAdd(message, reading)) return;
        ActiveReadings[message] = reading;
    }


    private struct Reading
    {
        public NhentaiPost Post;
        public int Page;

        public Reading(NhentaiPost post, int page)
        {
            Post = post;
            Page = page;
        }
    }

}

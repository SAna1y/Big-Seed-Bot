using Big_Seed_Bot.Api_Handler.Wrappers.Nhentai;
using Big_Seed_Bot.Api_Handler.Wrappers.Responses;
using Big_Seed_Bot.Api_Handler.Wrappers.Responses.NhentaiResponses;
using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;
using DisCatSharp.Entities;
using DisCatSharp.Enums;


namespace Big_Seed_Bot.Commands;

public class NhentaiCommandModule : BaseCommandModule
{
    private NhentaiClient _client = new NhentaiClient();
    private int _desiredPageNumber = 1;

    [Command("nhentai")]
    public async Task NhentaiGet(CommandContext ctx, int id)
    {
        Response<NhentaiPost> result = await _client.GetPostById(id);
        
        if (result.ApiResponse is null)
        {
            await ctx.Channel.SendMessageAsync(result.Error??"No Posts Found");
            return;
        }

        await ctx.Channel.SendMessageAsync(result.ApiResponse.GetUrl());
    }

    [Command("read")]
    public async Task NhentaiRead(CommandContext ctx, int id)
    {
        Response<NhentaiPost> postResult = await _client.GetPostById(id);
        
        if (postResult.ApiResponse is null)
        {
            await ctx.Channel.SendMessageAsync(postResult.Error??"No Posts Found");
            return;
        }

        if (postResult.ApiResponse.ContainsBannedTag())
        {
            await ctx.Channel.SendMessageAsync("azt elhitted");
            return;
        }
        
        _desiredPageNumber = 1;
        DiscordEmbed? embed = await GetEmbedByPage(ctx, postResult.ApiResponse, id);
        
        if (embed is null)
        {
            await ctx.Channel.SendMessageAsync("valami szar");
            return;
        }

        DiscordMessageBuilder messageBuilder = new DiscordMessageBuilder()
            .AddEmbed(embed)
            .AddComponents(
                new DiscordButtonComponent(ButtonStyle.Primary, "backwardButton",
                    emoji: new DiscordComponentEmoji("👈")),
                new DiscordButtonComponent(ButtonStyle.Primary, "forwardButton", 
                    emoji: new DiscordComponentEmoji("👉"))
            );
        
        DiscordMessage message = await messageBuilder.SendAsync(ctx.Channel);
        
        ctx.Client.ComponentInteractionCreated += async (s, e) =>
        {
            switch (e.Id)
            {
                case "forwardButton":
                    embed = await GetEmbedByPage(ctx, postResult.ApiResponse, 1);
                    
                    if (embed is null)
                    {
                        return;
                    }

                    await message.ModifyAsync(embed);
                    break;
                case "backwardButton":
                    embed = await GetEmbedByPage(ctx, postResult.ApiResponse, -1);
                    
                    if (embed is null)
                    {
                        return;
                    }
                    
                    await message.ModifyAsync(embed);
                    break;
            }
        };
    }

    private async Task<DiscordEmbed?> GetEmbedByPage(CommandContext ctx, NhentaiPost post, int changeBy = 0)
    {
        _desiredPageNumber += changeBy;
        if (_desiredPageNumber > post.NumPages) _desiredPageNumber = 1;
        else if (_desiredPageNumber < 1) _desiredPageNumber = post.NumPages;
        
        
        ImageType? desiredFormat = post.GetImageTypeOfPage(_desiredPageNumber);

        if (desiredFormat is null)
        {
            await ctx.Channel.SendMessageAsync("wrong format");
            return null;
        }
        
        Response<NhentaiImage> result = await _client.GetImage(post.MediaId??"", _desiredPageNumber, desiredFormat);
        
        if (result.ApiResponse is null) 
        {
            await ctx.Channel.SendMessageAsync(result.Error ?? "Error");
            return null;
        }

        DiscordEmbedBuilder embed = new DiscordEmbedBuilder()
            .WithTitle(post.Title?.Pretty ?? "") 
            .WithImageUrl(result.ApiResponse.GetUrl())
            .WithDescription(post.GetUrl()) 
            .AddField(new DiscordEmbedField("Page:", _desiredPageNumber.ToString()));

        return embed.Build();
    }
}

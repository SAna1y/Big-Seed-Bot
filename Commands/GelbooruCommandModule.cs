using Big_Seed_Bot.Api_Handler.Wrappers.Gelbooru;
using Big_Seed_Bot.Api_Handler.Wrappers.Gelbooru.Responses;
using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;
using DisCatSharp.Entities;

namespace Big_Seed_Bot.Commands;

public class GelbooruCommandModule : BaseCommandModule
{
    private PostResult _lastResult;
    private DiscordMessage? _lastMessage;
    private GelbooruClient _client = new GelbooruClient(Program._gelbooruAuth);
    
    [Command("goon")]
    public async Task GelbooruGetPostCommand(CommandContext ctx, params string[] searchText)
    {
        
        PostResult result = await _client.GetRandomPost(searchText);
        _lastResult = result;
        
        if (result.Post is null)
        {
            await ctx.Channel.SendMessageAsync("error: " + result.Error);
            return;
        }
        
        _lastMessage = await ctx.Channel.SendMessageAsync(result.Post.file_url);
    }

    [Command("link")]
    public async Task GelbooruGetLastPostLink(CommandContext ctx)
    {
        if (_lastResult.Post is null)
        {
            await ctx.Channel.SendMessageAsync(_lastResult.Error??"No post found!");
            return;
        }

        string link = $"https://gelbooru.com/index.php?page=post&s=view&id={_lastResult.Post.id}";
        await ctx.Channel.SendMessageAsync($"<{link}>");
    }

    [Command("delete")]
    public async Task DeleteLastPost(CommandContext ctx)
    {
        if (_lastMessage is null)
        {
            await ctx.Channel.SendMessageAsync("No post found!");
            return;
        }

        await _lastMessage.DeleteAsync();
    }
}
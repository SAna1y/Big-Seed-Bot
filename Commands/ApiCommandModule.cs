using Big_Seed_Bot.Api_Handler.GelbooruWrapper;
using Big_Seed_Bot.Api_Handler.GelbooruWrapper.Responses;
using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;

namespace Big_Seed_Bot.Commands;

public class ApiCommandModule : BaseCommandModule
{
    private PostResult _lastResult;
    
    [Command("goon")]
    public async Task GelbooruGetPostCommand(CommandContext ctx, params string[] text)
    {
        if (Wrapper.WrapperInstance is null)
        {
            await ctx.Channel.SendMessageAsync("error");
            return;
        }

        string search = "";
        foreach (string word in text)
        {
            if (word.Any(c => c == ':')) search += word + " ";
            else if (word[0] == '-') search += word + "* ";
            else search += "*" + word + "* ";
        }

        PostResult result = await Wrapper.WrapperInstance.GetRandomPost(tags: search);
        _lastResult = result;
        
        if (result.Post is null)
        {
            await ctx.Channel.SendMessageAsync("error: " + result.Error);
            return;
        }

        await ctx.Channel.SendMessageAsync($"||{result.Post.file_url} ||");
    }

    [Command("link")]
    public async Task GelbooruGetLastPostLink(CommandContext ctx)
    {
        if (_lastResult.Post is null)
        {
            await ctx.Channel.SendMessageAsync("No post found!");
            return;
        }

        string link = $"https://gelbooru.com/index.php?page=post&s=view&id={_lastResult.Post.id}";
        await ctx.Channel.SendMessageAsync($"<{link}>");
        
    }
}
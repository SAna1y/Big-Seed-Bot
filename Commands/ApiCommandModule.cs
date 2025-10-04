using Big_Seed_Bot.Api_Handler.GelbooruWrapper;
using Big_Seed_Bot.Api_Handler.GelbooruWrapper.Responses;
using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;

namespace Big_Seed_Bot.Commands;

public class ApiCommandModule : BaseCommandModule
{
    [Command("gelbooru")]
    public async Task GelbooruGetPostCommand(CommandContext ctx)
    {
        if (Wrapper.WrapperInstance is null)
        {
            await ctx.Channel.SendMessageAsync("error");
            return;
        }
        
        Post? post = await Wrapper.WrapperInstance.GetRandomPost();
        if (post is null)
        {
            await ctx.Channel.SendMessageAsync("error");
            return;
        }

        await ctx.Channel.SendMessageAsync($"||{post.file_url} ||");
    }
}
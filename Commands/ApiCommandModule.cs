using Big_Seed_Bot.Api_Handler.GelbooruWrapper;
using Big_Seed_Bot.Api_Handler.GelbooruWrapper.Responses;
using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;

namespace Big_Seed_Bot.Commands;

public class ApiCommandModule : BaseCommandModule
{
    [Command("gelbooru")]
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

        string? post = await Wrapper.WrapperInstance.GetRandomPost(search);
        if (post is null)
        {
            await ctx.Channel.SendMessageAsync("error");
            return;
        }

        await ctx.Channel.SendMessageAsync($"||{post} ||");
    }
}
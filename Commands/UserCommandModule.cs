using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;
using DisCatSharp.Entities;

namespace Big_Seed_Bot.Commands;

public class UserCommandModule : BaseCommandModule
{
    [Command("greet")]
    public async Task GreetCommand(CommandContext ctx)
    {
        await ctx.Channel.SendMessageAsync($"Hello, {ctx.User.Username}!"); 
    }

    [Command("picture")]
    [Aliases("pic", "p")]
    public async Task PictureCommand(CommandContext ctx, DiscordMember? member = null)
    {
        member ??= ctx.Member;
        string picture = member.AvatarUrl;
        
        await ctx.Channel.SendMessageAsync(picture);
    }
    
}
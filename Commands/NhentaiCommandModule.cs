using Big_Seed_Bot.Api_Handler.Wrappers.Nhentai;
using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;
using DisCatSharp.Entities;

namespace Big_Seed_Bot.Commands;

public class NhentaiCommandModule : BaseCommandModule
{
    private NhentaiClient _client = new NhentaiClient();
    
    [Command("nhentai")]
    public async Task NhentaiGet(CommandContext ctx)
    {
        string result = await _client.Get();
        await ctx.Channel.SendMessageAsync(result[..100]);
    }
    
    [Command("read")]
    public async Task NhentaiRead(CommandContext ctx)
    {
        byte[] result = await _client.GetImage();
        
        MemoryStream stream = new MemoryStream(result);
        DiscordMessageBuilder builder =
            new DiscordMessageBuilder().WithFile("image.png", stream);
        
        
        await builder.SendAsync(ctx.Channel);
    }
}
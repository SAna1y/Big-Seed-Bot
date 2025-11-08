using Big_Seed_Bot.Api_Handler.Wrappers.Responses;
using DisCatSharp.CommandsNext;
using DisCatSharp.Entities;

namespace Big_Seed_Bot.Commands.CommandUtils;

public class CommandService
{
    private static readonly Dictionary<DiscordMessage, ResponseContext> ActiveReadings = new Dictionary<DiscordMessage, ResponseContext>();
    public void AddService(DiscordMessage message, ResponseContext responseContext)
    {
        if (ActiveReadings.TryAdd(message, responseContext)) return;
        ActiveReadings[message] = responseContext;
    }

    public ResponseContext BuildResponseContext(CommandContext commandContext, IResponse post, int page = 0)
    {
        return new ResponseContext(commandContext, post, page);
    }

    public ResponseContext GetResponseContext(DiscordMessage message)
    {
        return ActiveReadings.GetValueOrDefault(message);
    }

    public struct ResponseContext(CommandContext context, IResponse post, int page)
    {
        public readonly CommandContext Context = context;
        public readonly IResponse Post = post;
        public readonly int Page = page;
    }
    
}
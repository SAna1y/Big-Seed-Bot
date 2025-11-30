using Big_Seed_Bot.Api_Handler.Wrappers.Pokemon;
using Big_Seed_Bot.Api_Handler.Wrappers.Responses;
using Big_Seed_Bot.Api_Handler.Wrappers.Responses.PokemonResponses;
using Big_Seed_Bot.Pokemon_Manager;
using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;

namespace Big_Seed_Bot.Commands;

public class PokemonCommandModule : BaseCommandModule
{
    private PokemonClient _client = new PokemonClient();
    private PokemonManager _manager;

    public PokemonCommandModule()
    {
        _manager = PokemonManager.SetupManager(_client).Result;
    }
    
    [Command("ppoke")]
    public async Task PokemonCommand(CommandContext ctx) 
    {
        await ctx.Channel.SendMessageAsync("First five pokemon names: " + _manager.GetFirstFivePokemonNames());
    }
}
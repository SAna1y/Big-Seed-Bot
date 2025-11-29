using Big_Seed_Bot.Api_Handler.Wrappers.Pokemon;
using Big_Seed_Bot.Api_Handler.Wrappers.Responses;
using Big_Seed_Bot.Api_Handler.Wrappers.Responses.PokemonResponses;
using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;

namespace Big_Seed_Bot.Commands;

public class PokemonCommandModule : BaseCommandModule
{
    private PokemonClient _client = new PokemonClient();
    
    
    [Command("ppoke")]
    public async Task PokemonCommand(CommandContext ctx, string pokemonName)
    {
        Response<PokemonResponse> response = await _client.GetPokemon(pokemonName);
        if (response.ApiResponse is null)
        {
            await ctx.Channel.SendMessageAsync(response.Error == "" ? "no pokemon found" : response.Error);
            return;
        }
        
        await ctx.Channel.SendMessageAsync($"ja létezik {response.ApiResponse.Name}");
    }
}
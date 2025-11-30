using Big_Seed_Bot.Api_Handler.Wrappers.Responses;
using Big_Seed_Bot.Api_Handler.Wrappers.Responses.PokemonResponses;

namespace Big_Seed_Bot.Api_Handler.Wrappers.Pokemon;

public class PokemonClient : Wrapper
{
    public PokemonClient()
    {
        BaseUrl = new Uri("https://pokeapi.co/api/v2");
    }

    public async Task<Response<PokemonResponse>> GetPokemon(string name)
    {
        Response<PokemonResponse> pokemon =
            await Get<PokemonResponse>(Client.GetStringAsync, $"{BaseUrl}/pokemon/{name}");
        return pokemon;
    }
    
}
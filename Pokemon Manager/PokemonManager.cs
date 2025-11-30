using Big_Seed_Bot.Api_Handler.Wrappers.Pokemon;
using Big_Seed_Bot.Api_Handler.Wrappers.Responses;
using Big_Seed_Bot.Api_Handler.Wrappers.Responses.PokemonResponses;

namespace Big_Seed_Bot.Pokemon_Manager;

public class PokemonManager
{
    public List<PokemonResponse> PokemonList { get; set; } = new List<PokemonResponse>();

    private PokemonManager(){}

    public static async Task<PokemonManager> SetupManager(PokemonClient pokemonClient)
    {
        PokemonManager manager = new PokemonManager();
        
        for (int i = 1; i <= 151; i++)
        {
            Response<PokemonResponse> pokemon = await pokemonClient.GetPokemon(i.ToString());
            if (pokemon.ApiResponse is null) continue;
            manager.PokemonList.Add(pokemon.ApiResponse);
        }
        
        return manager;
    }
    
    public string GetFirstFivePokemonNames()
    {
        return PokemonList.Take(5).Aggregate("", (current, pokemon) => current + (pokemon.Name + ", "));
    }
}
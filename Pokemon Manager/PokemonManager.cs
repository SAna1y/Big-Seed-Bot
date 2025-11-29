using Big_Seed_Bot.Api_Handler.Wrappers.Responses.PokemonResponses;

namespace Big_Seed_Bot.Pokemon_Manager;

public class PokemonManager
{
    public List<PokemonResponse> PokemonList { get; set; } = new List<PokemonResponse>();

    public PokemonManager()
    {
        
    }
}
namespace Big_Seed_Bot.Api_Handler.Wrappers.Pokemon;

public class PokemonClient : Wrapper
{
    public PokemonClient()
    {
        BaseUrl = new Uri("https://pokeapi.co/api/v2");
    }
    
    
}
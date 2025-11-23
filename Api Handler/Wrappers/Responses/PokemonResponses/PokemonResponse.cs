using System.Text.Json.Serialization;

namespace Big_Seed_Bot.Api_Handler.Wrappers.Responses.PokemonResponses;

public class PokemonResponse : IResponse
{
	[JsonPropertyName("abilities")]
	public Abilities[] Abilities { get; set; }

	[JsonPropertyName("base_experience")]
	public int BaseExperience { get; set; }

	[JsonPropertyName("cries")]
	public Cries Cries { get; set; }

	[JsonPropertyName("forms")]
	public Forms[] Forms { get; set; }

	[JsonPropertyName("game_indices")]
	public Game_indices[] GameIndices { get; set; }

	[JsonPropertyName("height")]
	public int Height { get; set; }

	[JsonPropertyName("held_items")]
	public object[] HeldItems { get; set; }

	[JsonPropertyName("id")]
	public int Id { get; set; }

	[JsonPropertyName("is_default")]
	public bool IsDefault { get; set; }

	[JsonPropertyName("location_area_encounters")]
	public string LocationAreaEncounters { get; set; }

	[JsonPropertyName("moves")]
	public Moves[] Moves { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("order")]
	public int Order { get; set; }

	[JsonPropertyName("past_abilities")]
	public Past_abilities[] PastAbilities { get; set; }

	[JsonPropertyName("past_types")]
	public object[] PastTypes { get; set; }

	[JsonPropertyName("species")]
	public Species Species { get; set; }

	[JsonPropertyName("sprites")]
	public Sprites Sprites { get; set; }

	[JsonPropertyName("stats")]
	public Stats[] Stats { get; set; }

	[JsonPropertyName("types")]
	public Types[] Types { get; set; }

	[JsonPropertyName("weight")]
	public int Weight { get; set; }
	
	public string GetUrl()
	{
		throw new NotImplementedException();
	}
}

public class Abilities
{
	[JsonPropertyName("ability")]
	public Ability Ability { get; set; }

	[JsonPropertyName("is_hidden")]
	public bool IsHidden { get; set; }

	[JsonPropertyName("slot")]
	public int Slot { get; set; }

}

public class Ability
{
	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("url")]
	public string Url { get; set; }

}

public class Cries
{
	[JsonPropertyName("latest")]
	public string Latest { get; set; }

	[JsonPropertyName("legacy")]
	public string Legacy { get; set; }

}

public class Forms
{
	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("url")]
	public string Url { get; set; }

}

public class Game_indices
{
	[JsonPropertyName("game_index")]
	public int GameIndex { get; set; }

	[JsonPropertyName("version")]
	public Version Version { get; set; }

}

public class Version
{
	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("url")]
	public string Url { get; set; }

}

public class Moves
{
	[JsonPropertyName("move")]
	public Move Move { get; set; }

	[JsonPropertyName("version_group_details")]
	public Version_group_details[] VersionGroupDetails { get; set; }

}

public class Move
{
	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("url")]
	public string Url { get; set; }

}

public class Version_group_details
{
	[JsonPropertyName("level_learned_at")]
	public int LevelLearnedAt { get; set; }

	[JsonPropertyName("move_learn_method")]
	public Move_learn_method MoveLearnMethod { get; set; }

	[JsonPropertyName("order")]
	public int? Order { get; set; }

	[JsonPropertyName("version_group")]
	public Version_group VersionGroup { get; set; }

}

public class Move_learn_method
{
	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("url")]
	public string Url { get; set; }

}

public class Version_group
{
	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("url")]
	public string Url { get; set; }

}

public class Past_abilities
{
	[JsonPropertyName("abilities")]
	public Abilities1[] Abilities { get; set; }

	[JsonPropertyName("generation")]
	public Generation Generation { get; set; }

}

public class Abilities1
{
	[JsonPropertyName("ability")]
	public object Ability { get; set; }

	[JsonPropertyName("is_hidden")]
	public bool IsHidden { get; set; }

	[JsonPropertyName("slot")]
	public int Slot { get; set; }

}

public class Generation
{
	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("url")]
	public string Url { get; set; }

}

public class Species
{
	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("url")]
	public string Url { get; set; }

}

public class Sprites
{
	[JsonPropertyName("back_default")]
	public string BackDefault { get; set; }

	[JsonPropertyName("back_female")]
	public object BackFemale { get; set; }

	[JsonPropertyName("back_shiny")]
	public string BackShiny { get; set; }

	[JsonPropertyName("back_shiny_female")]
	public object BackShinyFemale { get; set; }

	[JsonPropertyName("front_default")]
	public string FrontDefault { get; set; }

	[JsonPropertyName("front_female")]
	public object FrontFemale { get; set; }

	[JsonPropertyName("front_shiny")]
	public string FrontShiny { get; set; }

	[JsonPropertyName("front_shiny_female")]
	public object FrontShinyFemale { get; set; }

	[JsonPropertyName("other")]
	public Other Other { get; set; }

	[JsonPropertyName("versions")]
	public Versions Versions { get; set; }

}

public class Other
{
	[JsonPropertyName("dream_world")]
	public Dream_world DreamWorld { get; set; }

	[JsonPropertyName("home")]
	public Home Home { get; set; }

	[JsonPropertyName("official_artwork")]
	public Official_artwork OfficialArtwork { get; set; }

	[JsonPropertyName("showdown")]
	public Showdown Showdown { get; set; }

}

public class Dream_world
{
	[JsonPropertyName("front_default")]
	public string FrontDefault { get; set; }

	[JsonPropertyName("front_female")]
	public object FrontFemale { get; set; }

}

public class Home
{
	[JsonPropertyName("front_default")]
	public string FrontDefault { get; set; }

	[JsonPropertyName("front_female")]
	public object FrontFemale { get; set; }

	[JsonPropertyName("front_shiny")]
	public string FrontShiny { get; set; }

	[JsonPropertyName("front_shiny_female")]
	public object FrontShinyFemale { get; set; }

}

public class Official_artwork
{
	[JsonPropertyName("front_default")]
	public string FrontDefault { get; set; }

	[JsonPropertyName("front_shiny")]
	public string FrontShiny { get; set; }

}

public class Showdown
{
	[JsonPropertyName("back_default")]
	public string BackDefault { get; set; }

	[JsonPropertyName("back_female")]
	public object BackFemale { get; set; }

	[JsonPropertyName("back_shiny")]
	public string BackShiny { get; set; }

	[JsonPropertyName("back_shiny_female")]
	public object BackShinyFemale { get; set; }

	[JsonPropertyName("front_default")]
	public string FrontDefault { get; set; }

	[JsonPropertyName("front_female")]
	public object FrontFemale { get; set; }

	[JsonPropertyName("front_shiny")]
	public string FrontShiny { get; set; }

	[JsonPropertyName("front_shiny_female")]
	public object FrontShinyFemale { get; set; }

}

public class Versions
{
	[JsonPropertyName("generation_i")]
	public Generation_i GenerationI { get; set; }

	[JsonPropertyName("generation_ii")]
	public Generation_ii GenerationIi { get; set; }

	[JsonPropertyName("generation_iii")]
	public Generation_iii GenerationIii { get; set; }

	[JsonPropertyName("generation_iv")]
	public Generation_iv GenerationIv { get; set; }

	[JsonPropertyName("generation_v")]
	public Generation_v GenerationV { get; set; }

	[JsonPropertyName("generation_vi")]
	public Generation_vi GenerationVi { get; set; }

	[JsonPropertyName("generation_vii")]
	public Generation_vii GenerationVii { get; set; }

	[JsonPropertyName("generation_viii")]
	public Generation_viii GenerationViii { get; set; }

}

public class Generation_i
{
	[JsonPropertyName("red_blue")]
	public Red_blue RedBlue { get; set; }

	[JsonPropertyName("yellow")]
	public Yellow Yellow { get; set; }

}

public class Red_blue
{
	[JsonPropertyName("back_default")]
	public string BackDefault { get; set; }

	[JsonPropertyName("back_gray")]
	public string BackGray { get; set; }

	[JsonPropertyName("back_transparent")]
	public string BackTransparent { get; set; }

	[JsonPropertyName("front_default")]
	public string FrontDefault { get; set; }

	[JsonPropertyName("front_gray")]
	public string FrontGray { get; set; }

	[JsonPropertyName("front_transparent")]
	public string FrontTransparent { get; set; }

}

public class Yellow
{
	[JsonPropertyName("back_default")]
	public string BackDefault { get; set; }

	[JsonPropertyName("back_gray")]
	public string BackGray { get; set; }

	[JsonPropertyName("back_transparent")]
	public string BackTransparent { get; set; }

	[JsonPropertyName("front_default")]
	public string FrontDefault { get; set; }

	[JsonPropertyName("front_gray")]
	public string FrontGray { get; set; }

	[JsonPropertyName("front_transparent")]
	public string FrontTransparent { get; set; }

}

public class Generation_ii
{
	[JsonPropertyName("crystal")]
	public Crystal Crystal { get; set; }

	[JsonPropertyName("gold")]
	public Gold Gold { get; set; }

	[JsonPropertyName("silver")]
	public Silver Silver { get; set; }

}

public class Crystal
{
	[JsonPropertyName("back_default")]
	public string BackDefault { get; set; }

	[JsonPropertyName("back_shiny")]
	public string BackShiny { get; set; }

	[JsonPropertyName("back_shiny_transparent")]
	public string BackShinyTransparent { get; set; }

	[JsonPropertyName("back_transparent")]
	public string BackTransparent { get; set; }

	[JsonPropertyName("front_default")]
	public string FrontDefault { get; set; }

	[JsonPropertyName("front_shiny")]
	public string FrontShiny { get; set; }

	[JsonPropertyName("front_shiny_transparent")]
	public string FrontShinyTransparent { get; set; }

	[JsonPropertyName("front_transparent")]
	public string FrontTransparent { get; set; }

}

public class Gold
{
	[JsonPropertyName("back_default")]
	public string BackDefault { get; set; }

	[JsonPropertyName("back_shiny")]
	public string BackShiny { get; set; }

	[JsonPropertyName("front_default")]
	public string FrontDefault { get; set; }

	[JsonPropertyName("front_shiny")]
	public string FrontShiny { get; set; }

	[JsonPropertyName("front_transparent")]
	public string FrontTransparent { get; set; }

}

public class Silver
{
	[JsonPropertyName("back_default")]
	public string BackDefault { get; set; }

	[JsonPropertyName("back_shiny")]
	public string BackShiny { get; set; }

	[JsonPropertyName("front_default")]
	public string FrontDefault { get; set; }

	[JsonPropertyName("front_shiny")]
	public string FrontShiny { get; set; }

	[JsonPropertyName("front_transparent")]
	public string FrontTransparent { get; set; }

}

public class Generation_iii
{
	[JsonPropertyName("emerald")]
	public Emerald Emerald { get; set; }

	[JsonPropertyName("firered_leafgreen")]
	public Firered_leafgreen FireredLeafgreen { get; set; }

	[JsonPropertyName("ruby_sapphire")]
	public Ruby_sapphire RubySapphire { get; set; }

}

public class Emerald
{
	[JsonPropertyName("front_default")]
	public string FrontDefault { get; set; }

	[JsonPropertyName("front_shiny")]
	public string FrontShiny { get; set; }

}

public class Firered_leafgreen
{
	[JsonPropertyName("back_default")]
	public string BackDefault { get; set; }

	[JsonPropertyName("back_shiny")]
	public string BackShiny { get; set; }

	[JsonPropertyName("front_default")]
	public string FrontDefault { get; set; }

	[JsonPropertyName("front_shiny")]
	public string FrontShiny { get; set; }

}

public class Ruby_sapphire
{
	[JsonPropertyName("back_default")]
	public string BackDefault { get; set; }

	[JsonPropertyName("back_shiny")]
	public string BackShiny { get; set; }

	[JsonPropertyName("front_default")]
	public string FrontDefault { get; set; }

	[JsonPropertyName("front_shiny")]
	public string FrontShiny { get; set; }

}

public class Generation_iv
{
	[JsonPropertyName("diamond_pearl")]
	public Diamond_pearl DiamondPearl { get; set; }

	[JsonPropertyName("heartgold_soulsilver")]
	public Heartgold_soulsilver HeartgoldSoulsilver { get; set; }

	[JsonPropertyName("platinum")]
	public Platinum Platinum { get; set; }

}

public class Diamond_pearl
{
	[JsonPropertyName("back_default")]
	public string BackDefault { get; set; }

	[JsonPropertyName("back_female")]
	public object BackFemale { get; set; }

	[JsonPropertyName("back_shiny")]
	public string BackShiny { get; set; }

	[JsonPropertyName("back_shiny_female")]
	public object BackShinyFemale { get; set; }

	[JsonPropertyName("front_default")]
	public string FrontDefault { get; set; }

	[JsonPropertyName("front_female")]
	public object FrontFemale { get; set; }

	[JsonPropertyName("front_shiny")]
	public string FrontShiny { get; set; }

	[JsonPropertyName("front_shiny_female")]
	public object FrontShinyFemale { get; set; }

}

public class Heartgold_soulsilver
{
	[JsonPropertyName("back_default")]
	public string BackDefault { get; set; }

	[JsonPropertyName("back_female")]
	public object BackFemale { get; set; }

	[JsonPropertyName("back_shiny")]
	public string BackShiny { get; set; }

	[JsonPropertyName("back_shiny_female")]
	public object BackShinyFemale { get; set; }

	[JsonPropertyName("front_default")]
	public string FrontDefault { get; set; }

	[JsonPropertyName("front_female")]
	public object FrontFemale { get; set; }

	[JsonPropertyName("front_shiny")]
	public string FrontShiny { get; set; }

	[JsonPropertyName("front_shiny_female")]
	public object FrontShinyFemale { get; set; }

}

public class Platinum
{
	[JsonPropertyName("back_default")]
	public string BackDefault { get; set; }

	[JsonPropertyName("back_female")]
	public object BackFemale { get; set; }

	[JsonPropertyName("back_shiny")]
	public string BackShiny { get; set; }

	[JsonPropertyName("back_shiny_female")]
	public object BackShinyFemale { get; set; }

	[JsonPropertyName("front_default")]
	public string FrontDefault { get; set; }

	[JsonPropertyName("front_female")]
	public object FrontFemale { get; set; }

	[JsonPropertyName("front_shiny")]
	public string FrontShiny { get; set; }

	[JsonPropertyName("front_shiny_female")]
	public object FrontShinyFemale { get; set; }

}

public class Generation_v
{
	[JsonPropertyName("black_white")]
	public Black_white BlackWhite { get; set; }

}

public class Black_white
{
	[JsonPropertyName("animated")]
	public Animated Animated { get; set; }

	[JsonPropertyName("back_default")]
	public string BackDefault { get; set; }

	[JsonPropertyName("back_female")]
	public object BackFemale { get; set; }

	[JsonPropertyName("back_shiny")]
	public string BackShiny { get; set; }

	[JsonPropertyName("back_shiny_female")]
	public object BackShinyFemale { get; set; }

	[JsonPropertyName("front_default")]
	public string FrontDefault { get; set; }

	[JsonPropertyName("front_female")]
	public object FrontFemale { get; set; }

	[JsonPropertyName("front_shiny")]
	public string FrontShiny { get; set; }

	[JsonPropertyName("front_shiny_female")]
	public object FrontShinyFemale { get; set; }

}

public class Animated
{
	[JsonPropertyName("back_default")]
	public string BackDefault { get; set; }

	[JsonPropertyName("back_female")]
	public object BackFemale { get; set; }

	[JsonPropertyName("back_shiny")]
	public string BackShiny { get; set; }

	[JsonPropertyName("back_shiny_female")]
	public object BackShinyFemale { get; set; }

	[JsonPropertyName("front_default")]
	public string FrontDefault { get; set; }

	[JsonPropertyName("front_female")]
	public object FrontFemale { get; set; }

	[JsonPropertyName("front_shiny")]
	public string FrontShiny { get; set; }

	[JsonPropertyName("front_shiny_female")]
	public object FrontShinyFemale { get; set; }

}

public class Generation_vi
{
	[JsonPropertyName("omegaruby_alphasapphire")]
	public Omegaruby_alphasapphire OmegarubyAlphasapphire { get; set; }

	[JsonPropertyName("x_y")]
	public X_y XY { get; set; }

}

public class Omegaruby_alphasapphire
{
	[JsonPropertyName("front_default")]
	public string FrontDefault { get; set; }

	[JsonPropertyName("front_female")]
	public object FrontFemale { get; set; }

	[JsonPropertyName("front_shiny")]
	public string FrontShiny { get; set; }

	[JsonPropertyName("front_shiny_female")]
	public object FrontShinyFemale { get; set; }

}

public class X_y
{
	[JsonPropertyName("front_default")]
	public string FrontDefault { get; set; }

	[JsonPropertyName("front_female")]
	public object FrontFemale { get; set; }

	[JsonPropertyName("front_shiny")]
	public string FrontShiny { get; set; }

	[JsonPropertyName("front_shiny_female")]
	public object FrontShinyFemale { get; set; }

}

public class Generation_vii
{
	[JsonPropertyName("icons")]
	public Icons Icons { get; set; }

	[JsonPropertyName("ultra_sun_ultra_moon")]
	public Ultra_sun_ultra_moon UltraSunUltraMoon { get; set; }

}

public class Icons
{
	[JsonPropertyName("front_default")]
	public string FrontDefault { get; set; }

	[JsonPropertyName("front_female")]
	public object FrontFemale { get; set; }

}

public class Ultra_sun_ultra_moon
{
	[JsonPropertyName("front_default")]
	public string FrontDefault { get; set; }

	[JsonPropertyName("front_female")]
	public object FrontFemale { get; set; }

	[JsonPropertyName("front_shiny")]
	public string FrontShiny { get; set; }

	[JsonPropertyName("front_shiny_female")]
	public object FrontShinyFemale { get; set; }

}

public class Generation_viii
{
	[JsonPropertyName("icons")]
	public Icons1 Icons { get; set; }

}

public class Icons1
{
	[JsonPropertyName("front_default")]
	public string FrontDefault { get; set; }

	[JsonPropertyName("front_female")]
	public object FrontFemale { get; set; }

}

public class Stats
{
	[JsonPropertyName("base_stat")]
	public int BaseStat { get; set; }

	[JsonPropertyName("effort")]
	public int Effort { get; set; }

	[JsonPropertyName("stat")]
	public Stat Stat { get; set; }

}

public class Stat
{
	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("url")]
	public string Url { get; set; }

}

public class Types
{
	[JsonPropertyName("slot")]
	public int Slot { get; set; }

	[JsonPropertyName("type")]
	public Type Type { get; set; }

}

public class Type
{
	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("url")]
	public string Url { get; set; }

}
using Big_Seed_Bot.utils;

namespace Big_Seed_Bot;

internal class Program
{
    private static readonly string? Token = DotEnvReader.Read( Path.GetFullPath(".env"))?["TOKEN"];
    private static void Main(string[] args)
    {
        if (Token is null)
        {
            Console.WriteLine("No token was provided. Set an Environment Variable in your .env file.");
        }
        
    }
}
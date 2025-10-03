namespace Big_Seed_Bot.utils;

public static class DotEnvReader
{
    public static Dictionary<string, string>? Read(string envFile)
    {
        if (!File.Exists(envFile))
        {
            Console.Error.WriteLine($"File {envFile} does not exist");
            return null;
        }

        Dictionary<string, string> env = File.ReadAllLines(envFile)
            .Select(line => line.Split('=', StringSplitOptions.RemoveEmptyEntries))
            .Where(split => split.Length == 2)
            .ToDictionary(split => split[0], split => split[1]);
        
        return env;
    }
}
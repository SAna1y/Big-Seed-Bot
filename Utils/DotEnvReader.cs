namespace Big_Seed_Bot.Utils;

public static class DotEnvReader
{
    public static readonly Dictionary<string, string> Env = Read(Path.GetFullPath(".env"));

    public static Dictionary<string, string> Read(string envFile)
    {
        if (!File.Exists(envFile))
            return [];
        
        Dictionary<string, string> env = new Dictionary<string, string>();
        string[] lines = File.ReadAllLines(envFile);
        
        foreach (string line in lines)
        {
            if (line.StartsWith('#') || string.IsNullOrWhiteSpace(line)) continue;
            
            string[] split = line.Split('=', StringSplitOptions.RemoveEmptyEntries);
            if (split.Length != 2) continue;
            env.Add(split[0], split[1]);
        }
        
        return env;
    }
    public static string GetEnvironmentVariable(this Dictionary<string, string> env, string key) => env.GetValueOrDefault(key, "");
}
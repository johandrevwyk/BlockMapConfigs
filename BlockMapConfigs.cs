using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Newtonsoft.Json;

namespace BlockMapConfigs;
public class BlockMapConfigs : BasePlugin
{
    public override string ModuleName => "BlockMapConfigs";

    public override string ModuleVersion => "1.0.0";

    public const string ConfigFileName = "config.json";
    public Config? Configuration;
    public string GameDir = string.Empty;

    public class Config
    {
        public string[] Maps { get; set; }
    }

    public override void Load(bool hotReload)
    {
        LoadConfig();

        RegisterListener<Listeners.OnMapStart>(OnMapStart);
    }

    private void OnMapStart(string mapName)
    {
        if (Configuration != null && Configuration.Maps != null)
        {
            foreach (var map in Configuration.Maps)
            {
                if (Server.MapName == map)
                {
                    Console.WriteLine($"Map name = {map}, killing map cvars");
                    KillServerCommandEnts();
                    return;
                }
            }
        }
    }

    private static void KillServerCommandEnts()
    {
        var pointServerCommands = Utilities.FindAllEntitiesByDesignerName<CPointServerCommand>("point_servercommand");

        foreach (var servercmd in pointServerCommands)
        {
            if (servercmd == null) continue;
            servercmd.Remove();
        }
    }

    private void LoadConfig()
    {
        GameDir = Server.GameDirectory;
        var jsonPath = Path.Join(GameDir + "/csgo/addons/counterstrikesharp/plugins/BlockMapConfigs", "config.json");

        Configuration = JsonConvert.DeserializeObject<Config>(File.ReadAllText(jsonPath));

        Console.WriteLine("Loaded config file successfully");

        if (Configuration == null)
            throw new JsonException("Configuration could not be loaded");
    }
}
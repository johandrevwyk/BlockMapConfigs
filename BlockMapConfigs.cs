using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace BlockMapConfigs;
public class BlockMapConfigs : BasePlugin
{
    public override string ModuleName => "BlockMapConfigs";

    public override string ModuleVersion => "1.0.0";


    public override void Load(bool hotReload)
    {
        Console.WriteLine("Blocking aim_ map configs");

        RegisterListener<Listeners.OnMapStart>(OnMapStart);
    }

    private void OnMapStart(string mapName)
    {
        if (Server.MapName.Contains("aim_"))
        {
            Console.WriteLine("Map name contains aim_ , killing map cvars");
            KillServerCommandEnts();
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
}
global using BepInEx;
global using HarmonyLib;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using System.Reflection;

namespace PlayCount;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    internal static new ManualLogSource Log;

    public override void Load()
    {
        // Plugin startup logic
        Log = base.Log;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        Log.LogInfo($"Loaded {MyPluginInfo.PLUGIN_NAME}!");
    }
}

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

    // For Banner detection
    public static bool isActive = false;

    // Mode Switch
    public static string ShowScoreKey = "Show Score?";
    public static BepInEx.Configuration.ConfigEntry<bool> ShowScore;

    public override void Load()
    {
        Log = base.Log;

        ShowScore = Config.Bind("General",
            ShowScoreKey,
            true,
            new BepInEx.Configuration.ConfigDescription("Toggle between displaying the current score and the total playcount."));

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        Log.LogInfo($"Loaded {MyPluginInfo.PLUGIN_NAME}!");
    }
}

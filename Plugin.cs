global using BepInEx;
global using HarmonyLib;
global using UnityEngine;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using System.IO;
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

    // Json that stores all data
    public static readonly string JsonDataFolderPath = Paths.PluginPath + "\\" + MyPluginInfo.PLUGIN_GUID;
    public static readonly string JsonDataFilePath = JsonDataFolderPath + "\\Stats.json";

    public override void Load()
    {
        Log = base.Log;

        ShowScore = Config.Bind("General",
            ShowScoreKey,
            false,
            new BepInEx.Configuration.ConfigDescription("Toggle between displaying the current score and the total playcount."));

        // Creates the required Json if it does not exist
        if (!File.Exists(JsonDataFilePath)) { Directory.CreateDirectory(JsonDataFolderPath); File.Create(JsonDataFilePath); };

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        Log.LogInfo($"Loaded {MyPluginInfo.PLUGIN_NAME}!");
    }
}

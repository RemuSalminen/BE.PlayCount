global using MelonLoader;
global using HarmonyLib;
global using UnityEngine;
//using BepInEx.Unity.IL2CPP;
using System.IO;
using System.Reflection;

namespace PlayCount;

public class Plugin : MelonMod
{
    internal static MelonLogger.Instance Log = Melon<Plugin>.Logger;
    
    // For Banner detection
    public static bool isActive = false;

    // MelonLoader Configs
    private MelonPreferences_Category _Category;

    // Mode Switch
    public static string ShowScoreKey = "Show Score?";
    public static MelonPreferences_Entry<bool> ShowScore;

    // Json that stores all data
    public static readonly string JsonDataFolderPath = MelonLoader.Utils.MelonEnvironment.UserDataDirectory + "\\" + System.Reflection.Assembly.GetExecutingAssembly().FullName;
    public static readonly string JsonDataFilePath = JsonDataFolderPath + "\\Stats.json";

    // Create Configs
    public override void OnEarlyInitializeMelon()
    {
        _Category = MelonPreferences.CreateCategory("PlayCount");
        ShowScore = _Category.CreateEntry(ShowScoreKey, false, ShowScoreKey, "Toggle between displaying the current score and the total playcount.");

        MelonPreferences.Save();
        base.OnEarlyInitializeMelon();
    }

    public override void OnInitializeMelon()
    {
        // Creates the required Json if it does not exist
        if (!File.Exists(JsonDataFilePath)) {
            Directory.CreateDirectory(JsonDataFolderPath);
            using (StreamWriter sw = File.CreateText(JsonDataFilePath))
            {
                sw.WriteLine("{\n\n}");
            }
        };

        Log.Msg($"Loaded {MelonAssembly.Assembly.FullName}!");

        base.OnInitializeMelon();
    }
}

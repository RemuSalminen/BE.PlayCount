using Menu.MenuSteps.MatchMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace PlayCount.Patches;

[HarmonyPatch]
public class MultiSteps
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MultiStartGameMenuStep), nameof(MultiStartGameMenuStep.Open))]
    public static void BannerOpenFix()
    {
        Plugin.Log.LogInfo("Opened Multiplayer UI!");
        Plugin.isActive = true;

        Plugin.Log.LogInfo("Fetching Config...");
        int CandleAmount = Plugin.ShowScore.Value ? 1 : 2;
        Plugin.Log.LogInfo("Fetch Complete!");

        Plugin.Log.LogInfo("Placing Candles!");
        Internal.CandleSystem.Place(CandleAmount);
        Plugin.Log.LogInfo("Candles Placed!");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MultiStartGameMenuStep), nameof(MultiStartGameMenuStep.Close))]
    public static void BannerCloseFix()
    {
        if (!Plugin.isActive) return;
        Plugin.isActive = false;
        Plugin.Log.LogInfo("Closing Multiplayer UI Soon!");

        Plugin.Log.LogInfo("Clearing Candles!");
        Internal.CandleSystem.Clear();
        Plugin.Log.LogInfo("Cleared Candles!");
    }
}
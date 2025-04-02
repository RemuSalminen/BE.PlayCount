using Menu.MenuSteps.MatchMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace PlayCount.Patches
{
    [HarmonyPatch]
    public class MultiSteps
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MultiStartGameMenuStep), nameof(MultiStartGameMenuStep.Open))]
        public static void BannerOpenFix()
        {
            Plugin.isActive = true;
            Plugin.Log.LogInfo("Opening Multiplayer UI soon!");
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MultiStartGameMenuStep), nameof(MultiStartGameMenuStep.Close))]
        public static void BannerCloseFix()
        {
            if (!Plugin.isActive) return;
            Plugin.Log.LogInfo("Closed Multiplayer UI!");
        }
    }
}

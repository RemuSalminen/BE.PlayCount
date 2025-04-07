using Menu.MenuSteps.MatchMenu;

namespace PlayCount.Patches;

[HarmonyPatch]
public class MultiSteps
{
    // Game start logic
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MultiStartGameMenuStep), nameof(MultiStartGameMenuStep.Open))]
    public static void BannerOpenFix(MultiStartGameMenuStep __instance)
    {
        Plugin.Log.LogDebug("Multi: Opened Multiplayer UI!");
        Plugin.isActive = true;

        Plugin.Log.LogInfo("Multi: Fetching Stats...");
        string CurOpponentID = __instance._opponentBanner.Data.PlayFabID;
        (int CandleAmount, int Wins, int Losses, bool WonPrevious, bool New) = Internal.Stats.GetStats(CurOpponentID);
        Plugin.Log.LogInfo($"Multi: Fetch Complete!\n ShowScore: {Plugin.ShowScore.Value}\nNew: {New}\n W: {Wins}\n L: {Losses}\n Prev: {WonPrevious}");

        Plugin.Log.LogInfo($"Multi: Placing {CandleAmount} Candle(s)!");
        Internal.CandleSystem.Place(CandleAmount, Wins, Losses);
        Plugin.Log.LogInfo("Multi: Candles Placed!");

        Plugin.Log.LogInfo("Multi: Lighting Candles!");
        Internal.CandleSystem.DecorateCandles(WonPrevious, New);
        Plugin.Log.LogInfo("Multi: Candles Lit!");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MultiStartGameMenuStep), nameof(MultiStartGameMenuStep.Close))]
    public static void BannerCloseFix()
    {
        if (!Plugin.isActive) return;
        Plugin.isActive = false;
        Plugin.Log.LogDebug("Multi: Closing Multiplayer UI Soon!");

        Plugin.Log.LogInfo("Multi: Clearing Candles!");
        Internal.CandleSystem.Clear();
        Plugin.Log.LogInfo("Multi: Cleared Candles!");
    }


    // Game end & data update logic
    [HarmonyPostfix]
    [HarmonyPatch(typeof(RankedEndGameMenuStep), nameof(RankedEndGameMenuStep.Open))]
    public static void EndGameFix(RankedEndGameMenuStep __instance)
    {
        Plugin.Log.LogDebug("Multi: Ranked Match Over!");
        UI.EndGameScore.PlayerData Opponent = __instance._endGameScoreUI.RightPlayer;
        Internal.Stats.UpdateStats(Opponent);
    }
}
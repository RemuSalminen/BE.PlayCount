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
        Plugin.Log.Msg("DEBUG: Multi: Opened Multiplayer UI!");
        Plugin.isActive = true;

        Plugin.Log.Msg("Multi: Fetching Stats...");
        if (__instance == null) { Plugin.Log.BigError("Opponent Does Not Exist!"); return; }
        string CurOpponentID = __instance._opponentBanner.Data.PlayFabID;
        (int CandleAmount, int Wins, int Losses, bool WonPrevious, bool New) = Internal.Stats.GetStats(CurOpponentID);
        Plugin.Log.Msg($"Multi: Fetch Complete!");
        Plugin.Log.Msg($"DEBUG: Data:\n ShowScore: {Plugin.ShowScore.Value}\nNew: {New}\n W: {Wins}\n L: {Losses}\n Prev: {WonPrevious}");

        Plugin.Log.Msg($"Multi: Placing {CandleAmount} Candle(s)!");
        Internal.CandleSystem.Place(CandleAmount, Wins, Losses);
        Plugin.Log.Msg("Multi: Candles Placed!");

        Plugin.Log.Msg("Multi: Lighting Candles!");
        Internal.CandleSystem.DecorateCandles(WonPrevious, New);
        Plugin.Log.Msg("Multi: Candles Lit!");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MultiStartGameMenuStep), nameof(MultiStartGameMenuStep.Close))]
    public static void BannerCloseFix()
    {
        if (!Plugin.isActive) return;
        Plugin.isActive = false;
        Plugin.Log.Msg("DEBUG: Multi: Closing Multiplayer UI Soon!");

        Plugin.Log.Msg("Multi: Clearing Candles!");
        Internal.CandleSystem.Clear();
        Plugin.Log.Msg("Multi: Cleared Candles!");
    }


    // Game end & data update logic
    [HarmonyPostfix]
    [HarmonyPatch(typeof(RankedEndGameMenuStep), nameof(RankedEndGameMenuStep.Open))]
    public static void EndGameFix(RankedEndGameMenuStep __instance)
    {
        Plugin.Log.Msg("DEBUG: Multi: Ranked Match Over!");
        UI.EndGameScore.PlayerData Opponent = __instance._endGameScoreUI.RightPlayer;
        Internal.Stats.UpdateStats(Opponent);
    }
}
using UI.EndGameScore;
using PlayCount.Data;

namespace PlayCount.Internal;

public static class Stats
{
    static readonly int CandleAmount = Plugin.ShowScore.Value ? 2 : 1;
    static int Wins;
    static int Losses;
    /// <summary>
    /// Did the Player win their previous match
    /// </summary>
    static bool WonPrevious;
    static bool New;
    static string LastOpponentID;
    static string CurOpponentID;
    public static (int CandleAmount, int Wins, int Losses, bool WonPrevious, bool New) GetStats(string curOpponentID)
    {
        CurOpponentID = curOpponentID;
        Plugin.Log.Msg($"DEBUG: CurOp: {CurOpponentID}\nLastOp: {LastOpponentID}");
        if (CurOpponentID == LastOpponentID) return (CandleAmount, Wins, Losses, WonPrevious, New);

        Data.Data curOpponentData = Json.Get(Plugin.JsonDataFilePath, curOpponentID);
        Wins = curOpponentData.Wins;
        Losses = curOpponentData.Losses;
        WonPrevious = curOpponentData.WonPrevious;
        New = (Wins + Losses) == 0;

        return (CandleAmount, Wins, Losses, WonPrevious, New);
    }

    public static void UpdateStats(PlayerData lastOpponent)
    {
        Plugin.Log.Msg($"Updating Stats for {lastOpponent.Name} ({lastOpponent.PlayFabID})!");
        if (lastOpponent.PlayFabID != CurOpponentID) {
            Plugin.Log.Error("lastOpponent != CurOpponent\nAttempting to fetch correct stats!");
            GetStats(lastOpponent.PlayFabID);
        };
        LastOpponentID = CurOpponentID;
        CurOpponentID = null;
        WonPrevious = !lastOpponent.IsWinner;
        if (WonPrevious) Wins += 1; else Losses += 1;

        Data.Data data = new()
        {
            Wins = Wins,
            Losses = Losses,
            WonPrevious = WonPrevious
        };

        if ((Wins + Losses) == 1) Json.Add(Plugin.JsonDataFilePath, LastOpponentID, data); 
            else Json.Edit(Plugin.JsonDataFilePath, LastOpponentID, data);

        Plugin.Log.Msg($"Stats Updated!");
        return;
    }
}

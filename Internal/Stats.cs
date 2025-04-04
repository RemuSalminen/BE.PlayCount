using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PlayCount.Internal;

public class Stats
{
    static readonly int CandleAmount = Plugin.ShowScore.Value ? 2 : 1;
    static int Wins = 0;
    static int Losses = 0;
    /// <summary>
    /// Did the Player win their previous match
    /// </summary>
    static bool WonPrevious = false;
    static bool New = true;
    static string LastOpponentID = null;
    static string CurOpponentID = null;
    public static (int CandleAmount, int Wins, int Losses, bool WonPrevious, bool New) GetStats(string curOpponentID)
    {
        CurOpponentID = curOpponentID;
        if (CurOpponentID == LastOpponentID) return (CandleAmount, Wins, Losses, WonPrevious, New);

        Wins = 2;
        Losses = 3;
        WonPrevious = false;
        New = (Wins + Losses) == 0;
        return (CandleAmount, Wins, Losses, WonPrevious, New);
    }
}

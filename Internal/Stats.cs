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
    static int Wins;
    static int Losses;
    static bool WonPrevious;
    static bool New;
    public static (int CandleAmount, int Wins, int Losses, bool WonPrevious, bool New) GetStats()
    {
        Wins = 2;
        Losses = 3;
        WonPrevious = false;
        New = (Wins + Losses) == 0;
        return (CandleAmount, Wins, Losses, WonPrevious, New);
    }
}

using Menu.Candle;
using Menu;
using System.Collections.Generic;
using VR;

namespace PlayCount.Internal;

internal class CandleSystem
{
    private static GameObject CandleParent = null;

    public static List<MenuCandle> CandleList = new List<MenuCandle>();

    private static void Init()
    {
        CandleParent = new GameObject("PlayCount_CandleParent");
        // + Left, + Forward, + Up // Don't actually even seem to do anything
        CandleParent.transform.position = new Vector3(0f, -5f, 2f);
    }

    private static float CandleLeft = -24.4f;
    public static void Place(int CandleAmount, int Win = 0, int Loss = 0)
    {
        if (CandleParent == null)
        {
            Init();
        }

        List<CandleConfig> CConfigs;
        if (Plugin.ShowScore.Value)
        {
            CConfigs = new List<CandleConfig>(2)
            {
                new CandleConfig
                {
                    CandleType = CandleType.Basic,
                    CutAction = null,
                    LocalizationKey = $"{Win}"
                },
                new CandleConfig
                {
                    CandleType = CandleType.Basic,
                    CutAction = null,
                    LocalizationKey = $"{Loss}"
                }
            };
        } else
        {
            CConfigs = new List<CandleConfig>(1)
            {
                new CandleConfig
                {
                    CandleType = CandleType.Basic,
                    CutAction = null,
                    LocalizationKey = $"{Win+Loss}"
                }
            };
        }

        for (int i = 0; i < CandleAmount; i++)
        {
            Plugin.Log.LogInfo($"Placing {i + 1} Candle");
            MenuCandle Candle = MenuBase.BaseInstance.MenuCandleBuilder.CreateCandle(CandleParent, CConfigs[i]);
            MenuBase.BaseInstance.MenuCandleBuilder.PlaceCandle(Candle, Plugin.ShowScore.Value ? (CandleLeft - i*2*CandleLeft) : 0f );

            Candle.MoveToward._xrRigHead = XrRigManager.InstanceReference.HeadController;
            Candle.MoveToward.IsLookedAt = true;
            // Stops the Candles from Moving when looked at
            Candle.MoveToward.MoveTowardDistance = 0f;
            Candle.InitialAnimationDone = true;

            Plugin.Log.LogDebug($"{Candle.gameObject.transform.position}");
            CandleList.Add(Candle);
            Plugin.Log.LogInfo($"Placed {i + 1} Candle");
        }
    }

    /// <summary>
    /// WIP: Lights and cuts the candles
    /// </summary>
    /// <param name="WonPrevious">Did the Player win the previous match against this player</param>
    /// <param name="New">Has the player never played against this player</param>
    public static void DecorateCandles(bool WonPrevious = false, bool New = false)
    {
        for (int i = 0; i<CandleList.Count; i++)
        {
            MenuCandle Candle = CandleList[i];
            Candle.StopBurning(true);
            if (Plugin.ShowScore.Value && !New)
            {
                // WIP: Light the Previous match's Winner's Candle
                if (/*WonPrevious &&*/ i == 0) { Candle.StartBurning(); }
                else if (!WonPrevious && i == 1) { Candle.StartBurning(); Candle.StopBurning(true); }
                // Cut the Loser's Candle
                else { Candle.IsCut = true; Candle.Cut(new Vector3(5f, 3f, 5f)); }
            }
            else { Candle.IsBurning = true; Candle.StartBurning(); }
        }
    }

    /// <summary>
    ///  Destroys any possibly existing candles and clears the CandleList
    /// </summary>
    public static void Clear()
    {
        foreach (MenuCandle Candle in CandleList)
        {
            if (Candle != null)
            {
                Object.Destroy(Candle.gameObject);
            }
        }
        CandleList.Clear();
    }
}

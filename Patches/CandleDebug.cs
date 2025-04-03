using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Menu.Candle;

namespace PlayCount.Patches;

#if DEBUG
[HarmonyPatch]
public class CandleDebug
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(CandleActivation), nameof(CandleActivation.ActivateCandle))]
    public static void CActivate()
    {
        Plugin.Log.LogDebug("Activated Candle!");
    }
    [HarmonyPostfix]
    [HarmonyPatch(typeof(CandleActivation), nameof(CandleActivation.SetActiveCandle))]
    public static void CActive()
    {
        Plugin.Log.LogDebug("Set Active Candle!");
    }


    // Called when candles have spawned
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MenuCandle), nameof(MenuCandle.Start))]
    public static void CStart()
    {
        Plugin.Log.LogDebug("Started MenuCandle!");
    }
    // Called when candle has been cut
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MenuCandle), nameof(MenuCandle.Cut))]
    public static void CCut()
    {
        Plugin.Log.LogDebug("Cut MenuCandle!");
    }

    // 
    [HarmonyPostfix]
    [HarmonyPatch(typeof(CandleBase), nameof(CandleBase.PlayCandleCut))]
    public static void PCCut()
    {
        Plugin.Log.LogDebug("Played MenuCandle Cut!");
    }
    // Continuosly called when the candle is lit
    [HarmonyPostfix]
    [HarmonyPatch(typeof(CandleBase), nameof(CandleBase.StartBurning))]
    public static void CSBurning()
    {
        Plugin.Log.LogDebug("MenuCandle Started Burning!");
        throw new Exception("Dirty exception");
    }
    // 
    [HarmonyPostfix]
    [HarmonyPatch(typeof(CandleBase), nameof(CandleBase.OnTriggerEnter))]
    public static void CTriggerEnter()
    {
        Plugin.Log.LogDebug("Entered MenuCandle Trigger!");
    }
}
#endif
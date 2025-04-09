using System;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace JumpstartScreenFix
{
    [HarmonyPatch]
    [BepInPlugin(GUID, "Jumpstart Screen Fix", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        const string GUID = "bobot.jumpstartscreenfix";
        
        static Harmony _harmony;
        
        void Start()
        {
            _harmony = new Harmony(GUID);
            _harmony.PatchAll();

        }
        
        [HarmonyPatch(typeof(Nailgun), nameof(Nailgun.Start))]
        [HarmonyPostfix]
        static void NailgunStart(Nailgun __instance)
        {
            if (__instance is { variation: 2 })
            {
                var colourBlindGet1 = __instance.zapMeter?.transform.Find("Background")?.gameObject.AddComponent<ColorBlindGet>();
                var colourBlindGet2 = __instance.distanceMeter.handleRect?.gameObject.AddComponent<ColorBlindGet>();
                var colourBlindGet3 = __instance.distanceMeter.fillRect?.gameObject.AddComponent<ColourBlindGetWithHslStuff>();
                
                var colourBlindGet4 = __instance.distanceMeter.transform.Find("Handle (3)")?.gameObject.AddComponent<ColourBlindGetWithHslStuff>();
                var colourBlindGet5 = __instance.distanceMeter.transform.Find("Handle (2)")?.gameObject.AddComponent<ColourBlindGetWithHslStuff>();

                
                colourBlindGet1.variationColor = true;
                colourBlindGet1.variationNumber = 2;
                
                colourBlindGet2.variationColor = true;
                colourBlindGet2.variationNumber = 2;
                
                colourBlindGet3.variationNumber = 2;
                colourBlindGet3.lMult = 0.5f;
                
                colourBlindGet4.variationNumber = 2;
                colourBlindGet4.lMult = 0.4f;
                
                colourBlindGet5.variationNumber = 2;
                colourBlindGet5.lMult = 0.4f;
                colourBlindGet5.sMult = 0.5f;
            }
        }
        
        [HarmonyPatch(typeof(Nailgun), nameof(Nailgun.UpdateZapHud))]
        [HarmonyPostfix]
        static void NailgunUpdateZapHud(Nailgun __instance)
        {
            
            
            
            if (!__instance.currentZapper)
                return;
            
            if (__instance.currentZapper.distance > __instance.currentZapper.maxDistance || __instance.currentZapper.raycastBlocked)
            {
                __instance.warningX.enabled = true;
                __instance.warningX.color = ((__instance.currentZapper.breakTimer % 0.1f > 0.05f) ? ColorBlindSettings.Instance.variationColors[2] : Color.white);
                __instance.distanceMeter.value = 1f;
                __instance.statusText.text = (__instance.currentZapper.raycastBlocked ? "BLOCKED" : (__instance.altVersion ? "TOO FAR" : "OUT OF RANGE"));
                __instance.statusText.color = MonoSingleton<ColorBlindSettings>.Instance.variationColors[2];
                return;
            }
            __instance.statusText.color = Color.Lerp(ColorBlindSettings.Instance.variationColors[2], Color.white, (__instance.currentZapper.maxDistance - __instance.currentZapper.distance) / __instance.currentZapper.maxDistance);
        }
    }
}

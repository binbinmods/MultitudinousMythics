using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using static Obeliskial_Essentials.CardDescriptionNew;
using System;
using static MultitudinousMythics.CustomFunctions;
using System.Text.RegularExpressions;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;
using static MultitudinousMythics.Plugin;
// using static MatchManager;

// Make sure your namespace is the same everywhere
namespace MultitudinousMythics
{

    [HarmonyPatch] //DO NOT REMOVE/CHANGE

    public class MultitudinousMythicsPatches
    {
        // [HarmonyReversePatch]
        // [HarmonyPatch(typeof(MatchManager), "CreateNPC")]
        // public static void CreateNPCReversePatch(NPCData _npcData,
        //     string effectTarget = "",
        //     int _position = -1,
        //     bool generateFromReload = false,
        //     string internalId = "",
        //     CardData _cardActive = null) =>
        //     //This is intentionally a stub
        //     throw new NotImplementedException("Reverse patch has not been executed.");

        // [HarmonyPrefix]
        // [HarmonyPatch(typeof(Character), nameof(Character.HealReceivedFinal))]

        // public static bool HealReceivedFinalPrefix(Character __instance, ref int __result, int heal, bool isIndirect = false, CardData cardAux = null)
        // {
        //     if (__instance.IsHero || (PreventNPCHealing.Value && !__instance.IsHero))
        //     {
        //         __result = 0;
        //         return false;
        //     }
        //     return true;
        // }
        // [HarmonyPrefix]
        // [HarmonyPatch(typeof(PerkTree), "Show")]
        // public static void ShowPrefix(ref PerkTree __instance, ref int ___totalAvailablePoints)
        // {


        //     if (PerksPerLevel.Value > 1)
        //     {
        //         ___totalAvailablePoints = PerksPerLevel.Value * PlayerManager.Instance.GetHighestCharacterRank();
        //     }
        //     if (TotalPerks.Value != -1)
        //     {
        //         ___totalAvailablePoints = TotalPerks.Value;
        //     }

        // }


    }
}

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

        readonly static List<string> BossCombatNodes = ["sen_33", "aqua_35", "velka_32", "faen_38", "ulmin_56", "sahti_62", "voidlow_25", "voidhigh_13"];
        public static bool IsBossCombat = false;
        public static bool IsEndOfActBossCombatReward()
        {
            return RewardsManager.Instance != null && BossCombatNodes.Contains(AtOManager.Instance.currentMapNode);
        }

        public static bool IsBossCombatReward()
        {
            CombatData currentCombatData = Traverse.Create(AtOManager.Instance).Field("currentCombatData").GetValue<CombatData>();
            if (currentCombatData == null)
            {
                LogError("IsBossCombatReward: currentCombatData is null, cannot determine if it is a boss combat.");
                return false;
            }
            foreach (NPCData npc in currentCombatData.NPCList)
            {
                if (npc.IsBoss)
                {
                    IsBossCombat = true;
                }
            }
            return RewardsManager.Instance != null && IsBossCombat;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(RewardsManager), "SetRewards")]
        public static void SetRewardsPrefix(ref int ___numCardsReward)
        {
            if (IsEndOfActBossCombatReward() && EnableIncreasedRewards.Value)
                ___numCardsReward = 4;

            if (IsBossCombatReward() && EnableIncreasedRewardsForAllBosses.Value)
                ___numCardsReward = 4;
        }


        [HarmonyPostfix]
        [HarmonyPatch(typeof(Functions), nameof(Functions.GetCardByRarity))]
        public static void GetCardByRarityPostfix(ref string __result, int rarity, CardData _cardData, bool isChallenge = false)
        {

            if (!IsEndOfActBossCombatReward())
            {
                return;
            }
            LogDebug($"GetCardByRarityPostfix {rarity} {__result}");
            string seed = AtOManager.Instance?.currentMapNode ?? "" + AtOManager.Instance.GetGameId() + __result;
            UnityEngine.Random.InitState(seed.GetDeterministicHashCode());
            bool upgradeToMythic = UnityEngine.Random.Range(0, 100) < ChanceAtMythic.Value;
            if (!upgradeToMythic)
            {
                return;
            }


            seed += "1";
            // string cardName = GetRandomCardByCardRarity(seed);
            // __result = cardName;

        }

        public static string GetRandomCardByCardRarity(Enums.CardRarity cardRarity, Hero hero = null)
        {
            // LogDebug($"GetRandomMythic {seed}");
            // UnityEngine.Random.InitState(seed.GetDeterministicHashCode());
            Hero heroToCheck = hero ?? MatchManager.Instance?.GetHeroHeroActive();
            if (heroToCheck == null)
            {
                LogError("GetRandomCardByCardRarity: Hero is null, cannot determine card class.");
                return "";
            }
            Enums.CardClass result1 = Enums.CardClass.None;
            Enum.TryParse<Enums.CardClass>(Enum.GetName(typeof(Enums.HeroClass), (object)heroToCheck.HeroData.HeroClass), out result1);
            Enums.CardClass result2 = Enums.CardClass.None;
            Enum.TryParse<Enums.CardClass>(Enum.GetName(typeof(Enums.HeroClass), (object)heroToCheck.HeroData.HeroSubClass.HeroClassSecondary), out result2);
            // // int length = this.numCardsReward;
            // // if (this.numCardsReward == 3 && result2 != Enums.CardClass.None)
            // //     length = 4;
            // string[] arr = new string[length];

            List<string> stringList1 = Globals.Instance.CardListNotUpgradedByClass[result1];
            List<string> stringList2 = result2 == Enums.CardClass.None ? new List<string>() : Globals.Instance.CardListNotUpgradedByClass[result2];
            bool flag3 = false;
            CardData _cardData = null;
            while (!flag3)
            {
                bool flag2 = false;
                _cardData = Globals.Instance.GetCardData(result2 == Enums.CardClass.None ? stringList1[UnityEngine.Random.Range(0, stringList1.Count)] : stringList2[UnityEngine.Random.Range(0, stringList2.Count)], false);
                if (!flag2)
                {
                    if (_cardData.CardRarity == cardRarity)
                        flag3 = true;
                }
            }
            return _cardData?.Id.ToLower() ?? "";
        }


    }
}

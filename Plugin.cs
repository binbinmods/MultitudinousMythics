// These are your imports, mostly you'll be needing these 5 for every plugin. Some will need more.

using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using static Obeliskial_Essentials.Essentials;
using static Obeliskial_Essentials.CardDescriptionNew;
using System;
using System.Runtime.CompilerServices;
using BepInEx.Bootstrap;
using System.Collections.Generic;


// The Plugin csharp file is used to 


// Make sure all your files have the same namespace and this namespace matches the RootNamespace in the .csproj file
namespace MultitudinousMythics
{
    // These are used to create the actual plugin. If you don't need Obeliskial Essentials for your mod, 
    // delete the BepInDependency and the associated code "RegisterMod()" below.

    // If you have other dependencies, such as obeliskial content, make sure to include them here.
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("com.stiffmeds.obeliskialessentials", BepInDependency.DependencyFlags.SoftDependency)] // this is the name of the .dll in the !libs folder.
    [BepInProcess("AcrossTheObelisk.exe")] //Don't change this
    // [BepInIncompatibility("com.stiffmeds.obeliskialessentials")]

    // If PluginInfo isn't working, you are either:
    // 1. Using BepInEx v6
    // 2. Have an issue with your csproj file (not loading the analyzer or BepInEx appropriately)
    // 3. You have an issue with your solution file (not referencing the correct csproj file)


    public class Plugin : BaseUnityPlugin
    {

        // If desired, you can create configs for users by creating a ConfigEntry object here, 
        // and then use config = Config.Bind() to set the title, default value, and description of the config.
        // It automatically creates the appropriate configs.
        public static ConfigEntry<bool> EnableMod { get; set; }
        public static ConfigEntry<bool> EnableDebugging { get; set; }
        public static ConfigEntry<int> ChanceAtMythic { get; set; }
        public static ConfigEntry<bool> EnableIncreasedRewards { get; set; }
        public static ConfigEntry<bool> EnableIncreasedRewardsForAllBosses { get; set; }

        public static string debugBase = $"{PluginInfo.PLUGIN_GUID} ";

        internal static int ModDate = 20250605; //int.Parse(DateTime.Today.ToString("yyyyMMdd"));
        private readonly Harmony harmony = new(PluginInfo.PLUGIN_GUID);
        internal static ManualLogSource Log;
        public static string PluginName;
        public static string PluginVersion;
        public static string PluginGUID;
        private void Awake()
        {

            Log = Logger;
            Log.LogInfo($"{PluginInfo.PLUGIN_GUID} {PluginInfo.PLUGIN_VERSION} has loaded!");

            EnableMod = Config.Bind(new ConfigDefinition(PluginInfo.PLUGIN_NAME, "EnableMod"), true, new ConfigDescription("Enables the mod. If false, the mod will not work then next time you load the game."));
            EnableDebugging = Config.Bind(new ConfigDefinition(PluginInfo.PLUGIN_NAME, "EnableDebugging"), true, new ConfigDescription("Enables the debugging"));
            ChanceAtMythic = Config.Bind(new ConfigDefinition(PluginInfo.PLUGIN_NAME, "ChanceAtMythic"), 50, new ConfigDescription("The percent chance that the end of act boss will drop a mythic card. 0-100."));
            EnableIncreasedRewards = Config.Bind(new ConfigDefinition(PluginInfo.PLUGIN_NAME, "EnableIncreasedRewards"), true, new ConfigDescription("End of Act Bosses drop 4 cards rather than 3"));
            EnableIncreasedRewardsForAllBosses = Config.Bind(new ConfigDefinition(PluginInfo.PLUGIN_NAME, "IncreasedRewardsForAllBosses"), true, new ConfigDescription("All Bosses drop 4 cards rather than 3"));
            ChanceAtMythic.Value = Math.Clamp(ChanceAtMythic.Value, 0, 100); // Ensure the value is between 0 and 100
                                                                             // ChanceAtMythic.Value = 100;
                                                                             // EnableIncreasedRewards.Value = EnableIncreasedRewardsForAllBosses.Value = true;
                                                                             // Log.LogInfo($"{PluginInfo.PLUGIN_GUID} second test (pre-register)");
                                                                             // OnlyImmortalPurples.Value = true;

            PluginName = PluginInfo.PLUGIN_NAME;
            PluginVersion = PluginInfo.PLUGIN_VERSION;
            PluginGUID = PluginInfo.PLUGIN_GUID;
            // Log.LogInfo($"{PluginInfo.PLUGIN_GUID} third test (pre patch)");

            // apply patches
            if (EnableMod.Value)
            {
                EssentialsCompatibility.EssentialsRegister();
                harmony.PatchAll();

            }

            // Log.LogInfo($"{PluginInfo.PLUGIN_GUID} fourth test(post patch)");

        }

        internal static void LogDebug(string msg)
        {
            if (EnableDebugging.Value)
            {
                Log.LogDebug(debugBase + msg);
            }

        }
        internal static void LogInfo(string msg)
        {
            Log.LogInfo(debugBase + msg);
        }
        internal static void LogError(string msg)
        {
            Log.LogError(debugBase + msg);
        }
    }
}
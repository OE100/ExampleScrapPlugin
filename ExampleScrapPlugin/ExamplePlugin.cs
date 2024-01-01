using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using ExampleScrapPlugin.Patches;
using HarmonyLib;
using LethalLib.Modules;
using UnityEngine;

namespace ExampleScrapPlugin
{
    [BepInPlugin(GUID:GUID, Name:NAME, Version:VERSION)]
    public class ExamplePlugin : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony(GUID);
        
        private const string GUID = "oe.example.plugin";
        private const string NAME = "Monkey Scrap Examples";
        private const string VERSION = "1.0.0";

        internal static ExamplePlugin Instance;

        internal static ManualLogSource Log;

        internal static AssetBundle ab;

        private void Awake()
        {
            Log = Logger;
            Log.LogInfo($"'{NAME}' is loading...");

            if (Instance == null)
                Instance = this;
            
            Log.LogDebug("Trying to load asset bundle");
            ab = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(Assembly.GetExecutingAssembly().GetManifestResourceNames()[0]));
            if (ab == null)
            {
                Log.LogError("Failed to load asset bundle");
                return;
            }
            
            // register the item to spawn
            RegisterItem(itemName:"MonkeyBasic.asset");
            RegisterItem(itemName:"MonkeySound.asset", pathToItem:"assets/templates/monkeysound/");
            RegisterItem(itemName:"DiscoMonkey.asset", pathToItem:"assets/templates/discomonkey/");
            
            harmony.PatchAll();

            Log.LogInfo($"'{NAME}' loaded!");
        }
        
        /*
         * This function registers scrap items to spawn on moons via the great LethalLib
         * (Given to us by the almighty Evaisa)
         */
        private void RegisterItem(string itemName, string pathToItem="assets/templates/monkey/")
        {
            // loads asset from the asset bundle we provided
            Item item = ab.LoadAsset<Item>(pathToItem + itemName);
            if (item == null)
                Log.LogError($"Failed to load {itemName} from ab");
            else
            {
                Items.RegisterScrap(item, 50, Levels.LevelTypes.All);
                NetworkStuffPatch.networkPrefabs.Add(item.spawnPrefab);
            }
        }
    }
}
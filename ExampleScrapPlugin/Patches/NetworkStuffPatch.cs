using System.Collections.Generic;
using HarmonyLib;
using LethalLib;
using Unity.Netcode;
using UnityEngine;

namespace ExampleScrapPlugin.Patches
{
    [HarmonyPatch]
    public class NetworkStuffPatch
    {
        internal static List<GameObject> networkPrefabs = new List<GameObject>();
        
        [HarmonyPatch(typeof(GameNetworkManager), "Start"), HarmonyPostfix]
        public static void PatchGameNetworkManagerStart()
        {
            if (networkPrefabs.Count != 0)
                return;
            
            foreach (GameObject networkPrefab in networkPrefabs)
                NetworkManager.Singleton.AddNetworkPrefab(networkPrefab);
        }
    }
}
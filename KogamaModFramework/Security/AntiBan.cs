using HarmonyLib;
using Il2Cpp;
using Il2CppMV.Common;
using UnityEngine;

namespace KogamaModFramework.Security;

[HarmonyPatch]
public static class AntiBan
{
    public static bool Enabled = false;
    public static void Initialize()
    {
        State.InitializeAllPatches();
        Enabled = true;
    }

    // tysm beckowl

    [HarmonyPatch(typeof(CheatHandling), "Init")]
    [HarmonyPatch(typeof(CheatHandling), "ExecuteBan")]
    [HarmonyPatch(typeof(CheatHandling), "CheatSoftwareRunningDetected")]
    [HarmonyPatch(typeof(CheatHandling), "TextureHackDetected")]
    [HarmonyPatch(typeof(CheatHandling), "MachineBanDetected")]
    [HarmonyPatch(typeof(MVNetworkGame.OperationRequests), "Ban", typeof(int), typeof(MVPlayer), typeof(string))]
    [HarmonyPatch(typeof(MVNetworkGame.OperationRequests), "Ban", typeof(CheatType))]
    [HarmonyPatch(typeof(MVNetworkGame.OperationRequests), "Expel")]
    [HarmonyPatch(typeof(MVNetworkGame.OperationRequests), "Kick")]
    [HarmonyPrefix]
    private static bool NoBan() => !Enabled;

}
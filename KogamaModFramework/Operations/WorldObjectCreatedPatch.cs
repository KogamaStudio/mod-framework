using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Il2Cpp;

namespace KogamaModFramework.Operations;

[HarmonyPatch]
internal static class WorldObjectCreatedPatch
{
    public static event System.Action<int, MVWorldObjectClient> OnWorldObjectCreated;

    [HarmonyPatch(typeof(WorldNetwork), "CreateQueryEvent")]
    [HarmonyPrefix]
    private static void OnCreateQueryEvent(WorldNetwork __instance, MVWorldObjectClient root, int instigatorActorNumber)
    {
        OnWorldObjectCreated?.Invoke(root.Id, root);
    }
}

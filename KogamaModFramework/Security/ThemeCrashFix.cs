using HarmonyLib;
using Il2Cpp;
using KogamaModFramework.Commands;
using MelonLoader;
using UnityEngine;

namespace KogamaModFramework.Security;

[HarmonyPatch]
public class ThemeCrashFix
{
    public static bool Enabled = false;
    public static void Initialize()
    {
        State.InitializeAllPatches();
        Enabled = true;
    }

    // tysm Veni


    [HarmonyPatch(typeof(Theme), "Initialize", new Type[] { })]
    [HarmonyPrefix]
    private static bool BlockThemeInit() => !Enabled;

    [HarmonyPatch(typeof(Theme), "Initialize", new Type[] { typeof(int) })]
    [HarmonyPrefix]
    private static bool BlockThemeInitInt() => !Enabled;

    [HarmonyPatch(typeof(ThemeSkybox), "Activate")]
    [HarmonyPrefix]
    private static bool BlockSkyboxActivate() => !Enabled;
}

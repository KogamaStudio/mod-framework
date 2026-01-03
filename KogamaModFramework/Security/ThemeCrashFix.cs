using HarmonyLib;
using Il2Cpp;

namespace KogamaModFramework.Security;

[HarmonyPatch]
internal class ThemeCrashFix
{
    [HarmonyPatch(typeof(Theme), "Initialize", typeof(int))]
    [HarmonyPrefix]
    private static bool BlockThemeInitialize() => false;
}


using HarmonyLib;
using Il2Cpp;

namespace KogamaModFramework.Commands;

[HarmonyPatch]
internal class CommandInterceptor
{
    [HarmonyPatch(typeof(SendMessageControl), "HandleChatCommands")]
    [HarmonyPrefix]
    private static bool HandleChat(string chatMsg)
    {
        if (!CommandManager.Enabled) return true;
        if (chatMsg.StartsWith("/"))
        {
            string[] parts = chatMsg[1..].Split(' ');
            KogamaModFramework.Commands.CommandManager.Execute(parts[0], parts[1..]);
            return false;
        }

        return true;
    }
}


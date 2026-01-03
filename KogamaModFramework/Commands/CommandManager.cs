using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace KogamaModFramework.Commands;

public static class CommandManager
{
    private static Dictionary<string, Command> commands = new();
    private static bool initialized = false;

    public static void Initialize()
    {
        if (initialized) return;

        var harmony = new HarmonyLib.Harmony("KogamaModFramework");
        harmony.PatchAll(typeof(CommandInterceptor).Assembly);
        initialized = true;
    }

    public static void Register(Command cmd)
    {
        commands[cmd.Name.ToLower()] = cmd;
    }

    public static void Execute(string cmdName, string[] args)
    {
        if (commands.TryGetValue(cmdName.ToLower(), out var cmd)) 
            cmd.Execute(args);
    }
}


using Godot;
using HarmonyLib;
using Mariner.MarinerCode.Monsters;
using Mariner.MarinerCode.Nodes;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace Mariner.MarinerCode.Patches;

[HarmonyPatch(typeof(NCreature), nameof(NCreature._Ready))]
internal class NCreatureReadyPatch
{
    [HarmonyPrefix]
    private static void Prefix(NCreature __instance)
    {
        if (!__instance.Entity.IsPlayer) return;
        var manager = NBarnacleManager.Create(__instance, LocalContext.IsMe(__instance.Entity));
        __instance.AddChildSafely(manager);
        manager.Position = Vector2.Zero;
    }
}

[HarmonyPatch(typeof(NCreature), nameof(NCreature.SetOrbManagerPosition))]
internal class NCreatureSetOrbManagerPositionPatch
{
    [HarmonyPostfix]
    private static void Postfix(NCreature __instance)
    {
        if (!__instance.Entity.IsPlayer) return;
        var manager = NBarnacleManager.NBarnacleManagerField.Get(__instance);

        if (manager == null) return;
        manager.Scale = __instance.Visuals.Scale.X > 1f
            ? Vector2.One
            : __instance.Visuals.Scale.Lerp(Vector2.One, 0.5f);
        manager.Position = __instance.Visuals.OrbPosition.Position * Mathf.Min(__instance.Visuals.Scale.X, 1.25f);
    }
}

[HarmonyPatch(typeof(NCreature), "AnimDie")]
internal class NCreatureAnimDiePatch
{
    [HarmonyPostfix]
    private static async Task Postfix(Task results, NCreature __instance, bool shouldRemove)
    {
        if (shouldRemove && __instance.Entity.Monster is Barnacle barn)
        {
            var node = NCombatRoom.Instance?.GetCreatureNode(barn.PlayerOwner.Creature);
            if(node == null) 
                return;
            NBarnacleManager.NBarnacleManagerField.Get(node)?.RemoveBarnacle(__instance);
        }
        
        await results;
        
        if (!RunManager.Instance.IsSingleplayerOrFakeMultiplayer)
        {
            var manager = NBarnacleManager.NBarnacleManagerField.Get(__instance);
            manager?.Clear();
        }
    }
}

[HarmonyPatch(typeof(NCreature), "OnCombatEnded")]
internal class NCreatureOnCombatEndedPatch
{
    [HarmonyPrefix]
    private static void Prefix(NCreature __instance)
    {
        var manager = NBarnacleManager.NBarnacleManagerField.Get(__instance);
        manager?.Clear();
    }
}
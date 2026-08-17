using HarmonyLib;
using Mariner.MarinerCode.Monsters;
using MegaCrit.Sts2.Core.Entities.Creatures;

namespace Mariner.MarinerCode.Patches;

public class CreaturePatches
{
    [HarmonyPatch(typeof(Creature), nameof(Creature.ScaleMonsterHpForMultiplayer))]
    public class FilterForCombatPatch
    {
        public static bool Prefix(Creature __instance)
        {
            return __instance.Monster is not Barnacle;
        }
    }
}
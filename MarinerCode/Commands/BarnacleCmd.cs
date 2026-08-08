using Godot;
using Mariner.MarinerCode.Monsters;
using Mariner.MarinerCode.Nodes;
using Mariner.MarinerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace Mariner.MarinerCode.Commands;

public static class BarnacleCmd
{
    public const int BarnacleCap = 8;

    public static async Task Spawn(
        PlayerChoiceContext choiceContext,
        Player summoner,
        AbstractModel? source)
    {
        var combatState = summoner.Creature.CombatState;
        var barnacle = (Barnacle) ModelDb.Monster<Barnacle>().ToMutable();
        barnacle.PlayerOwner = summoner;
        var playerNode = NCombatRoom.Instance?.GetCreatureNode(summoner.Creature);
        if(!CheckIfPossibleAndShowThoughtBubbleIfNot(playerNode))
            return;
        
        var creature = await CreatureCmd.Add(barnacle, combatState);
        var node = NCombatRoom.Instance?.GetCreatureNode(creature);
        if(playerNode != null)
            NBarnacleManager.NBarnacleManagerField.Get(playerNode)?.AddBarnacle(node);
        if (node != null && source is CardModel)
        {
            node.Modulate = Colors.Transparent;
            node.CreateTween().TweenProperty(node, (NodePath) "modulate", Colors.White, 0.35).SetDelay(0.1);
        }

        var power = (EnrichmentPower) ModelDb.Power<EnrichmentPower>().ToMutable();
        power.PlayerOwner = summoner;
        await PowerCmd.Apply(choiceContext, power, creature, 1M, null, null);
    }
    
    private static bool CheckIfPossibleAndShowThoughtBubbleIfNot(NCreature playerNode)
    {
        var manager = NBarnacleManager.NBarnacleManagerField.Get(playerNode);
        if (manager.Barnacles.Count < BarnacleCap)
            return true;
        ThinkCmd.Play(new LocString("combat_messages", "MARINER-BARNACLE_FULL"), playerNode.Entity, 2.0);
        return false;
    }
}
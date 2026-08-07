using Godot;
using Mariner.MarinerCode.Monsters;
using Mariner.MarinerCode.Nodes;
using Mariner.MarinerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
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
        var creature = await CreatureCmd.Add(barnacle, combatState);
        var node = NCombatRoom.Instance?.GetCreatureNode(creature);
        var playerNode = NCombatRoom.Instance?.GetCreatureNode(summoner.Creature);
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
}
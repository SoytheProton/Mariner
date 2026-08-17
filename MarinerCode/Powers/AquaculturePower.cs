using Mariner.MarinerCode.Monsters;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mariner.MarinerCode.Powers;

public sealed class AquaculturePower : MarinerPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDeath(
        PlayerChoiceContext choiceContext,
        Creature target,
        bool wasRemovalPrevented,
        float deathAnimLength)
    {
        if (target.Side == Owner.Side)
            return;
        Flash();
        await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies.Where(c => c.Monster is not Barnacle), Amount, ValueProp.Unpowered, Owner);
    }
}
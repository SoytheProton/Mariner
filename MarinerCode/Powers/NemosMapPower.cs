using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mariner.MarinerCode.Powers;

public sealed class NemosMapPower : MarinerPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task BeforeSideTurnEndEarly(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        var enumerable = participants.ToList();
        if (!enumerable.Contains(Owner))
            return;
        Flash();
        foreach (var unused in enumerable.Where(c => c.Side == CombatSide.Enemy))
        {
            await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null, true);
            await Cmd.Wait(0.1f);
        }

    }
    
}
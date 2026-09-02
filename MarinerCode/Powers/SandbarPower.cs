using Mariner.MarinerCode.Commands.CombatHistoryEntries;
using Mariner.MarinerCode.Interfaces;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mariner.MarinerCode.Powers;

public sealed class SandbarPower : MarinerPower, IAfterSubmergeHook
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;


    public async Task AfterCardSubmerged(PlayerChoiceContext choiceContext, CardModel card)
    {
        if(card.Owner.Creature != Owner)
            return;
        await CardCmd.AutoPlay(choiceContext, card, null);
        await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, Amount, ValueProp.Unpowered, Owner);
    }
}
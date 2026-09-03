using Mariner.MarinerCode.Commands.CombatHistoryEntries;
using Mariner.MarinerCode.Interfaces;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Mariner.MarinerCode.Powers;

public sealed class VoyagerFormPower : MarinerPower, IAfterSubmergeHook
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;


    public async Task AfterCardSubmerged(PlayerChoiceContext choiceContext, CardModel card)
    {
        if(card.Owner.Creature != Owner || CombatManager.Instance.History.Entries.OfType<CardSubmergedEntry>().Count(e => e.HappenedThisTurn(CombatState) && e.Actor == Owner) > Amount)
            return;
        await CardCmd.AutoPlay(choiceContext, card, null);
        Flash();
    }
}
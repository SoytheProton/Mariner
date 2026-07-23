using Mariner.MarinerCode.Commands.CombatHistoryEntries;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Models;

namespace Mariner.MarinerCode.Extensions;

public static class CombatHistoryExtensions
{
    public static void CardSubmerged(this CombatHistory history, ICombatState combatState, CardModel card)
    {
        history.Add(combatState, new CardSubmergedEntry(card, combatState.RoundNumber, combatState.CurrentSide, history, combatState.Players));
    }
}
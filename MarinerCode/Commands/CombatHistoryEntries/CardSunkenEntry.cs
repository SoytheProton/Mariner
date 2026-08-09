using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace Mariner.MarinerCode.Commands.CombatHistoryEntries;

public class CardSunkenEntry : CombatHistoryEntry
{
    public CardModel Card { get; }
    
    public int PlayAmount { get; }

    public override string Description => $"{Actor.Player.Character.Id.Entry}'s {Card.Id.Entry} was sunken.";

    public CardSunkenEntry(
        CardModel card,
        int playAmount,
        int roundNumber,
        CombatSide currentSide,
        CombatHistory history,
        IEnumerable<Player> players)
        : base(card.Owner.Creature, roundNumber, currentSide, history, players)
    {
        Card = card;
        PlayAmount = playAmount;
    }
}
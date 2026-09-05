using Mariner.MarinerCode.Interfaces;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Mariner.MarinerCode.Powers;

public class BioluminescencePower() : MarinerPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType => 
        PowerStackType.Counter;

    protected override object InitInternalData() => new Data();

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        GetInternalData<Data>().sunkenPlayedThisTurn = CombatManager.Instance.History.CardPlaysStarted.Count(e => e.CardPlay.Card is ISunkenCard && e.CardPlay.Player == Owner.Player && e.HappenedThisTurn(CombatState));
        return Task.CompletedTask;
    }

    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player || cardPlay.Card is not ISunkenCard)
            return;
        ++GetInternalData<Data>().sunkenPlayedThisTurn;
        if (GetInternalData<Data>().sunkenPlayedThisTurn != 3)
            return;
        Flash();
        for (int i = 0; i < Amount; ++i)
        {
            await CardPileCmd.AddGeneratedCardToCombat(cardPlay.Card.CreateClone(), PileType.Hand, Owner.Player);
        }
    }

    public override Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner))
            return Task.CompletedTask;
        GetInternalData<Data>().sunkenPlayedThisTurn = 0;
        return Task.CompletedTask;
    }

    public class Data
    {
        public int sunkenPlayedThisTurn;
    }

    
}
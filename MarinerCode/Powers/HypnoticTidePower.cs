using Mariner.MarinerCode.Interfaces;
using Mariner.MarinerCode.Patches;
using Mariner.MarinerCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Mariner.MarinerCode.Powers;


public class HypnoticTidePower() : MarinerPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player || cardPlay.Card is not IAbyssalCard card)
            return;
        for (var i = 0; i < Amount; i++)
        {
            await card.BeforeShuffled(choiceContext);
        }
    }

}
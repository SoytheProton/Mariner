using Mariner.MarinerCode.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Mariner.MarinerCode.Powers;

public class PetrichorPower() : MarinerPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override Decimal ModifyHandDraw(Player player, Decimal count)
    {
        return player != Owner.Player ? count : count + Amount;
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player)
            return;
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, Amount);
        CardModel[] array = (await CardSelectCmd.FromHand(choiceContext, Owner.Player, prefs, null, this)).ToArray();
        if (array.Length == 0)
            return;
        await CardPileCmd.Add(array, PileType.Draw, CardPilePosition.Random);
    }
}
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Mariner.MarinerCode.Powers;

public sealed class PetrichorPower : MarinerPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        return player != Owner.Player ? count : count + Amount;
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player)
            return;
        var prefs = new CardSelectorPrefs(SelectionScreenPrompt, Amount);
        var array = (await CardSelectCmd.FromHand(choiceContext, Owner.Player, prefs, null, this)).ToArray();
        if (array.Length == 0)
            return;
        await CardPileCmd.Add(array, PileType.Draw, CardPilePosition.Random);
    }
}
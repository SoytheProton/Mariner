using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace Mariner.MarinerCode.Commands;

public static class DredgeCmd
{
    private static LocString DredgeSelectionPrompt => new("card_selection", "MARINER-TO_DREDGE");

    public static async Task<CardModel?> Dredge(PlayerChoiceContext choiceContext, Player player, int amount)
    {
        if (CombatManager.Instance.IsOverOrEnding)
            return null;

        var discardPile = PileType.Discard.GetPile(player);
        if(discardPile.IsEmpty)
            return null;

        var cards = discardPile.Cards.TakeLast(amount).ToList();

        var cardToAdd = (await CardSelectCmd.FromSimpleGrid(choiceContext, cards, player, new CardSelectorPrefs(DredgeSelectionPrompt, 1))).FirstOrDefault();

        if (cardToAdd == null)
            return null;
        
        await CardPileCmd.Add(cardToAdd, PileType.Hand);
        
        discardPile.InvokeContentsChanged();
        return cardToAdd;
    }
}
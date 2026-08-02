using Mariner.MarinerCode.Cards;
using Mariner.MarinerCode.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Mariner.MarinerCode.Commands;

public static class SubmergeCmd
{
    public static async Task Submerge(PlayerChoiceContext choiceContext, Player player, int amount = 1)
    {
        var cards = new List<CardModel>(amount);
        var drawPile = PileType.Draw.GetPile(player);
        for (var i = 0; i < amount; ++i)
        {
            var card = drawPile.Cards.ElementAtOrDefault(i);
            if (card == null)
                break;
            cards.Add(card);
        }
        await Submerge(choiceContext, cards);
    }

    public static async Task Submerge(PlayerChoiceContext choiceContext, CardModel card)
    {
        await Submerge(choiceContext, [card]);
    }

    public static async Task Submerge(PlayerChoiceContext choiceContext, IEnumerable<CardModel> cards)
    {
        if (CombatManager.Instance.IsOverOrEnding)
            return;
        
        var submergedCards = cards.ToList();
        
        if(submergedCards.Count == 0)
            return;
        var discardPile = PileType.Discard.GetPile(submergedCards[0].Owner);
        var hand = PileType.Hand.GetPile(submergedCards[0].Owner);
        var combatState = submergedCards[0].CombatState ?? submergedCards[0].Owner.Creature.CombatState;
        
        foreach (var card in submergedCards)
        {
            var targetPile = card.Keywords.Contains(MarinerCardKeywords.Trawl) ? hand : discardPile;
            await CardPileCmd.Add(card, targetPile);
            CombatManager.Instance.History.CardSubmerged(combatState, card);
            await MarinerHook.AfterCardSubmerged(combatState, choiceContext, card);
        }
        discardPile.InvokeContentsChanged();
    }
}
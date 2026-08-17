using Mariner.MarinerCode.Cards.Variables;
using Mariner.MarinerCode.Commands;
using Mariner.MarinerCode.Extensions;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Mariner.MarinerCode.Cards.Common;

public sealed class Resurface() : MarinerCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new SubmergeVar(4)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await SubmergeCmd.Submerge(choiceContext, DynamicVars.Submerge().BaseValue, Owner);
        var card2 = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Discard.GetPile(Owner), Owner, new CardSelectorPrefs(SelectionScreenPrompt, 1))).FirstOrDefault();
        if (card2 == null)
            return;
        await CardPileCmd.Add(card2, PileType.Draw, CardPilePosition.Top);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Submerge().UpgradeValueBy(2M);
    }
}
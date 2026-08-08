using BaseLib.Utils;
using Mariner.MarinerCode.Character;
using Mariner.MarinerCode.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Mariner.MarinerCode.Cards.Rare;

[Pool(typeof(MarinerCardPool))]
public class DeepestDepths() : MarinerCard(3,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MarinerCardKeywords.Ballast)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        var cards = await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this);
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        var selectedCard = cards.FirstOrDefault();
        if (selectedCard == null)
            return;
        (await PowerCmd.Apply<DeepestDepthsPower>(choiceContext, Owner.Creature, 1M, Owner.Creature, this))?.SetSelectedCard(selectedCard);
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}
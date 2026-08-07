using BaseLib.Utils;
using Mariner.MarinerCode.Cards;
using Mariner.MarinerCode.Character;
using Mariner.MarinerCode.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

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
        DeepestDepths nightmare = this;
        CardSelectorPrefs prefs = new CardSelectorPrefs(nightmare.SelectionScreenPrompt, 1);
        IEnumerable<CardModel> cards = await CardSelectCmd.FromHand(choiceContext, nightmare.Owner, prefs, (Func<CardModel, bool>) null, (AbstractModel) nightmare);
        await CreatureCmd.TriggerAnim(nightmare.Owner.Creature, "Cast", nightmare.Owner.Character.CastAnimDelay);
        CardModel selectedCard = cards.FirstOrDefault<CardModel>();
        if (selectedCard == null)
        {
            cards = (IEnumerable<CardModel>) null;
            selectedCard = (CardModel) null;
        }
        else
        {
            (await PowerCmd.Apply<DeepestDepthsPower>(choiceContext, nightmare.Owner.Creature, 1M, nightmare.Owner.Creature, (CardModel) nightmare)).SetSelectedCard(selectedCard);
            cards = (IEnumerable<CardModel>) null;
            selectedCard = (CardModel) null;
        }
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}
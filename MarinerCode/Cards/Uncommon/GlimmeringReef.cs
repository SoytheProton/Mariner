using BaseLib.Utils;
using Mariner.MarinerCode.Cards.Variables;
using Mariner.MarinerCode.Character;
using Mariner.MarinerCode.Commands;
using Mariner.MarinerCode.Extensions;
using Mariner.MarinerCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Mariner.MarinerCode.Cards.Uncommon;

[Pool(typeof(MarinerCardPool))]
public sealed class GlimmeringReef() : MarinerCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self), IAbyssalCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new SubmergeVar(2M), new CardsVar("Cards2", 2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(MarinerStaticHovertip.Abyssal)];
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars["Cards2"].BaseValue, Owner);
        await SubmergeCmd.Submerge(choiceContext, DynamicVars.Submerge().BaseValue, Owner);
    }

    public async Task BeforeShuffled(PlayerChoiceContext choiceContext)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1M);
        DynamicVars.Submerge().UpgradeValueBy(1M);
    }
}
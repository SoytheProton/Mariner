using Mariner.MarinerCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Mariner.MarinerCode.Cards.Common;

public sealed class Twilight() : MarinerCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.AnyEnemy), IAfterSubmergeHook
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WeakPower>(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(MarinerStaticHovertip.Submerge)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [MarinerCardKeywords.Trawl];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<WeakPower>(choiceContext, play.Target, DynamicVars.Weak.BaseValue, Owner.Creature, this);
    }

    public async Task AfterCardSubmerged(
        PlayerChoiceContext choiceContext, CardModel card)
    {
        if (card != this)
            return;
        SetToFreeThisTurn();
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Weak.UpgradeValueBy(1M);
    }
}
using BaseLib.Abstracts;
using BaseLib.Utils;
using Mariner.MarinerCode.Character;
using Mariner.MarinerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Mariner.MarinerCode.Cards.Ancient;

[Pool(typeof(MarinerCardPool))]
public sealed class Tenebris() : MarinerCard(2,
    CardType.Power, CardRarity.Ancient,
    TargetType.Self), ITomeCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new RepeatVar(1)];

    protected override HashSet<CardTag> CanonicalTags => [];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<TenebrisPower>(choiceContext, Owner.Creature, DynamicVars.Repeat.BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}
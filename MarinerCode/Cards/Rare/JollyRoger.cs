using BaseLib.Extensions;
using BaseLib.Utils;
using Mariner.MarinerCode.Cards;
using Mariner.MarinerCode.Character;
using Mariner.MarinerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Mariner.MarinerCode.Cards.Rare;

[Pool(typeof(MarinerCardPool))]
public sealed class JollyRoger() : MarinerCard(1,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    public override bool CanBeGeneratedInCombat => false;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<JollyRogerPower>(1)];

    protected override HashSet<CardTag> CanonicalTags => [];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [MarinerCardKeywords.Ballast];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<JollyRogerPower>(choiceContext, Owner.Creature, DynamicVars.Power<JollyRogerPower>().BaseValue, Owner.Creature, this);

    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(MarinerCardKeywords.Ballast);
    }
}
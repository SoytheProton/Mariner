using BaseLib.Extensions;
using BaseLib.Utils;
using Mariner.MarinerCode.Character;
using Mariner.MarinerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Mariner.MarinerCode.Cards.Uncommon;

[Pool(typeof(MarinerCardPool))]
public sealed class ShiftingSeas() : MarinerCard(2,
    CardType.Power, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ShiftingSeasPower>(2)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<ShiftingSeasPower>(choiceContext, Owner.Creature, DynamicVars.Power<ShiftingSeasPower>().BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => DynamicVars["ShiftingSeasPower"].UpgradeValueBy(1);
}
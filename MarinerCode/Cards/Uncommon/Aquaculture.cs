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
public sealed class Aquaculture() : MarinerCard(1,
    CardType.Power, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<AquaculturePower>(9)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<AquaculturePower>(choiceContext, Owner.Creature, DynamicVars.Power<AquaculturePower>().BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => DynamicVars.Power<AquaculturePower>().UpgradeValueBy(3M);
}
using BaseLib.Utils;
using Mariner.MarinerCode.Cards.Variables;
using Mariner.MarinerCode.Character;
using Mariner.MarinerCode.Commands;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Mariner.MarinerCode.Cards.Uncommon;

[Pool(typeof(MarinerCardPool))]
public sealed class Hoist() : MarinerCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DredgeVar("Dredge1", 2), new DredgeVar("Dredge2", 4), new DredgeVar("Dredge3", 6)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await DredgeCmd.Dredge(choiceContext, DynamicVars["Dredge1"].BaseValue, Owner);
        await DredgeCmd.Dredge(choiceContext, DynamicVars["Dredge2"].BaseValue, Owner);
        await DredgeCmd.Dredge(choiceContext, DynamicVars["Dredge3"].BaseValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Dredge1"].UpgradeValueBy(2M);
        DynamicVars["Dredge2"].UpgradeValueBy(2M);
        DynamicVars["Dredge3"].UpgradeValueBy(2M);
    }
}
using BaseLib.Utils;
using Mariner.MarinerCode.Cards;
using Mariner.MarinerCode.Cards.Variables;
using Mariner.MarinerCode.Character;
using Mariner.MarinerCode.Commands;
using Mariner.MarinerCode.Extensions;
using Mariner.MarinerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Mariner.MarinerCode.Cards.Uncommon;

[Pool(typeof(MarinerCardPool))]
public class HighTide() : MarinerCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2), new DredgeVar(5)];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.PowerUpAnimDelay);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        await DredgeCmd.Dredge(choiceContext, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
        DynamicVars.Dredge().UpgradeValueBy(1);
    }
}
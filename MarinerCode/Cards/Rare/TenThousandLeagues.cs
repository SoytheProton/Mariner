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

namespace Mariner.MarinerCode.Cards.Rare;

[Pool(typeof(MarinerCardPool))]
public class TenThousandLeagues() : MarinerCard(3,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new SubmergeVar(15), new DredgeVar(15), new RepeatVar(4)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.PowerUpAnimDelay);
        await SubmergeCmd.Submerge(choiceContext, Owner, DynamicVars.Submerge().IntValue);
        for (int i = 0; i < DynamicVars.Repeat._baseValue; i++)
        {
            await DredgeCmd.Dredge(choiceContext, Owner, DynamicVars.Dredge().IntValue);   
        }
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}
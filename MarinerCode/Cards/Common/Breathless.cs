using Mariner.MarinerCode.Cards.Variables;
using Mariner.MarinerCode.Commands;
using Mariner.MarinerCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Mariner.MarinerCode.Cards.Common;

public class Breathless() : MarinerCard(2,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new SubmergeVar(13), new DredgeVar(13)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await SubmergeCmd.Submerge(choiceContext, Owner, DynamicVars.Submerge().IntValue);
        await DredgeCmd.Dredge(choiceContext, Owner, DynamicVars.Dredge().IntValue);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Submerge().UpgradeValueBy(3M);
        DynamicVars.Dredge().UpgradeValueBy(3M);
    }
}
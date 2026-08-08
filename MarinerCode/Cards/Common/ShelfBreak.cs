using Mariner.MarinerCode.Cards.Variables;
using Mariner.MarinerCode.Commands;
using Mariner.MarinerCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mariner.MarinerCode.Cards.Common;

public class ShelfBreak() : MarinerCard(2,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(14, ValueProp.Move), new SubmergeVar(5)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        await SubmergeCmd.Submerge(choiceContext, Owner, DynamicVars.Submerge().IntValue);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(4M);
        DynamicVars.Submerge().UpgradeValueBy(2M);
    }
}
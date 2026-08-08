using Mariner.MarinerCode.Cards.Variables;
using Mariner.MarinerCode.Commands;
using Mariner.MarinerCode.Extensions;
using Mariner.MarinerCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mariner.MarinerCode.Cards.Common;

public class DyingReef() : MarinerCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self), IAbyssalCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(7M, ValueProp.Move)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
    }
    
    public async Task BeforeShuffled(PlayerChoiceContext choiceContext)
    {
        await BarnacleCmd.Spawn(choiceContext, Owner, this);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3M);
    }
}
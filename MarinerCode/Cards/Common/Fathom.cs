using Mariner.MarinerCode.Cards.Variables;
using Mariner.MarinerCode.Commands;
using Mariner.MarinerCode.Extensions;
using Mariner.MarinerCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mariner.MarinerCode.Cards.Common;

public class Fathom() : MarinerCard(0,
    CardType.Skill, CardRarity.Common,
    TargetType.Self), ISunkenCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(5M, ValueProp.Move), new BlockVar("Block2", 3M, ValueProp.Move)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
    }
    
    public async Task OnSunken(PlayerChoiceContext choiceContext)
    {
        await PowerCmd.Apply<BlockNextTurnPower>(choiceContext, Owner.Creature, DynamicVars["Block2"].IntValue, Owner.Creature, this);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3M);
        DynamicVars["Block2"].UpgradeValueBy(1M);
    }
}
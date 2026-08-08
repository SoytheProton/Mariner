using BaseLib.Utils;
using Mariner.MarinerCode.Character;
using Mariner.MarinerCode.Interfaces;
using Mariner.MarinerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mariner.MarinerCode.Cards.Uncommon;

[Pool(typeof(MarinerCardPool))]
public sealed class TidePool() : MarinerCard(2,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self), ISunkenCard, IAbyssalCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(11M, ValueProp.Move), new BlockVar("Block2",11M, ValueProp.Move)];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
    }
    
    public async Task OnSunken(PlayerChoiceContext choiceContext)
    {
        await CreatureCmd.GainBlock(Owner.Creature, (BlockVar)DynamicVars["Block2"], null);
    }
    
    public async Task BeforeShuffled(PlayerChoiceContext choiceContext)
    {
        await CreatureCmd.GainBlock(Owner.Creature, (BlockVar)DynamicVars["Block2"], null);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(4M);
        DynamicVars["Block2"].UpgradeValueBy(2M);
    }
}
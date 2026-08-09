using BaseLib.Utils;
using Mariner.MarinerCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mariner.MarinerCode.Cards.Uncommon;

[Pool(typeof(MarinerCardPool))]
public sealed class TranquilWaters() : MarinerCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(10M, ValueProp.Move), new PowerVar<VulnerablePower>(2)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [MarinerCardKeywords.Ballast];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        await PowerCmd.Apply<VulnerablePower>(choiceContext, CombatState?.HittableEnemies, DynamicVars.Vulnerable.BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(4M);
        DynamicVars.Vulnerable.UpgradeValueBy(1M);
    }
}
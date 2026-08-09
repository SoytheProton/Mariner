using Mariner.MarinerCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mariner.MarinerCode.Cards.Common;

public sealed class Wavebreaker() : MarinerCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self), ISunkenCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8M, ValueProp.Move), new DamageVar(6M, ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(MarinerStaticHovertip.Sunken)];
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
    }
    
    public async Task OnSunken(PlayerChoiceContext choiceContext)
    {
        var hp = CombatState.HittableEnemies.Max(c => c.CurrentHp);
        var highHp = Owner.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies.Where(c => c.CurrentHp == hp));
        if(highHp == null)
            return;
        await CreatureCmd.Damage(choiceContext, highHp, DynamicVars.Damage, this, null);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3M);
        DynamicVars.Damage.UpgradeValueBy(3M);
    }
}
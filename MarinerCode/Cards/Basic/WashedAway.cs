using BaseLib.Utils;
using Mariner.MarinerCode.Character;
using Mariner.MarinerCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mariner.MarinerCode.Cards.Basic;

[Pool(typeof(MarinerCardPool))]
public sealed class WashedAway() : MarinerCard(2, CardType.Attack,
    CardRarity.Basic, TargetType.AnyEnemy), ISunkenCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(16M, ValueProp.Move), new DamageVar("Damage2", 6M, ValueProp.Move)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(MarinerStaticHovertip.Sunken)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    }

    public async Task OnSunken(PlayerChoiceContext choiceContext)
    {
        await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, DynamicVars["Damage2"]._baseValue, ValueProp.Move, Owner.Creature);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4M);
        DynamicVars["Damage2"].UpgradeValueBy(2M);
    }
}
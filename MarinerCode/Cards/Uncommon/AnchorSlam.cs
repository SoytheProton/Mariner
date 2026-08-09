using BaseLib.Utils;
using Mariner.MarinerCode.Cards.Variables;
using Mariner.MarinerCode.Character;
using Mariner.MarinerCode.Commands;
using Mariner.MarinerCode.Extensions;
using Mariner.MarinerCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mariner.MarinerCode.Cards.Uncommon;

[Pool(typeof(MarinerCardPool))]
public sealed class AnchorSlam() : MarinerCard(3,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy), IAbyssalCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(26M, ValueProp.Move), new SubmergeVar(3)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(MarinerStaticHovertip.Abyssal)];
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target).WithHitFx("vfx/vfx_heavy_blunt", tmpSfx: "heavy_attack.mp3").Execute(choiceContext);
    }
    
    public async Task BeforeShuffled(PlayerChoiceContext choiceContext)
    {
        await SubmergeCmd.Submerge(choiceContext, DynamicVars.Submerge().BaseValue, Owner);
    }
    
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(6);
}
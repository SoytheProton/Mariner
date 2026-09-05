using BaseLib.Utils;
using Mariner.MarinerCode.Character;
using Mariner.MarinerCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mariner.MarinerCode.Cards.Uncommon;

[Pool(typeof(MarinerCardPool))]
public sealed class StarboardStrike() : MarinerCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy), ISunkenCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(13M, ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(MarinerStaticHovertip.Sunken), HoverTipFactory.Static(MarinerStaticHovertip.Abyssal)];
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);   
    }
    
    public async Task OnSunken(PlayerChoiceContext choiceContext)
    {
        AddKeyword(MarinerCardKeywords.Ballast);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4M);
    }
}
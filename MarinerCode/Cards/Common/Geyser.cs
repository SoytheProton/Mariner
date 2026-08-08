using Mariner.MarinerCode.Cards.Variables;
using Mariner.MarinerCode.Commands;
using Mariner.MarinerCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mariner.MarinerCode.Cards.Common;

public class Geyser() : MarinerCard(2,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(17M, ValueProp.Move), new SubmergeVar(4)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    }
    
    public override async Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw)
    {
        if (card != this)
            return;
        await SubmergeCmd.Submerge(choiceContext, Owner, DynamicVars.Submerge().IntValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5M);
        DynamicVars.Submerge().UpgradeValueBy(2M);
    }
}
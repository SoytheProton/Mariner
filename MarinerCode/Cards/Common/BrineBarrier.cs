using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mariner.MarinerCode.Cards.Common;

public sealed class BrineBarrier() : MarinerCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WeakPower>(2)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<WeakPower>(choiceContext, CombatState?.HittableEnemies, DynamicVars.Weak.BaseValue, Owner.Creature, this);
    }
    
    protected override void OnUpgrade()
    {
        AddKeyword(MarinerCardKeywords.Trawl);
    }
}
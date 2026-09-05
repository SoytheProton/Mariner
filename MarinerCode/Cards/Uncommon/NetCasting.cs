using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mariner.MarinerCode.Cards.Uncommon;

public sealed class NetCasting() : MarinerCard(2,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(17M, ValueProp.Move)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(MarinerStaticHovertip.Lethal)];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).TargetingAllOpponents(CombatState).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "heavy_attack.mp3").Execute(choiceContext);
        var killed = attackCommand.Results.SelectMany(r => r).Count(r => r.WasTargetKilled);
        if(killed <= 0)
            return;
        var prefs = new CardSelectorPrefs(SelectionScreenPrompt, killed);
        var cards = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Discard.GetPile(Owner), Owner, prefs)).ToList();
        if (cards.Count == 0)
            return;
        await CardPileCmd.Add(cards, PileType.Hand);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4M);
    }
}
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
using MegaCrit.Sts2.Core.Models.Powers;

namespace Mariner.MarinerCode.Cards.Uncommon;

[Pool(typeof(MarinerCardPool))]
public sealed class CatchAndRelease() : MarinerCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self), ISunkenCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new SubmergeVar(5), new CardsVar(2), new CardsVar("Cards2", 1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(MarinerStaticHovertip.Sunken)];
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.PowerUpAnimDelay);
        await SubmergeCmd.Submerge(choiceContext, DynamicVars.Submerge().BaseValue, Owner);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }
    
    public async Task OnSunken(PlayerChoiceContext choiceContext)
    {
        await PowerCmd.Apply<DrawCardsNextTurnPower>(choiceContext, Owner.Creature, DynamicVars["Cards2"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => DynamicVars["Cards2"].UpgradeValueBy(1M);

}
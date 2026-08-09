using BaseLib.Utils;
using Mariner.MarinerCode.Character;
using Mariner.MarinerCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Mariner.MarinerCode.Cards.Uncommon;

[Pool(typeof(MarinerCardPool))]
public sealed class Doubloons() : MarinerCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self), ISunkenCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1), new EnergyVar("Energy2", 1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(MarinerStaticHovertip.Sunken)];
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PlayerCmd.GainEnergy(DynamicVars["Energy2"].BaseValue, Owner);
    }
    
    public async Task OnSunken(PlayerChoiceContext choiceContext)
    {
        await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, Owner.Creature, DynamicVars.Energy.BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => DynamicVars.Energy.UpgradeValueBy(1);

}
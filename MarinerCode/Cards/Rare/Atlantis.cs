using Mariner.MarinerCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mariner.MarinerCode.Cards.Rare;

public sealed class Atlantis() : MarinerCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self), ISunkenCard
{
    private decimal _extraBlockFromPlays;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(6M, ValueProp.Move), new("Increase", 2M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(MarinerStaticHovertip.Sunken)];
    private decimal ExtraBlockFromPlays
    {
        get => _extraBlockFromPlays;
        set
        {
            AssertMutable();
            _extraBlockFromPlays = value;
        }
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        DynamicVars.Block.BaseValue += DynamicVars["Increase"].BaseValue;
        ExtraBlockFromPlays += DynamicVars["Increase"].BaseValue;
    }

    public async Task OnSunken(PlayerChoiceContext choiceContext)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, null);
    }
    
    protected override CardLocation GetResultLocationForCardPlay()
    {
        var locationForCardPlay = base.GetResultLocationForCardPlay();
        if (locationForCardPlay.pileType == PileType.Discard)
            locationForCardPlay.pileType = PileType.Hand;
        return locationForCardPlay;
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars["Increase"].UpgradeValueBy(1);
    }

    protected override void AfterDowngraded()
    {
        base.AfterDowngraded();
        var block = DynamicVars.Block;
        block.BaseValue += ExtraBlockFromPlays;
    }
}
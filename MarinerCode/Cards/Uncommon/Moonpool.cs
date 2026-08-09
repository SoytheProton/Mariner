using BaseLib.Utils;
using Mariner.MarinerCode.Character;
using Mariner.MarinerCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Mariner.MarinerCode.Cards.Uncommon;

[Pool(typeof(MarinerCardPool))]
public sealed class Moonpool() : MarinerCard(3,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.None), IAbyssalCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1), new CardsVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(MarinerStaticHovertip.Abyssal)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [MarinerCardKeywords.Ballast];
    
    public async Task BeforeShuffled(PlayerChoiceContext choiceContext)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        if (IsUpgraded) await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }
}
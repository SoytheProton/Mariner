using BaseLib.Utils;
using Mariner.MarinerCode.Character;
using Mariner.MarinerCode.Interfaces;
using Mariner.MarinerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mariner.MarinerCode.Cards.Uncommon;

[Pool(typeof(MarinerCardPool))]
public sealed class Moonpool() : MarinerCard(-1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.None), IAbyssalCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable, MarinerCardKeywords.Ballast];
    
    public async Task BeforeShuffled(PlayerChoiceContext choiceContext)
    {
        await PlayerCmd.GainEnergy(1, Owner);
        if (IsUpgraded) await CardPileCmd.Draw(choiceContext, 1, Owner);
    }
}
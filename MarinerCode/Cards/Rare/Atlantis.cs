using BaseLib.Utils;
using Mariner.MarinerCode.Cards;
using Mariner.MarinerCode.Cards.Other;
using Mariner.MarinerCode.Character;
using Mariner.MarinerCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Mariner.MarinerCode.Cards.Rare;

[Pool(typeof(MarinerCardPool))]
public sealed class Atlantis() : MarinerCard(2,
    CardType.Skill, CardRarity.Rare,
    TargetType.None), ISunkenCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override HashSet<CardTag> CanonicalTags => [];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<MarbleStatue>()];

    public async Task OnSunken(PlayerChoiceContext choiceContext)
    {
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<MarbleStatue>(Owner), PileType.Draw, Owner, CardPilePosition.Random));
    }

    protected override void OnUpgrade()
    {

    }
}
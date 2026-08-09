using Mariner.MarinerCode.Cards;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Mariner.MarinerCode.Powers;

public sealed class DeepestDepthsPower : MarinerPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override PowerStackType StackType => PowerStackType.Single;

    protected override object InitInternalData() => new Data();

    protected override IEnumerable<DynamicVar> CanonicalVars=> [new StringVar("Card")];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MarinerCardKeywords.Ballast)];

    public override async Task BeforeHandDraw(
        Player player,
        PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        if (player != Owner.Player)
            return;
        var card = GetInternalData<Data>().selectedCard;
        await CardPileCmd.AddGeneratedCardToCombat(card.CreateClone(), PileType.Hand, Owner.Player);
    }

    public void SetSelectedCard(CardModel card)
    {
        var clone = card.CreateClone();
        CardCmd.ClearAffliction(clone);
        CardCmd.ApplyKeyword(clone, MarinerCardKeywords.Ballast);
        GetInternalData<Data>().selectedCard = clone;
        ((StringVar) DynamicVars["Card"]).StringValue = clone.Title;
    }

    private class Data
    {
        /// <summary>
        /// This will be null for the moment after this power is applied but before this is set by Nightmare.OnPlay.
        /// For all current use cases, this means we should never see this being null.
        /// However, if we needed to override AfterApplied in here in the future, this would be null in it, so let's
        /// leave this nullable for future-proofing.
        /// </summary>
        public CardModel? selectedCard;
    }
}
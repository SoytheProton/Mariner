using BaseLib.Utils;
using Mariner.MarinerCode.Cards;
using Mariner.MarinerCode.Character;
using Mariner.MarinerCode.Interfaces;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mariner.MarinerCode.Cards.Rare;

[Pool(typeof(MarinerCardPool))]
public sealed class BermudaTriangle() : MarinerCard(2,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self), ISunkenCard, IAbyssalCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(1), 
        new CalculationBaseVar(1M),
        new ExtraDamageVar(1M),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => card.Owner.PlayerCombatState.AllCards.Count(c => c is BermudaTriangle)))];

    protected override HashSet<CardTag> CanonicalTags => [];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        List<CardModel> selection = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(Owner), Owner, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, DynamicVars.Cards.IntValue))).ToList();
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        foreach (CardModel original in selection)
        {
            var card2 = CreateClone();
            CardCmd.Transform(original, card2);
        }
    }
    
    public override async Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw)
    {
        if (card != this)
            return;
        await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, DynamicVars.CalculatedDamage.IntValue, ValueProp.Move, Owner.Creature);
    }
    
    public async Task OnSunken(PlayerChoiceContext choiceContext)
    {
        await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, DynamicVars.CalculatedDamage.IntValue, ValueProp.Move, Owner.Creature);
    }
    
    public async Task BeforeShuffled(PlayerChoiceContext choiceContext)
    {
        await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, DynamicVars.CalculatedDamage.IntValue, ValueProp.Move, Owner.Creature);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}
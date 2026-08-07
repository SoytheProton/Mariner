using BaseLib.Utils;
using Mariner.MarinerCode.Cards;
using Mariner.MarinerCode.Character;
using Mariner.MarinerCode.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Mariner.MarinerCode.Cards.Uncommon;

[Pool(typeof(MarinerCardPool))]
public class LeadBoots() : MarinerCard(1,
    CardType.Power, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<LeadBootsPower>(2)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MarinerCardKeywords.Ballast)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<LeadBootsPower>(choiceContext, Owner.Creature, DynamicVars["LeadBootsPower"]._baseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => DynamicVars["LeadBootsPower"].UpgradeValueBy(1);
}
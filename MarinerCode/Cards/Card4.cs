using Mariner.MarinerCode.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Mariner.MarinerCode.Cards;

public class Card4() : MarinerCard(1, CardType.Attack,
    CardRarity.Basic, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await SubmergeCmd.Submerge(choiceContext, Owner);
    }

    protected override void OnUpgrade()
    {

    }
}
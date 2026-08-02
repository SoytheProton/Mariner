using Mariner.MarinerCode.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Mariner.MarinerCode.Cards;

public class Card6() : MarinerCard(1, CardType.Attack,
    CardRarity.Basic, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DredgeCmd.Dredge(choiceContext, Owner, 3);
    }

    protected override void OnUpgrade()
    {

    }
}
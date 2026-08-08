using Mariner.MarinerCode.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mariner.MarinerCode.Cards;

public class BarnacleSummoner() : MarinerCard(0,
    CardType.Attack, CardRarity.Basic,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    { 
        await BarnacleCmd.Spawn(choiceContext, Owner, this);
        await BarnacleCmd.Spawn(choiceContext, Owner, this);
        await BarnacleCmd.Spawn(choiceContext, Owner, this);
    }

}
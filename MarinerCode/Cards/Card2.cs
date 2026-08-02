using Mariner.MarinerCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mariner.MarinerCode.Cards;

public class Card2() : MarinerCard(1, CardType.Attack,
    CardRarity.Basic, TargetType.Self), ISunkenCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
    }

    protected override void OnUpgrade()
    {

    }

    public async Task OnSunken(PlayerChoiceContext choiceContext)
    {
        await CreatureCmd.GainBlock(Owner.Creature, 1, ValueProp.Move, null);
    }
}
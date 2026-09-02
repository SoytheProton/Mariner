using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Mariner.MarinerCode.Powers;

public sealed class RipCurrentPower : MarinerPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw)
    {
        if(card.Owner != Owner.Player)
            return;
        // slightly jank but it's fine...
        var method = card.GetType().GetMethod(nameof(AfterCardDrawn));
        if (method != null && method.DeclaringType != method.GetBaseDefinition().DeclaringType)
        {
            Flash();
            await CardPileCmd.Draw(choiceContext, Amount, Owner.Player);
        }
    }
}
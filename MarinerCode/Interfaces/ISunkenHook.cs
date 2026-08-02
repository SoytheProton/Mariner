using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Mariner.MarinerCode.Interfaces;

public interface ISunkenHook
{
    public Task AfterCardSunken(PlayerChoiceContext choiceContext, CardModel card);
}
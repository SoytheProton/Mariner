using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Mariner.MarinerCode.Interfaces;

public interface IAfterSunkenHook
{
    public Task AfterCardSunken(PlayerChoiceContext choiceContext, CardModel card);
}
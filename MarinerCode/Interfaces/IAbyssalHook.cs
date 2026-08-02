using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Mariner.MarinerCode.Interfaces;

public interface IAbyssalHook
{
    public Task BeforeCardShuffled(PlayerChoiceContext choiceContext, CardModel card);
}
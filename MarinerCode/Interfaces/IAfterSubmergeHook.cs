using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Mariner.MarinerCode.Interfaces;

public interface IAfterSubmergeHook
{
    public Task AfterCardSubmerged(PlayerChoiceContext choiceContext, CardModel card);
}
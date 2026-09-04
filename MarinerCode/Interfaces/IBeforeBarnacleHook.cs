using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Mariner.MarinerCode.Interfaces;

public interface IBeforeBarnacleHook
{
    public Task BeforeBarnacle(PlayerChoiceContext choiceContext, Player summoner);
}
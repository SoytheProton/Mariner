using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Mariner.MarinerCode.Interfaces;

public interface IAbyssalCard
{
    public Task BeforeShuffled(PlayerChoiceContext choiceContext);
}
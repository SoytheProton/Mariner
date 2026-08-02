using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Mariner.MarinerCode.Interfaces;

public interface ISunkenCard
{
    public Task OnSunken(PlayerChoiceContext choiceContext);
}
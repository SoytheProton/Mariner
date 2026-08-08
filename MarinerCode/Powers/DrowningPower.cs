using Mariner.MarinerCode.Interfaces;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace Mariner.MarinerCode.Powers;

public sealed class DrowningPower : MarinerPower, IModifySunkenHook
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public int ModifySunkenAmount(CardModel card, int playCount)
    {
        return playCount + Amount;
    }

    public Task AfterModifyingSunkenAmount(CardModel card)
    {
        Flash();
        return Task.CompletedTask;
    }
}
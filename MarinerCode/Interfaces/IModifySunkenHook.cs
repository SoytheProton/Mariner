using MegaCrit.Sts2.Core.Models;

namespace Mariner.MarinerCode.Interfaces;

public interface IModifySunkenHook
{
    public int ModifySunkenAmount(CardModel card, int playCount);

    public Task AfterModifyingSunkenAmount(CardModel card);
}
using MegaCrit.Sts2.Core.Models;

namespace Mariner.MarinerCode.Interfaces;

public interface IModifyAbyssalHook
{
    public int ModifyAbyssalAmount(CardModel card, int playCount);

    public Task AfterModifyingAbyssalAmount(CardModel card);
}
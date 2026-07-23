using Mariner.MarinerCode.Interfaces;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;

namespace Mariner.MarinerCode.Commands;

public class MarinerHook
{
    public static async Task AfterCardSubmerged(
        ICombatState combatState,
        PlayerChoiceContext choiceContext,
        CardModel card)
    {
        foreach (var model in Hook.IterateCombatHookListeners(combatState))
        {
            if(model is not ISubmergeHook submergeModel)
                return;
            choiceContext.PushModel(model);
            await submergeModel.AfterCardSubmerged(choiceContext, card);
            model.InvokeExecutionFinished();
            choiceContext.PopModel(model);
        }
    }
    
    public static async Task BeforeCardShuffled(
        ICombatState combatState,
        PlayerChoiceContext choiceContext,
        CardModel card)
    {
        foreach (var model in Hook.IterateCombatHookListeners(combatState))
        {
            if(model is not IAbyssalHook abyssalModel)
                return;
            choiceContext.PushModel(model);
            await abyssalModel.BeforeCardShuffled(choiceContext, card);
            model.InvokeExecutionFinished();
            choiceContext.PopModel(model);
        }
    }
}
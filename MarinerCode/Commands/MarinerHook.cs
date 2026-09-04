using Mariner.MarinerCode.Interfaces;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;

namespace Mariner.MarinerCode.Commands;

public class MarinerHook
{
    public static async Task BeforeBarnacleSummoned(
        ICombatState combatState,
        PlayerChoiceContext choiceContext,
        Player summoner)
    {
        foreach (var model in Hook.IterateCombatHookListeners(combatState))
        {
            if(model is not IBeforeBarnacleHook barnacleHook)
                continue;
            choiceContext.PushModel(model);
            await barnacleHook.BeforeBarnacle(choiceContext, summoner);
            model.InvokeExecutionFinished();
            choiceContext.PopModel(model);
        }
    }
    
    public static async Task AfterCardSubmerged(
        ICombatState combatState,
        PlayerChoiceContext choiceContext,
        CardModel card)
    {
        foreach (var model in Hook.IterateCombatHookListeners(combatState))
        {
            if(model is not IAfterSubmergeHook submergeModel)
                continue;
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
            if(model is not IAfterAbyssalHook abyssalModel)
                continue;
            choiceContext.PushModel(model);
            await abyssalModel.BeforeCardShuffled(choiceContext, card);
            model.InvokeExecutionFinished();
            choiceContext.PopModel(model);
        }
    }
    
    public static async Task OnCardSunken(
        ICombatState combatState,
        PlayerChoiceContext choiceContext,
        CardModel card)
    {
        foreach (var model in Hook.IterateCombatHookListeners(combatState))
        {
            if(model is not IAfterSunkenHook sunkenHook)
                continue;
            choiceContext.PushModel(model);
            await sunkenHook.AfterCardSunken(choiceContext, card);
            model.InvokeExecutionFinished();
            choiceContext.PopModel(model);
        }
    }
    
    public static int ModifyAbyssalAmount(
        ICombatState combatState,
        CardModel card,
        int playCount,
        out List<IModifyAbyssalHook> modifyingModels)
    {
        modifyingModels = [];
        var playCount1 = playCount;
        foreach (var combatHookListener in Hook.IterateCombatHookListeners(combatState))
        {
            if(combatHookListener is not IModifyAbyssalHook sunkenHook)
                continue;
            var num = playCount1;
            playCount1 = sunkenHook.ModifyAbyssalAmount(card, playCount1);
            if (playCount1 != num)
                modifyingModels.Add(sunkenHook);
        }
        return playCount1;
    }
    
    public static async Task AfterModifyingAbyssalAmount(
        ICombatState combatState,
        CardModel card,
        List<IModifyAbyssalHook> modifyingModels)
    {
        foreach (var combatHookListener in Hook.IterateCombatHookListeners(combatState))
        {
            if (combatHookListener is not IModifyAbyssalHook modifier || !modifyingModels.Contains(modifier))
                continue;
            await modifier.AfterModifyingAbyssalAmount(card);
            combatHookListener.InvokeExecutionFinished();
        }
    }
    
    public static int ModifySunkenAmount(
        ICombatState combatState,
        CardModel card,
        int playCount,
        out List<IModifySunkenHook> modifyingModels)
    {
        modifyingModels = [];
        var playCount1 = playCount;
        foreach (var combatHookListener in Hook.IterateCombatHookListeners(combatState))
        {
            if(combatHookListener is not IModifySunkenHook sunkenHook)
                continue;
            var num = playCount1;
            playCount1 = sunkenHook.ModifySunkenAmount(card, playCount1);
            if (playCount1 != num)
                modifyingModels.Add(sunkenHook);
        }
        return playCount1;
    }
    
    public static async Task AfterModifyingSunkenAmount(
        ICombatState combatState,
        CardModel card,
        List<IModifySunkenHook> modifyingModels)
    {
        foreach (var combatHookListener in Hook.IterateCombatHookListeners(combatState))
        {
            if (combatHookListener is not IModifySunkenHook modifier || !modifyingModels.Contains(modifier))
                continue;
            await modifier.AfterModifyingSunkenAmount(card);
            combatHookListener.InvokeExecutionFinished();
        }
    }
}
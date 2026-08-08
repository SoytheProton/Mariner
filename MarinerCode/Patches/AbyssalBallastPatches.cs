using System.Reflection;
using System.Reflection.Emit;
using BaseLib.Utils.Patching;
using HarmonyLib;
using Mariner.MarinerCode.Cards;
using Mariner.MarinerCode.Commands;
using Mariner.MarinerCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Mariner.MarinerCode.Patches;

public class AbyssalBallastPatches
{
    [HarmonyPatch(typeof(CardPileCmd), nameof(CardPileCmd.Shuffle), MethodType.Async)]
    public class BallastPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codeMatcher = new CodeMatcher(instructions);

            var ballast = AccessTools.Method(typeof(BallastPatch), nameof(BallastPileChanger));

            codeMatcher.MatchStartForward(
                    new CodeMatch(OpCodes.Ldc_I4_3),
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Ldfld),
                    new CodeMatch(OpCodes.Call),
                    new CodeMatch(OpCodes.Callvirt),
                    new CodeMatch(OpCodes.Call),
                    new CodeMatch(OpCodes.Stloc_2)
                )
                .ThrowIfInvalid("Couldn't find list for BallastPatch")
                .Advance(7)
                .InsertAndAdvance(
                    new CodeInstruction(OpCodes.Ldloc_2),
                    new CodeInstruction(OpCodes.Call, ballast),
                    new CodeInstruction(OpCodes.Stloc_2));
            
            return codeMatcher.InstructionEnumeration();
        }

        private static List<CardModel> BallastPileChanger(List<CardModel> cards)
        {
            return cards.Where(card => !card.Keywords.Contains(MarinerCardKeywords.Ballast)).ToList();
        }
    }
    
    [HarmonyPatch(typeof(CardPileCmd), nameof(CardPileCmd.CheckIfDrawIsPossibleAndShowThoughtBubbleIfNot))]
    public class BallastPatchPartTwo
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codeMatcher = new CodeMatcher(instructions);

            var ballast = AccessTools.Method(typeof(BallastPatchPartTwo), nameof(Subtraction));

            codeMatcher.MatchStartForward(
                    new CodeMatch(OpCodes.Add))
                .ThrowIfInvalid("Couldn't find list for BallastPatch")
                .Advance()
                .InsertAndAdvance(
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, ballast),
                    new CodeInstruction(OpCodes.Sub));
            
            return codeMatcher.InstructionEnumeration();
        }

        private static int Subtraction(Player player)
        {
            return PileType.Discard.GetPile(player).Cards.Count(c => c.Keywords.Contains(MarinerCardKeywords.Ballast));
        }
    }
    
    [HarmonyPatch(typeof(CardPileCmd), nameof(CardPileCmd.Shuffle), MethodType.Async)]
    public class AbyssalPatch
    {
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator, MethodBase original)
        {
            return AsyncMethodCall.Create(generator, instructions, original,
                AccessTools.Method(typeof(AbyssalPatch), nameof(AbyssalTask)), beforeState: original);
        }
        
        private static async Task AbyssalTask(PlayerChoiceContext choiceContext, Player player)
        {
            foreach (var card in PileType.Discard.GetPile(player).Cards.ToList())
            {
                if(card is not IAbyssalCard) 
                    continue;
                await AbyssalWrapper(choiceContext, card);
                await MarinerHook.BeforeCardShuffled(player.Creature.CombatState, choiceContext, card);
            }
        }
    }
    
    public static async Task AbyssalWrapper(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (card is not IAbyssalCard abyssalCard)
        {
            MarinerMainFile.Logger.Error("Why are we using AbyssalWrapper for non-Abyssal cards?");
            return;
        }

        await CardPileCmd.Add(card, PileType.Play);
        
        if(card.CombatState == null) 
            return;
        var playCount = await card.GeneratePlayCount(card.CombatState, null);
        playCount = MarinerHook.ModifyAbyssalAmount(card.CombatState, card, playCount, out var list);
        await MarinerHook.AfterModifyingAbyssalAmount(card.CombatState, card, list);
        for (var i = 0; i < playCount; ++i)
            await abyssalCard.BeforeShuffled(choiceContext);
        
        if (LocalContext.IsMe(card.Owner))
            await Cmd.CustomScaledWait(0.1f, 0.2f);
        await abyssalCard.BeforeShuffled(choiceContext);
        await CardPileCmd.Add(card, card.Keywords.Contains(MarinerCardKeywords.Ballast) ? PileType.Discard.GetPile(card.Owner) : PileType.Draw.GetPile(card.Owner));
    }
}
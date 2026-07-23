using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using BaseLib.Utils;
using BaseLib.Utils.Patching;
using Godot;
using HarmonyLib;
using Mariner.MarinerCode.Cards;
using Mariner.MarinerCode.Commands;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Label = System.Reflection.Emit.Label;

namespace Mariner.MarinerCode.Patches;

public class AbyssalBallastPatches
{
    [HarmonyPatch(typeof(CardPileCmd), nameof(CardPileCmd.Shuffle), MethodType.Async)]
    public class BallastPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var codeMatcher = new CodeMatcher(instructions, generator);

            var ballast = AccessTools.Method(typeof(BallastPatch), nameof(BallastPileChanger));

            codeMatcher.MatchStartForward(
                    new CodeMatch(OpCodes.Ldloc_S),
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Ldfld),
                    new CodeMatch(OpCodes.Ldc_I4_1),
                    new CodeMatch(OpCodes.Ldnull),
                    new CodeMatch(OpCodes.Ldc_I4_0)
                )
                .ThrowIfInvalid("Couldn't find CardPileCmd.Add for BallastPatch")
                .Advance()
                .InsertAndAdvance(
                    new CodeInstruction(OpCodes.Dup))
                .Advance(2)
                .InsertAndAdvance(
                    new CodeInstruction(OpCodes.Call, ballast));
            
            return codeMatcher.InstructionEnumeration();
        }

        private static CardPile BallastPileChanger(CardModel card, CardPile pile)
        {
            if(!card.Keywords.Contains(MarinerCardKeywords.Ballast))
                return pile;
            return PileType.Discard.GetPile(card.Owner);
        }
    }
    
    [HarmonyDebug]
    [HarmonyPatch(typeof(CardPileCmd), nameof(CardPileCmd.Shuffle), MethodType.Async)]
    public class AbyssalPatch
    {
        private static readonly SpireField<PlayerCombatState, CardModel> CardArgHolder = new(_ => null);
        
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator, MethodBase original)
        {
            MethodBase methodBase = AccessTools.Method(typeof(CardPileCmd), nameof(CardPileCmd.Add), [typeof(CardModel), typeof(CardPile), typeof(CardPilePosition), typeof(AbstractModel), typeof(bool)]);
            var asyncInstructions = AsyncMethodCall.Create(generator, instructions, original,
                AccessTools.Method(typeof(AbyssalPatch), nameof(AbyssalTask)), beforeState: methodBase);
            
            var codeMatcher = new CodeMatcher(asyncInstructions, generator);

            var getCurrent = AccessTools.Method(typeof(List<CardModel>.Enumerator), "get_Current");
            
            var stateMachineType = AccessTools.Method(typeof(CardPileCmd), nameof(CardPileCmd.Shuffle))
                .GetCustomAttribute<AsyncStateMachineAttribute>().StateMachineType;

            var cardMethod = AccessTools.Method(typeof(AbyssalPatch), nameof(SetCard));
                
            var bullshit = stateMachineType.GetField("<>7__wrap6", // Incase this breaks, do it more "responsibly" by adding a label where it goes to the card correctly and sets it. Basically br -> card set br -> back, when it goes to card set br past it. 
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            var label = generator.DefineLabel();
            var labeledInstruction = new CodeInstruction(OpCodes.Ldarg_0);
            labeledInstruction.labels.Add(label);

            codeMatcher.MatchStartForward(
                    new CodeMatch(OpCodes.Ldloc_0),
                    new CodeMatch(OpCodes.Ldc_I4_1),
                    new CodeMatch(OpCodes.Beq),
                    new CodeMatch(OpCodes.Br)
                )
                .ThrowIfInvalid("Couldn't find AsyncMethodCall.Create for AbyssalPatch")
                .Advance(4)
                .InsertAndAdvance(
                    labeledInstruction,
                    new CodeInstruction(OpCodes.Ldflda, bullshit),
                    new CodeInstruction(OpCodes.Call, getCurrent),
                    new CodeInstruction(OpCodes.Stloc_S, (byte)4),
                    new CodeInstruction(OpCodes.Ldloc_S, (byte)4),
                    new CodeInstruction(OpCodes.Call, cardMethod));

            codeMatcher.MatchStartForward(
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Ldflda),
                    new CodeMatch(OpCodes.Call),
                    new CodeMatch(OpCodes.Brtrue),
                    new CodeMatch(OpCodes.Leave_S)
                )
                .ThrowIfInvalid("Couldn't find foreach (CardModel card in list) for AbyssalPatch")
                .Advance(3)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Brtrue, label),
                    new CodeInstruction(OpCodes.Ldc_I4_0));
            
            return codeMatcher.InstructionEnumeration();
        }
        
        private static async Task AbyssalTask(PlayerChoiceContext choiceContext, Player player) 
        {
            MainFile.Logger.Info("ran");
            await MarinerHook.BeforeCardShuffled(player.Creature.CombatState, choiceContext, CardArgHolder.Get(player.PlayerCombatState)); 
        }
        
        private static void SetCard(CardModel card)
        {
            MainFile.Logger.Info("Card: " + card.Title);
            CardArgHolder.Set(card.Owner.PlayerCombatState, card);
        }
    }
    
    /* IGNORE THIS.

        var abyssalHook = AccessTools.Method(typeof(MarinerHook), nameof(MarinerHook.BeforeCardShuffled));
        var arg = AccessTools.Method(typeof(AbyssalPatch), nameof(RandomBullshitGo));

        if (methodBase == null)
        {
            throw new Exception("AHHHHHHHHHHHHHHHHHHHHHHHHHHH");
        }

        var async = AsyncMethodCall.Create(generator, instructions, original, arg, beforeState: methodBase);

        var stateMachineType = AccessTools.Method(typeof(CardPileCmd), nameof(CardPileCmd.Shuffle))
            .GetCustomAttribute<AsyncStateMachineAttribute>().StateMachineType;

        var player = stateMachineType.GetField("player",
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

        var choiceContext = stateMachineType.GetField("choiceContext",
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);



    }

    private static async Task RandomBullshitGo(Player player, PlayerChoiceContext choiceContext, CardModel card)
    {
        try
        {
            await MarinerHook.BeforeCardShuffled(player.Creature.CombatState, choiceContext, card);
            MainFile.Logger.Info("It ran!!!!!!!!!!!!!!!!!!");
        }
        catch (Exception e)
        {
            MainFile.Logger.Warn($"Async fucked up!!! {e}");
        }
    }*/
}
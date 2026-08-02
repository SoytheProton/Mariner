using System.Reflection;
using System.Reflection.Emit;
using BaseLib.Utils.Patching;
using HarmonyLib;
using Mariner.MarinerCode.Commands;
using Mariner.MarinerCode.Interfaces;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Orbs;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;

namespace Mariner.MarinerCode.Patches;

public class SunkenPatch
{
    [HarmonyPatch(typeof(CombatManager), nameof(CombatManager.DoTurnEnd), MethodType.Async)]
    public class EtherealSunkenPatch
    {
        // Patch made to avoid Sunken cards Exhausting twice if they have Ethereal.
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codeMatcher = new CodeMatcher(instructions);

            var sunken = AccessTools.Method(typeof(EtherealSunkenPatch), nameof(SunkenCardRemover));

            codeMatcher.MatchStartForward(
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Ldloc_3),
                    new CodeMatch(OpCodes.Callvirt),
                    new CodeMatch(OpCodes.Stfld)
                )
                .ThrowIfInvalid("Couldn't find Ethereal list for EtherealSunkenPatch")
                .Advance()
                .InsertAndAdvance(
                    new CodeInstruction(OpCodes.Pop),
                    new CodeInstruction(OpCodes.Ldloc_3),
                    new CodeInstruction(OpCodes.Call, sunken),
                    new CodeInstruction(OpCodes.Stloc_3),
                    new CodeInstruction(OpCodes.Ldarg_0));
            
            return codeMatcher.InstructionEnumeration();
        }

        private static List<CardModel> SunkenCardRemover(List<CardModel> cards)
        {
            return cards.Where(card => card is not ISunkenCard).ToList();
        }
    }
    
    [HarmonyPatch(typeof(CombatManager), nameof(CombatManager.DoTurnEnd), MethodType.Async)]
    public class SunkenAsyncPatch
    {
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator, MethodBase original)
        {
            MethodBase orb = AccessTools.Method(typeof(OrbQueue), nameof(OrbQueue.BeforeTurnEnd));
            return AsyncMethodCall.Create(generator, instructions, original,
                AccessTools.Method(typeof(SunkenAsyncPatch), nameof(SunkenTask)), afterState: orb);
        }
        
        private static async Task SunkenTask(Player player, PlayerChoiceContext choiceContext)
        {
            foreach (var card in PileType.Hand.GetPile(player).Cards.ToList())
            {
                if(card is not ISunkenCard) 
                    continue;
                await OnSunkenWrapper(choiceContext, card);
                await MarinerHook.OnCardSunken(player.Creature.CombatState, choiceContext, card);
            }
        }
    }
    
    public static async Task OnSunkenWrapper(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (card is not ISunkenCard sunkenCard)
        {
            MarinerMainFile.Logger.Error("Why are we using OnSunkenWrapper for non-Sunken cards?");
            return;
        }
        await CardPileCmd.Add(card, PileType.Play);
        if (LocalContext.IsMe(card.Owner))
            await Cmd.CustomScaledWait(0.3f, 0.6f);
        await sunkenCard.OnSunken(choiceContext);
        if (card.Keywords.Contains(CardKeyword.Ethereal))
        {
            await CardCmd.Exhaust(choiceContext, card, true);
        }
        else if (card.Keywords.Contains(CardKeyword.Retain) || !Hook.ShouldFlush(card.CombatState, card.Owner))
        {
            await CardPileCmd.Add(card, PileType.Hand.GetPile(card.Owner));
        }
        else
        {
            await CardPileCmd.Add(card, PileType.Discard.GetPile(card.Owner));
        }
    }
}
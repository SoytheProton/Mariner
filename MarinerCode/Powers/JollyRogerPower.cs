using Mariner.MarinerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace Mariner.MarinerCode.Powers;

public class JollyRogerPower() : MarinerPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterCombatEnd(CombatRoom room)
    {
        List<Reward> list = new List<Reward>();
        CardCreationOptions options = CardCreationOptions.ForNonCombatWithUniformOdds([Owner.Player.Character.CardPool], c => c.Rarity == CardRarity.Rare).WithFlags(CardCreationFlags.NoRarityModification);
            
        for (int i = 0; i < Amount; i++)
        {
            list.Add(new CardReward(options, 1, Owner.Player));
        }

        foreach (Reward item in list)
        {
            room.AddExtraReward(Owner.Player, item);   
        }
        // await RewardsCmd.OfferCustom(Owner.Player, list);
    }
}
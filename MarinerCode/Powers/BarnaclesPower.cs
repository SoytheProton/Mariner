using Mariner.MarinerCode.Cards;
using Mariner.MarinerCode.Commands;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace Mariner.MarinerCode.Powers;

public sealed class BarnaclesPower : MarinerPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(MarinerStaticHovertip.Barnacle)];

    public override async Task AfterEnergyReset(Player player)
    {
        if (player != Owner.Player)
            return;
        await BarnacleCmd.Spawn(new ThrowingPlayerChoiceContext(), Owner.Player, this);
        Flash();
        await PowerCmd.Decrement(this);
    }
}
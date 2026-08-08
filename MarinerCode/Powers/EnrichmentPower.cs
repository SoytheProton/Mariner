using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;

namespace Mariner.MarinerCode.Powers;

public sealed class EnrichmentPower : MarinerPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar("Owner")];

    private Player? _playerOwner;
    
    public Player? PlayerOwner
    {
        get => _playerOwner;
        set
        {
            AssertMutable();
            ((StringVar)DynamicVars["Owner"]).StringValue = PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, value.NetId);
            _playerOwner = value;
        }
    }
    
    public override async Task AfterDeath(
        PlayerChoiceContext choiceContext,
        Creature creature,
        bool wasRemovalPrevented,
        float deathAnimLength)
    {
        if (wasRemovalPrevented || creature != Owner)
            return;
        var player = PlayerOwner;
        if (player == null)
            return;
        await CardPileCmd.Draw(choiceContext, Amount, player);
        
    }
}
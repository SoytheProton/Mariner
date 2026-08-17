using BaseLib.Abstracts;
using Mariner.MarinerCode.Powers;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mariner.MarinerCode.Monsters;

// TODO: Patch this in Creature.cs: public void ScaleMonsterHpForMultiplayer(EncounterModel? encounter, int playerCount, int actIndex)
// TODO: Look into can hit and whatnot. Also look into PowerCmd
public class Barnacle : CustomMonsterModel
{
    private Player _playerOwner;
    
    public Player PlayerOwner
    {
        get => _playerOwner;
        set
        {
            AssertMutable();
            _playerOwner = value;
        }
    }
    
    
    protected override string VisualsPath => "res://scenes/creature_visuals/sludge_spinner.tscn";
    
    public override async Task AfterAddedToRoom()
    {
        await PowerCmd.Apply<MinionPower>(new ThrowingPlayerChoiceContext(), Creature, 1M, Creature, null);
    }
    
    public override int MinInitialHp => 5;
    public override int MaxInitialHp => MinInitialHp;

    public override DamageSfxType TakeDamageSfxType => DamageSfxType.Slime;
    
    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var states = new List<MonsterState>();
        var initialState = new MoveState("FILTER", Filter, new UnknownIntent());
        initialState.FollowUpState = initialState;
        states.Add(initialState);
        return new MonsterMoveStateMachine(states, initialState);
    }

    private static Task Filter(IReadOnlyList<Creature> targets)
    {
        return Task.CompletedTask;
    }
    
    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (target != Creature)
            return 1M;
        if(dealer != null && dealer.Player != PlayerOwner)
            return 0M;
        return 1M;
    }
}
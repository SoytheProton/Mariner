using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace Mariner.MarinerCode.Monsters;

// TODO: Patch this in Creature.cs: public void ScaleMonsterHpForMultiplayer(EncounterModel? encounter, int playerCount, int actIndex)
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
        var initialState = new MoveState("GROWTH", Growth, new BuffIntent());
        initialState.FollowUpState = initialState;
        states.Add(initialState);
        return new MonsterMoveStateMachine(states, initialState);
    }

    private async Task Growth(IReadOnlyList<Creature> targets)
    {
        await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Creature, 1, Creature, null);
    }
}
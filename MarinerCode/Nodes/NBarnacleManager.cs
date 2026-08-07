using BaseLib.Utils;
using Godot;
using Mariner.MarinerCode.Commands;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace Mariner.MarinerCode.Nodes;

// I have no sweet clue what I'm doing here tbh.
[GlobalClass]
public partial class NBarnacleManager : Control
{
    public static SpireField<NCreature, NBarnacleManager> NBarnacleManagerField = new (_ => null);
    private readonly List<NCreature> _barnacles = [];

    private NCreature? _creatureNode;

    private Tween? _curTween;
    
    private bool IsLocal { get; set; }

    private Player Player => _creatureNode?.Entity.Player ?? throw new Exception("RuneManager does not have a Player");

    private const float Radius = 400f;

    private const float FanAngle = 120f;

    private const float AngleOffset = 20f;

    private const float TweenFadeDuration = 0.7f;
    
    private static readonly Vector2 CenterOffset = new(0, 0);
    
    private static string ScenePath => "res://Mariner/scenes/barnacles/barnacle_manager.tscn";
    
    public static NBarnacleManager Create(NCreature creature, bool isLocal)
    {
        if (creature.Entity.Player == null)
            throw new InvalidOperationException("NBarnacleManager can only be applied to player creatures");
        var nBarnacleManager = PreloadManager.Cache.GetScene(ScenePath).Instantiate<NBarnacleManager>();
        nBarnacleManager._creatureNode = creature;
        nBarnacleManager.IsLocal = isLocal;
        NBarnacleManagerField.Set(creature, nBarnacleManager);
        return nBarnacleManager;
    }
    
    public override void _Ready()
    {
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        CombatManager.Instance.StateTracker.CombatStateChanged += OnCombatStateChanged;
        CombatManager.Instance.CombatSetUp += OnCombatSetup;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        CombatManager.Instance.StateTracker.CombatStateChanged -= OnCombatStateChanged;
        CombatManager.Instance.CombatSetUp -= OnCombatSetup;
    }

    private void OnCombatSetup(CombatState _)
    {
    }

    private void OnCombatStateChanged(CombatState state)
    {
    }

    private void TweenLayout()
    {
        var capacity = _barnacles.Count;
        if (capacity == 0)
            return;
        _curTween?.Kill();
        _curTween = CreateTween().SetParallel();
        for (var index = 0; index < capacity; ++index)
        {
            var position = GetPosition(index);
            _curTween.TweenProperty(_barnacles[index], "global_position", _creatureNode.GlobalPosition + position, TweenFadeDuration).SetEase(Tween.EaseType.InOut)
                .SetTrans(Tween.TransitionType.Sine);
        }
    }
    
    public void AddBarnacle(NCreature node)
    {
        if(node == null)
            return;
        _barnacles.Add(node);
        // node.GlobalPosition = _creatureNode?.GlobalPosition ?? Vector2.Zero;
        node.Position = _creatureNode?.Position ?? Vector2.Zero;
        TweenLayout();
    }
    
    public void RemoveBarnacle(NCreature node)
    {
        if(node == null || !_barnacles.Contains(node))
            return;
        _barnacles.Remove(node);
        if (node.HasFocus())
            _creatureNode?.Hitbox.TryGrabFocus();
        TweenLayout();
    }
    
    private Vector2 GetPosition(int index)
    {
        var radius = Radius;
        if (!IsLocal) radius *= 0.75f;

        const float angleStep = FanAngle / (BarnacleCmd.BarnacleCap - 1);
        var angle = float.DegreesToRadians(-angleStep * index - AngleOffset); // neg angle is counter-clockwise
        return new Vector2(radius, 0f).Rotated(angle) + CenterOffset;
    }
    
    public void Clear()
    {
        _curTween?.Kill();
        if (_barnacles.Count == 0)
            return;
        _curTween = CreateTween();
        foreach (var barn in _barnacles)
        {
            _curTween.Parallel().TweenProperty(barn, (NodePath) "global_position", _creatureNode.GlobalPosition +  Vector2.Zero, 1.0).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Sine);
            _curTween.Parallel().TweenProperty(barn, (NodePath) "modulate:a", 0, 0.25);
        }
        foreach (var barn in _barnacles)
            _curTween.Chain().TweenCallback(Callable.From(barn.QueueFreeSafely));
        _barnacles.Clear();
    }
}
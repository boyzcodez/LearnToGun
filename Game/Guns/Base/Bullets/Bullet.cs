using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class Bullet : Area2D
{
    #region Fields & Properties

    [Export] public Behavior[] Behaviors = [];
    [Export] public AnimatedSprite2D Animation;
    [Export] public Sprite2D shadow;
    //[Export] public PackedScene impactDust;

    private BulletPool _pool;
    public DamageData DamageData;
    public CollisionShape2D collisionShape;

    public float Speed { get; private set; } = 80f;
    public Vector2 Direction { get; set; }
    public float rotation { get; set; }
    public string Key { get; private set; }
    public bool Active { get; set; } = false;
    public bool hasHit = false;
    public bool doesRotate = false;
    public bool _pendingActivation = false;

    public float _timer;

    #endregion

    #region Godot Lifecycle

    public override void _Ready()
    {
        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        SetPhysicsProcess(false);
        Hide();

        Animation.AnimationFinished += Hide;
        BodyEntered += WallHit;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_pendingActivation)
        {
            _pendingActivation = false;
            Monitoring = true;
            Monitorable = true;
            Active = true;
            return; // skip first tick for stable state
        }

        if (!Active) return;

        foreach (var behavior in Behaviors)
        {
            behavior.Update(this, delta);
        }

        _timer += (float)delta;
        if (_timer >= 4f) Deactivate();

        if (Monitoring)
        {
            if (GetOverlappingAreas().Count > 0)
            {
                OnHit();
            }
        }
        
    }

    #endregion

    #region Initialization & Pooling

    public void Initialize()
    {
        foreach (var behavior in Behaviors)
        {
            behavior.Initialize(this);
        }
    }

    public void Init(DamageData damageData, string type, float newSpeed, bool rotate, BulletPool newPool)
    {
        DamageData = damageData;
        Key = type;
        Speed = newSpeed;
        _pool = newPool;
        doesRotate = rotate;
    }

    public void Activate(float newRotation)
    {
        if (Active || _pendingActivation) return;

        rotation = newRotation;
        if (doesRotate)
        {
            Animation.Rotation = newRotation;
            collisionShape.Rotation = newRotation;
            if (shadow != null) shadow.Rotation = newRotation;
        }
        Initialize();
        _timer = 0f;
        hasHit = false;

        // Defer enabling physics process for safety
        CallDeferred("set_physics_process", true);
        _pendingActivation = true;

        Show();
        Animation?.Play("default");
    }

    public void Deactivate()
    {
        if (!Active && !_pendingActivation) return;

        Active = false;
        _pendingActivation = false;
        CallDeferred("set_physics_process", false);

        SetDeferred("monitoring", false);
        SetDeferred("monitorable", false);

        _pool.ReturnBullet(Key, this);

        Animation?.Play("hit");
    }
    private void WallHit(Node body)
    {
        Deactivate();
    }

    #endregion

    #region Behaviors

    public void OnHit()
    {
        foreach (var behavior in Behaviors)
        {
            behavior.OnHit(this);
        }
    }

    #endregion
}

using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class Bullet : Area2D
{
    #region Fields & Properties

    [Export] public Behavior[] Behaviors = [];
    [Export] public AnimatedSprite2D Animation;

    private BulletPool _pool;
    public DamageData DamageData;

    public float Speed { get; private set; } = 80f;
    public Vector2 Direction { get; set; }
    public float rotation { get; set; }
    public string Key { get; private set; }
    public bool Active { get; private set; } = false;
    public bool hasHit = false;

    public float _timer;

    #endregion

    #region Godot Lifecycle

    public override void _Ready()
    {
        SetPhysicsProcess(false);
        SetDeferred("monitoring", false);
        SetDeferred("monitorable", false);
        Hide();

        Animation.AnimationFinished += Hide;
        BodyEntered += WallHit;

    }

    public override void _PhysicsProcess(double delta)
    {
        if (!Active) return;

        foreach (var behavior in Behaviors)
        {
            behavior.Update(this, delta);
        }

        
        // var overlaps = GetOverlappingAreas();
            
        if (GetOverlappingAreas().Count > 0)
        {
            hasHit = true;
            OnHit();
        }
        // if (GetOverlappingBodies().Count > 0 && !hasHit)
        // {
        //     Deactivate();
        // }

        _timer += (float)delta;
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

    public void Init(DamageData damageData, string type, float newSpeed, BulletPool newPool)
    {
        DamageData = damageData;
        Key = type;
        Speed = newSpeed;
        _pool = newPool;
    }

    public void Activate(float newRotation)
    {
        if (Active) return;
        
        rotation = newRotation;

        Initialize();

        _timer = 0f;
        Active = true;
        hasHit = false;

        Show();
        SetPhysicsProcess(true);
        SetDeferred("monitoring", true);
        SetDeferred("monitorable", true);

        // Optional: Play default animation
        Animation?.Play("default");
    }

    public void Deactivate()
    {
        if (!Active) return;
        
        Active = false;
        //Hurtboxes.Clear();

        //Hide();
        SetPhysicsProcess(false);
        SetDeferred("monitoring", false);
        SetDeferred("monitorable", false);

        _pool.ReturnBullet(Key, this);

        // Optional: Play hit animation
        Animation?.Play("hit");
    }
    private void WallHit(Node body)
    {
        if (hasHit) return;
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

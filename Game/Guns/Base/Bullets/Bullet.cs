using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class Bullet : Area2D
{
    #region Fields & Properties

    [Export] public Behavior[] Behaviors = [];
    [Export] public AnimatedSprite2D Animation;

    private RayCast2D raycast;
    private BulletPool _pool;
    public DamageData DamageData;

    public List<Hurtbox> Hurtboxes { get; private set; } = new();
    public float Speed { get; private set; } = 80f;
    public Vector2 Direction { get; set; }
    public float rotation { get; set; }
    public string Key { get; private set; }
    public bool Active { get; private set; } = false;
    public bool hasHit = false;

    public float _timer;
    private int _currentValue = 0;

    #endregion

    #region Godot Lifecycle

    public override void _Ready()
    {
        raycast = GetNode<RayCast2D>("RayCast2D");
        raycast.Position = new Vector2(-5, 0);

        SetPhysicsProcess(false);
        SetDeferred("monitoring", false);
        SetDeferred("monitorable", false);
        Hide();
    }

    public override void _Process(double delta)
    {
        foreach (var behavior in Behaviors)
        {
            behavior.Update(this, delta);
        }
        if (Hurtboxes.Count > 0 && !hasHit)
        {
            hasHit = true;
            OnHit();
        }
        // if (raycast.IsColliding() && Active)
        // {
        //     GlobalPosition += Direction * 5;
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
        rotation = newRotation;
        raycast.Rotation = newRotation;

        Initialize();

        _timer = 0f;
        Active = true;
        hasHit = false;

        Show();
        SetProcess(true);
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
        SetProcess(false);
        SetDeferred("monitoring", false);
        SetDeferred("monitorable", false);

        _pool.ReturnBullet(Key, this);

        // Optional: Play hit animation
        Animation?.Play("hit");
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

    #region Signals

    private void _on_area_entered(Node body)
    {
        if (body is Hurtbox hurtbox && !hurtbox.immune)
        {
            Hurtboxes.Add(hurtbox);
        }
    }

    private void _on_area_exited(Node body)
    {
        if (body is Hurtbox hurtbox && Hurtboxes.Contains(hurtbox))
        {
            Hurtboxes.Remove(hurtbox);
        }
    }

    #endregion
}

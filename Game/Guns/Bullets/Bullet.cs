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

    public List<Hurtbox> Hurtboxes { get; private set; } = new();
    public float Speed { get; private set; } = 80f;
    public Vector2 Direction { get; private set; }
    public string Key { get; private set; }
    public bool Active { get; private set; } = false;

    public float _timer;
    private int _currentValue = 0;

    #endregion

    #region Godot Lifecycle

    public override void _Ready()
    {
        SetPhysicsProcess(false);
        SetDeferred("monitoring", false);
        SetDeferred("monitorable", false);
        Hide();
    }

    public override void _PhysicsProcess(double delta)
    {
        foreach (var behavior in Behaviors)
        {
            behavior.Update(this, delta);
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

    public void Init(DamageData damageData, string type, float newSpeed, BulletPool newPool)
    {
        DamageData = damageData;
        Key = type;
        Speed = newSpeed;
        _pool = newPool;
    }

    public void Activate(Vector2 newDirection)
    {
        _timer = 0f;
        Direction = newDirection;
        Active = true;

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
            OnHit();
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

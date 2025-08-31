using Godot;

[GlobalClass]
public partial class Bullet : Area2D
{
    [Export] public Behavior[] behaviors;
    [Export] public float speed = 110f;
    private DamageData _damageData;
    public Vector2 direction;
    public string key;
    public bool active;
    public float timer;

    // Behavior stuff
    public void Initialize()
    {
        foreach (var behavior in behaviors)
        {
            behavior.Initialize(this);
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        foreach (var behavior in behaviors)
        {
            behavior.Update(this, delta);
        }
    }
    public void OnHit()
    {
        foreach (var behavior in behaviors)
        {
            behavior.OnHit(this);
        }
    }

    // Necissary stuff
    public override void _Ready()
    {
        Deactivate();
    }
    public void Init(DamageData damageData, string type)
    {
        _damageData = damageData;
        key = type;
    }
    public void Activate(Vector2 newDirection)
    {
        active = true;
        Initialize();
        direction = newDirection;
        SetPhysicsProcess(true);
    }
    public void Deactivate()
    {
        active = false;
        SetPhysicsProcess(false);
        var pool = GetTree().GetFirstNodeInGroup("BulletPool") as BulletPool;
        pool.ReturnBullet(key, this);
    }
    public void _on_area_entered(Node body)
    {
        if (active && body is Hurtbox hurtbox)
        {
            hurtbox.TakeDamage(_damageData, direction);
            OnHit();
        }
        
    }
}

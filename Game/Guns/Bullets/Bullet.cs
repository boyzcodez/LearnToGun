using Godot;

[GlobalClass]
public partial class Bullet : Area2D
{
    [Export] public Behavior[] behaviors;
    BulletPool pool;
    private DamageData _damageData;
    public float speed = 80f;
    public Vector2 direction;
    public Area2D area;
    public string key;
    public bool active = false;
    public float timer;
    public bool check = false;

    // Behavior stuff
    public void Initialize()
    {
        if (behaviors == null) return;
        foreach (var behavior in behaviors)
        {
            behavior.Initialize(this);
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        if (!active) return;

        if (behaviors == null) return;
        foreach (var behavior in behaviors)
        {
            behavior.Update(this, delta);
        }
    }
    public void OnHit()
    {
        if (behaviors == null) return;
        foreach (var behavior in behaviors)
        {
            behavior.OnHit(this);
        }
    }

    // Necissary stuff
    public void Init(DamageData damageData, string type, float newSpeed, BulletPool newPool)
    {
        _damageData = damageData;
        key = type;
        speed = newSpeed;
        pool = newPool;
    }
    public void Activate(Vector2 newDirection)
    {
        Initialize();
        direction = newDirection;
        active = true;
    }
    public void Deactivate()
    {
        active = false;
        pool.ReturnBullet(key, this);
    }
    public void _on_area_entered(Node body)
    {
        if (active && body is Hurtbox newHurtbox)
        {
            if (!newHurtbox.immune)
            {
                newHurtbox.TakeDamage(_damageData, direction);
                OnHit();
            }
        }

    }
}

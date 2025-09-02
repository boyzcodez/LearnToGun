using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

[GlobalClass]
public partial class Bullet : Area2D
{
    [Export] public Behavior[] behaviors;
    [Export] public AnimatedSprite2D animation;
    BulletPool pool;
    public DamageData _damageData;
    public List<Hurtbox> hurtboxes = new();
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
    public override void _Process(double delta)
    {
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
    public override void _Ready()
    {
        SetPhysicsProcess(false);
        SetDeferred("monitoring", false);
        SetDeferred("monitorable", false);
        Hide();
    }

    public void Init(DamageData damageData, string type, float newSpeed, BulletPool newPool)
    {
        _damageData = damageData;
        key = type;
        speed = newSpeed;
        pool = newPool;
    }
    public void Activate(Vector2 newDirection)
    {
        timer = 0f;
        Show();
        if (animation != null) animation.Play("default");
        direction = newDirection;

        active = true;
        SetPhysicsProcess(true);

        SetDeferred("monitoring", true);
        SetDeferred("monitorable", true);

        //await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        //Initialize();
    }
    public void Deactivate()
    {
        hurtboxes.Clear();
        active = false;
        SetPhysicsProcess(false);

        SetDeferred("monitoring", false);
        SetDeferred("monitorable", false);

        Hide();
        pool.ReturnBullet(key, this);
        
    }
    public void _on_area_entered(Node body)
    {
        if (body is Hurtbox hurtbox && !hurtbox.immune)
        {
            hurtboxes.Add(hurtbox);
            OnHit();
        }
    }
    public void _on_area_exited(Node body)
    {
        if (body is Hurtbox hurtbox && hurtboxes.Contains(hurtbox))
        {
            hurtboxes.Remove(hurtbox);
        }

    }
}

using Godot;

[GlobalClass]
public partial class Bullet : Area2D
{
    [Export] public float speed = 110f;
    public bool IsEnemy { get; private set; }
    private DamageData _damageData;
    private Vector2 direction;
    private bool check = false;
    private Node insideBody;
    private bool active;
    private string key;
    private float timer = 0f;

    public override void _Ready()
    {
        Deactivate();
    }

    public void Init(DamageData damageData, string type)
    {
        _damageData = damageData;
        key = type;
    }
    public override void _Process(double delta)
    {
        GlobalPosition += direction * speed * (float)delta;

        if (check) ReCheck();

        timer += (float)delta;
        if (timer >= 5f) Deactivate();
    }

    private void _on_area_entered(Node body)
    {
        if (body is Hurtbox health && health.immune == false)
        {
            Hit(health);
        }
        else
        {
            check = true;
            insideBody = body;
        }
    }
    private void _on_area_exited(Node body)
    {
        check = false;
    }

    private void ReCheck()
    {
        if (insideBody is Hurtbox health && health.immune == false)
        {
            Hit(health);
        }
    }

    private void Hit(Hurtbox body)
    {
        if (!active) return;
        body.TakeDamage(_damageData, direction);
        Deactivate();
    }

    public void Activate(Vector2 newDirection)
    {
        timer = 0f;
        direction = newDirection;
        active = true;
        SetProcess(true);
    }
    public void Deactivate()
    {
        active = false;
        SetProcess(false);
        var pool = GetTree().GetFirstNodeInGroup("BulletPool") as BulletPool;
        pool.ReturnBullet(key, this);
        timer = 0f;
    }
}

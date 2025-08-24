using Godot;

[GlobalClass]
public partial class Bullet : Area2D
{
    [Export] float speed = 50;
    private DamageData _damageData;
    private Vector2 direction;

    public void Init(DamageData damageData, Vector2 bulletDirection)
    {
        _damageData = damageData;
        direction = bulletDirection;
    }
    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition += direction * speed * (float)delta;
    }

    private void _on_area_entered(Node body)
    {
        if (body is Hurtbox health)
        {
            health.TakeDamage(_damageData, direction);
            QueueFree();
        }
    }
}

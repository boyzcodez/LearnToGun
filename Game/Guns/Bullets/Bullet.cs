using Godot;

[GlobalClass]
public partial class Bullet : Area2D
{
    [Export] float speed = 50;
    private DamageData _damageData;
    private Vector2 direction;
    private bool check = false;
    private Node insideBody;

    public void Init(DamageData damageData, Vector2 bulletDirection)
    {
        _damageData = damageData;
        direction = bulletDirection;
    }
    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition += direction * speed * (float)delta;

        if (check) ReCheck();
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
        body.TakeDamage(_damageData, direction);
        QueueFree();
    }
}

using Godot;
using System;
using System.Threading.Tasks;

[GlobalClass]
public partial class BasicBullet : Area2D
{
    private RayCast2D _ray;
    private RayCast2D ray => _ray ??= GetNode<RayCast2D>("RayCast2D");
    private AnimatedSprite2D _animation;
    private AnimatedSprite2D animation => _animation ??= GetNode<AnimatedSprite2D>("AnimatedSprite");
    [Export] private float speed = 30f;
    [Export] private DamageInfo damageInfo;
    public Vector2 direction;
    private bool hit = false;
    public virtual void Enter()
    {
        ray.TargetPosition = direction * 11f;
    }
    public virtual void Kill()
    {
        speed = 0f;
        animation.Play("kill");
    }
    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition += direction * speed * (float)delta;

        // if (ray.IsColliding() && hit == false)
        // {
        //     hit = true;
        //     Kill();
        // }
    }

    // hooked up on the engine side
    private void _on_area_entered(Hurtbox body)
    {
        body.Damage(damageInfo, direction);
        Kill();
    }
    // hooked up on the engine side
    private void _animation_finished()
    {
        QueueFree();
    }

}

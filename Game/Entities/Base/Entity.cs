using Godot;

[GlobalClass]
public partial class Entity : CharacterBody2D
{

    public Vector2 direction = Vector2.Zero;
    public float KnockbackTime = 0f;
    public Vector2 KnockbackVelocity;
    public bool Dead = false;

    public void Knockback(Vector2 direction, float force)
    {
        if (force <= 0) return;
        Vector2 Knockback = direction.Normalized() * force;

        if (Knockback == Vector2.Zero)
        {
            return;
        }
        else
        {
            KnockbackTime = 0.2f;
            KnockbackVelocity = Knockback;
        }
    }

    public virtual void Death()
    {
        EventBus.OnEnemyDied();
        Dead = true;
        QueueFree();
    }
    

    
}

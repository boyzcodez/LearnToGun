using Godot;
using System;

[GlobalClass]
public partial class Entity : CharacterBody2D
{
    public Vector2 direction = Vector2.Zero;
    public float KnockbackTime = 0f;
    public void Knockback(Vector2 direction, float force)
    {
        Vector2 Knockback = direction.Normalized() * force;

        if (Knockback == Vector2.Zero)
        {
            return;
        }
        else
        {
            KnockbackTime = 0.2f;
            Velocity = Knockback;
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        if (KnockbackTime > 0f)
        {
            KnockbackTime -= (float)delta;
            if (KnockbackTime <= 0f)
            {
                Velocity = Vector2.Zero; // Stop movement after knockback
            }
        }
        else
        {
            Velocity = direction;
        }

        MoveAndSlide();
    }
}

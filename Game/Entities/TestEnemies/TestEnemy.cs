using Godot;
using System;

public partial class TestEnemy : Entity
{
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

        MoveAndSlide();
    }

}

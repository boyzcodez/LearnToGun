using Godot;
using System;

[GlobalClass]
public partial class Entity : CharacterBody2D
{
    public float KnockbackTime = 0f;
    public void Knockback(Vector2 direction, float force)
    {
        if (KnockbackTime > 0f)
        {
            return;
        }
        else
        { 
        KnockbackTime = 0.3f;
        Vector2 Knockback = direction.Normalized() * force;
        Velocity = Knockback;
        }
    }
}

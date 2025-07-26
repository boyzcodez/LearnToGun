using Godot;
using System;

[GlobalClass]
public partial class Hurtbox : Area2D
{
    private Node2D DamageNumbers;
    [Export]
    private Health healthComponent;

    public override void _Ready()
    {
        DamageNumbers = GetNode<Node2D>("DamageNumbers");
    }
    public void Damage(int damage, Vector2 attacker_pos)
    {
        Vector2 knockbackDirection = attacker_pos.DirectionTo(GlobalPosition);

        if (healthComponent != null)
        {
            healthComponent.TakeDamage(damage, knockbackDirection, 200f);
            DamageNumbers.Call("DisplayNumber", damage);
        }
        else
        {
            GD.PrintErr("No health component attached to Hurtbox.");
        }
    }
}

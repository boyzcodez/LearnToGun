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
    public void Damage(DamageInfo damageInfo, Vector2 direction)
    {

        if (healthComponent != null)
        {
            healthComponent.TakeDamage(damageInfo, direction, 100f);
        }
        else
        {
            GD.PrintErr("No health component attached to Hurtbox in " + GetParent<Entity>().Name);
        }
    }
}

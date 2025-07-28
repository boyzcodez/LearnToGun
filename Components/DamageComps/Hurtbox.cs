using Godot;
using System;

[GlobalClass]
public partial class Hurtbox : Area2D
{
    [Export]
    private Health healthComponent;

    public void Damage(DamageInfo damageInfo, Vector2 direction = default)
    {

        if (healthComponent != null)
        {
            healthComponent.TakeDamage(damageInfo, direction);
        }
        else
        {
            GD.PrintErr("No health component attached to Hurtbox in " + GetParent<Entity>().Name);
        }
    }
}

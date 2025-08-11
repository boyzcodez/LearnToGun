using Godot;
using System;

[GlobalClass]
public partial class Hurtbox : Area2D
{
    public bool immune = false;
    [Export]
    private Health healthComponent;

    public async void Damage(DamageInfo damageInfo, Vector2 direction = default)
    {

        if (healthComponent != null && immune == false)
        {
            await healthComponent.TakeDamage(damageInfo, direction);
        }
    }
}

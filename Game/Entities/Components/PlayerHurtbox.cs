using Godot;
using System;

public partial class PlayerHurtbox : Hurtbox
{
    public override void OnInit()
    {
        EventBus.Health(currentHealth, maxHealth);
    }

    public override void OnHit()
    {
        EventBus.Health(currentHealth, maxHealth);
    }
    public override void ResetHealth()
    {
        immune = false;
        currentHealth = maxHealth;
        EventBus.Health(currentHealth, maxHealth);
    }


}

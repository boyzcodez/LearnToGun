using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class MultiHit : Behavior
{
    public override void Initialize(Bullet bullet)
    {
        
    }
    public override void Update(Bullet bullet, double delta)
    {
        bullet.timer += (float)delta;
        if (bullet.timer >= 0.1f) Trigger(bullet); 
    }
    public override void OnHit(Bullet bullet)
    {
    }
    public void Trigger(Bullet bullet)
    {
        foreach (var hurtbox in bullet.hurtboxes)
        {
            hurtbox.TakeDamage(bullet._damageData, bullet.direction);
        }
        bullet.Deactivate();
    }
}

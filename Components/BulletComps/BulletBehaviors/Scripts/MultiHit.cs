using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class MultiHit : Behavior
{
    public override void Initialize(Bullet bullet)
    {
        foreach (var hurtbox in bullet.hurtboxes)
        {
            hurtbox.TakeDamage(bullet._damageData, bullet.direction);
        }
        bullet.Deactivate();
    }
    public override void Update(Bullet bullet, double delta)
    {
    }
    public override void OnHit(Bullet bullet)
    {
    }
}

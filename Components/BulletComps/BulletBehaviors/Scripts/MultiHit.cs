using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class MultiHit : Behavior
{
    private List<Hurtbox> alreadyHit = new();
    public override void Initialize(Bullet bullet)
    {
    }
    public override void Update(Bullet bullet, double delta)
    {
    }
    public override void OnHit(Bullet bullet)
    {
        foreach (var hurtbox in bullet.hurtboxes)
        {
            hurtbox.TakeDamage(bullet._damageData, bullet.direction);
            //alreadyHit.Add(hurtbox);
        }
        // foreach (var hurtbox in alreadyHit)
        // {
        //     bullet.hurtboxes.Remove(hurtbox);
        // }
        //alreadyHit.Clear();
        bullet.Deactivate();
    }
}

using Godot;
using System;

[GlobalClass]
public partial class SingleHit : Behavior
{
    public override void Initialize(Bullet bullet)
    {
    }
    public override void Update(Bullet bullet, double delta)
    {
    }
    public override void OnHit(Bullet bullet)
    {
        bullet.hurtboxes[0].TakeDamage(bullet._damageData, bullet.direction);
        bullet.Deactivate();
    }
}

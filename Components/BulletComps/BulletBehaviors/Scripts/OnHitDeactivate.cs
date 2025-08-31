using Godot;
using System;

[GlobalClass]
public partial class OnHitDeactivate : Behavior
{
    public override void Initialize(Bullet bullet)
    {
    }
    public override void Update(Bullet bullet, double delta)
    {
    }
    public override void OnHit(Bullet bullet)
    {
        bullet.Deactivate();
    }
}

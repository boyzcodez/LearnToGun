using Godot;
using System;

[GlobalClass]
public partial class TowardsRotation : Behavior
{
    public override void Initialize(Bullet bullet)
    {
        bullet.Direction = Vector2.Right.Rotated(bullet.rotation);
    }
    public override void Update(Bullet bullet, double delta)
    {
    }
    public override void OnHit(Bullet bullet)
    {
    }
}

using Godot;
using System;

[GlobalClass]
public partial class PelletSpeedControl : Behavior
{
    [Export] float inRange = 0.9f;
    public override void Initialize(Bullet bullet)
    {
        bullet.Speed = bullet.Speed * (float)GD.RandRange(inRange, 1 + (1 - inRange));
    }
    public override void Update(Bullet bullet, double delta)
    {
    }
    public override void OnHit(Bullet bullet)
    {
    }
}

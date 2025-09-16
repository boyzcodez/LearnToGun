using Godot;
using System;

[GlobalClass]
public partial class TowardsMouse : Behavior
{
    public override void Initialize(Bullet bullet)
    {
        bullet.Direction = (bullet.GetGlobalMousePosition() - bullet.GlobalPosition).Normalized();
    }
    public override void Update(Bullet bullet, double delta)
    {
    }
    public override void OnHit(Bullet bullet)
    {
    }
}

using Godot;
using System;

public partial class Gun : Node2D
{
    public Hitbox hitbox;
    [Export] BaseGun gun;

    public override void _Ready()
    {
        hitbox = GetNode<Hitbox>("Hitbox");
        SetGun();
    }
    public void SetGun()
    {
        hitbox.damageInfo = gun.damageInfo;
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("attack"))
        {
            hitbox.ApplyDamage();
        }
    }
}

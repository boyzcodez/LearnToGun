using Godot;
using System;

public partial class Gun : Node2D
{
    public Hitbox hitbox;
    [Export] BaseGun gun;

    public override void _Ready()
    {
        hitbox = GetChild<Hitbox>(0);
        SetGun();
    }
    public void SetGun()
    {
        hitbox.damageInfo = gun.damageInfo;
    }
}

using Godot;
using System;

[GlobalClass]
public partial class BaseGun : Resource
{
    [Export] public DamageInfo damageInfo;
    [Export] int ammo = 10;
    [Export] int maxAmmo = 10;
    [Export] float fireRate = 0.5f;
    [Export] public float RangeX = 85f;
    [Export] public float RangeY = 40f;

}

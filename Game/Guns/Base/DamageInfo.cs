using Godot;
using System;

[GlobalClass]
public partial class DamageInfo : Resource
{
    [Export] public int damage = 10;
    [Export] public float knockbackForce = 100f;
    [Export] public string damageType = "Obliterate";
    [Export] public int typeDamage = 5;
}


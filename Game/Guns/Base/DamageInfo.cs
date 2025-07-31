using Godot;
using System;

[GlobalClass]
public partial class DamageInfo : Resource
{
    [Export] public int damage = 10;
    [Export] public float knockbackForce = 100f;
    [Export] public int repeatCount = 1;
    [Export(PropertyHint.Enum, "Obliterate,Cascade,Burn,None")]
    public string damageType = "None";
    [Export] public int typeDamage = 5;
}


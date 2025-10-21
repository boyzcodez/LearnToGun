using System;
using Godot;

[GlobalClass]
public partial class GunData : Resource
{
    [Export] public string GunName { get; set; } = "weapon";
    [Export(PropertyHint.Enum, "LV1_,LV2_,LV3_,LV4_,LV5_")]
    public string LVL { get; set; } = "LV1_";
    [Export] public int currentXP { get; set; } = 0;
    [Export] public int maxXP { get; set; } = 10;
    [Export] public int Damage { get; set; } = 1;
    [Export] public float Knockback { get; set; } = 0f;
    [Export] public float BulletSpeed { get; set; } = 80f;
    [Export] public Vector2 ShootPosition { get; set; }
    [Export] public PackedScene BulletScene { get; set; }
    [Export] public int SpawnAmount { get; set; } = 30;
    [Export] public int CurrentAmmo { get; set; } = 10;
    [Export] public int MaxAmmo { get; set; } = 10;
    [Export] public bool UsesAmmo { get; set; } = true;
    [Export] public float FireRate { get; set; } = 0.2f;
    [Export] public int BulletCount { get; set; } = 1;
    [Export] public float SpreadAngle { get; set; } = 0f;
    [Export] public float RandomFactor { get; set; } = 0f;
    [Export] public Texture2D GunSprite { get; set; }
    [Export] public bool isEnemy { get; set; } = false;
    [Export] public bool rotate { get; set; } = false;
    [Export] public bool LaserSight { get; set; } = false;
    [Export] public GunData NextLevelData { get; set; }

    public void UseBullet()
    {
        if (UsesAmmo) CurrentAmmo -= 1;
    }
    public void ReFillAmmo(int ammoAmount)
    {
        CurrentAmmo += ammoAmount;
        Math.Clamp(CurrentAmmo, 0, MaxAmmo);
    }
}

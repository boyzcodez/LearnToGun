using Godot;

[GlobalClass]
public partial class GunData : Resource
{
    [Export] public string GunName { get; set; } = "weapon";
    [Export] public int Damage { get; set; } = 1;
    [Export] public float Knockback { get; set; } = 0f;
    [Export] public PackedScene BulletScene { get; set; }
    [Export] public int MaxAmmo { get; set; } = 10;
    [Export] public float FireRate { get; set; } = 0.2f;
    [Export] public Texture2D GunSprite { get; set; }
}

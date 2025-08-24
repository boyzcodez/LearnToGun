using Godot;
using System;

[GlobalClass]
public partial class Gun : Node2D
{
    [Export] public GunData gunData { get; set; }
    [Export] public Marker2D spot { get; set; }
    [Export] public GunSprite sprite { get; set; }
    [Export] public bool rotate = false;

    private float _cooldown = 0f;

    public override void _Process(double delta)
    {
        if (_cooldown > 0)
            _cooldown -= (float)delta;
    }

    public void Shoot(Node shooter)
    {
        if (_cooldown > 0) return; // still cooling down
        if (gunData == null) return;
        if (sprite != null) sprite.FireAnimation();

        // Spawn bullet
        var bulletInstance = gunData.BulletScene.Instantiate() as Node2D;
        if (bulletInstance is Bullet bullet)
        {
            bullet.Init(new DamageData(gunData.Damage, gunData.Knockback, shooter), Vector2.Right.Rotated(GlobalRotation));
            bullet.GlobalPosition = spot.GlobalPosition;
            if (rotate) bullet.GlobalRotation = spot.GlobalRotation;
        }
        GetTree().CurrentScene.CallDeferred("add_child", bulletInstance);

        // Reset cooldown
        _cooldown = gunData.FireRate;
    }
}

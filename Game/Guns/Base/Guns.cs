using Godot;
using System;

public partial class Guns : Node2D
{
    [Export] public GunData[] guns { get; set; } = [];
    [Export] public AnimatedGun sprite { get; set; }

    private BulletPool pool;
    private GunData currentGun;

    private int _currentGunIndex = 0;
    private float _cooldown = 0f;
    public bool active = false;
    public string type;
    
    public override void _Ready()
    {
        pool = GetTree().GetFirstNodeInGroup("BulletPool") as BulletPool;

        foreach (var gunData in guns)
        {
            type = gunData.GunName + gunData.LVL + GetInstanceId();
            pool?.PreparePool(type, gunData, gunData.SpawnAmount);
        }

        currentGun = guns[0];
        sprite?.Play(currentGun.GunName + "_" + currentGun.LVL);
        //if (!guns[_currentGunIndex].isEnemy) XpHandler.AddGun(guns[_currentGunIndex].GunName, this);
    }

    public void SwitchGuns(int direction)
    {
        _currentGunIndex = (_currentGunIndex + direction) % guns.Length;
        if (_currentGunIndex < 0) _currentGunIndex = guns.Length - 1;

        EquipGun(_currentGunIndex);
    }
    public void EquipGun(int index)
    {
        currentGun = guns[index];
        EventBus.Ammo(currentGun.CurrentAmmo, currentGun.MaxAmmo);
    }

    public override void _Process(double delta)
    {
        if (_cooldown > 0) _cooldown -= (float)delta;
    }

    public void Shoot()
    {
        if (_cooldown > 0 || currentGun.CurrentAmmo <= 0) return;
        else currentGun.UseBullet();

        if (currentGun == null) return;
        if (sprite != null)
        {
            sprite.FireAnimation();
            PlayAnimation();
        }

        Vector2 baseDirection = Vector2.Right.Rotated(GlobalRotation);

        float spreadRad = Mathf.DegToRad(currentGun.SpreadAngle);
        float angleStep = currentGun.BulletCount > 1 ? spreadRad / (currentGun.BulletCount - 1) : 0f;

        for (int i = 0; i < currentGun.BulletCount; i++)
        {
            float angleOffset = -spreadRad / 2f + i * angleStep;
            //Vector2 direction = baseDirection.Rotated(angleOffset);
            float rotation = GlobalRotation + angleOffset;

            Bullet bullet = pool.GetBullet(type, currentGun);
            bullet.GlobalPosition = GlobalPosition;

            if (currentGun.rotate) bullet.Rotation = rotation;

            bullet.Activate(rotation);
        }

        _cooldown = currentGun.FireRate;
        if (active) EventBus.Ammo(currentGun.CurrentAmmo, currentGun.MaxAmmo);
    }
    private async void PlayAnimation()
    {
        sprite.Play(currentGun.GunName + "_" + currentGun.LVL + "_Shoot");

        await ToSignal(sprite, "animation_finished");

        sprite.Play(currentGun.GunName + "_" + currentGun.LVL);
    }

}

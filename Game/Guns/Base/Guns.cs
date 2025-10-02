using Godot;
using System;

public partial class Guns : Node2D
{
    [Export] public GunData[] guns { get; set; } = [];
    [Export] public bool active = false;
    public AnimatedGun sprite { get; set; }

    private BulletPool pool;
    private GunData currentGun;
    private ShaderMaterial shaderMaterial;

    private int _currentGunIndex = 0;
    private float _cooldown = 0f;
    public string type;
    
    public override void _Ready()
    {
        pool = GetTree().GetFirstNodeInGroup("BulletPool") as BulletPool;
        sprite = GetNode<AnimatedGun>("GunAnimation");
        shaderMaterial = sprite.Material as ShaderMaterial;

        foreach (var gunData in guns)
        {
            type = gunData.GunName + gunData.LVL + GetInstanceId();
            pool?.PreparePool(type, gunData, gunData.SpawnAmount);
        }

        EquipGun(0);
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
        type = currentGun.GunName + currentGun.LVL + GetInstanceId();
        if (active == true) EventBus.Ammo(currentGun.CurrentAmmo, currentGun.MaxAmmo);
        sprite?.Play(currentGun.GunName + "_" + currentGun.LVL);
    }

    public override void _Process(double delta)
    {
        if (_cooldown > 0) _cooldown -= (float)delta;

        if (GlobalRotation > -1.5f && GlobalRotation < 1.5f)
            shaderMaterial.SetShaderParameter("flip_v", false);
        else
            shaderMaterial.SetShaderParameter("flip_v", true);
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
        sprite.Play(currentGun.GunName + "_" + currentGun.LVL + "Shoot");

        await ToSignal(sprite, "animation_finished");

        sprite.Play(currentGun.GunName + "_" + currentGun.LVL);
    }

}

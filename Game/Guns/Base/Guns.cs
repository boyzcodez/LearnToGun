using Godot;
using System;

public partial class Guns : Node2D
{
    [Export] public GunData[] guns { get; set; } = [];
    [Export] public bool active = false;
    [Export] public LaserSight2 laserSight;
    public AnimatedGun sprite { get; set; }

    private BulletPool pool;
    private GunData currentGun;
    private AnimatedSprite2D muzzleFlash;
    private ShaderMaterial shaderMaterial;

    public bool shooting = false;
    private int _currentGunIndex = 0;
    private float _cooldown = 0f;
    public string type;
    
    public override void _Ready()
    {
        pool = GetTree().GetFirstNodeInGroup("BulletPool") as BulletPool;
        sprite = GetNode<AnimatedGun>("GunAnimation");
        muzzleFlash = GetNode<AnimatedSprite2D>("MuzzleFlash");
        shaderMaterial = sprite.Material as ShaderMaterial;

        EventBus.Reset += ReFillGuns;

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

        sprite?.Play(currentGun.GunName + "_" + currentGun.LVL);
        muzzleFlash.Position = currentGun.ShootPosition;
        Position = new Vector2(currentGun.GunX, 0); 

        if (active) EventBus.Ammo(currentGun.CurrentAmmo, currentGun.MaxAmmo);
        if (laserSight != null) laserSight.ToggleLaser(currentGun.LaserSight);
    }

    public override void _Process(double delta)
    {
        if (shooting && _cooldown <= 0) Shoot();
        else if (_cooldown > 0) _cooldown -= (float)delta;

        if (GlobalRotation > -1.5f && GlobalRotation < 1.5f)
        {
            shaderMaterial.SetShaderParameter("flip_v", false);
            muzzleFlash.Position = new Vector2(currentGun.ShootPosition.X, currentGun.ShootPosition.Y);
        }
        else
        {
            shaderMaterial.SetShaderParameter("flip_v", true);
            muzzleFlash.Position = new Vector2(currentGun.ShootPosition.X, -currentGun.ShootPosition.Y);
        } 
    }

    public void Shoot()
    {
        if (currentGun.CurrentAmmo <= 0) return;
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
            float rotation = GlobalRotation + angleOffset + (float)GD.RandRange(-currentGun.RandomFactor, currentGun.RandomFactor);

            Bullet bullet = pool.GetBullet(type, currentGun);
            bullet.GlobalPosition = muzzleFlash.GlobalPosition + new Vector2(10 * NumBet(currentGun.RandomFactor), 10* NumBet(currentGun.RandomFactor));

            //if (currentGun.rotate) bullet.Rotation = rotation;

            bullet.Activate(rotation);
        }

        _cooldown = currentGun.FireRate;
        if (active) EventBus.Ammo(currentGun.CurrentAmmo, currentGun.MaxAmmo);
    }
    private async void PlayAnimation()
    {
        muzzleFlash.Play("default");
        sprite.Play(currentGun.GunName + "_" + currentGun.LVL + "Shoot");

        await ToSignal(sprite, "animation_finished");

        sprite.Play(currentGun.GunName + "_" + currentGun.LVL);
    }
    private float NumBet(double bet)
    {
        return (float)GD.RandRange(-bet, bet);
    }
    private void ReFillGuns()
    {
        foreach (GunData gun in guns)
        {
            gun.ReFillAmmo(999);
        }
        
        if (active) EventBus.Ammo(currentGun.CurrentAmmo, currentGun.MaxAmmo);
    }

    public void ApplyAnimationData(AnimationData AnimationData, string name)
{
    var frames = new SpriteFrames();

    int fullWidth = AnimationData.SpriteSheet.GetWidth();
    int fullHeight = AnimationData.SpriteSheet.GetHeight();

    int frameWidth = fullWidth / AnimationData.HorizontalFrames;
    int frameHeight = fullHeight / AnimationData.VerticalFrames;

    int totalFrames = AnimationData.HorizontalFrames * AnimationData.VerticalFrames;

    frames.AddAnimation(name);
    frames.SetAnimationSpeed(name, AnimationData.FrameRate);

    for (int i = 0; i < totalFrames; i++)
    {
        int x = i % AnimationData.HorizontalFrames;
        int y = i / AnimationData.HorizontalFrames;

        var region = new Rect2I(
            x * frameWidth,
            y * frameHeight,
            frameWidth,
            frameHeight
        );

        var atlas = new AtlasTexture
        {
            Atlas = AnimationData.SpriteSheet,
            Region = region
        };

        frames.AddFrame(name, atlas);
    }

    sprite.SpriteFrames = frames;
    sprite.Play(name);
}

}

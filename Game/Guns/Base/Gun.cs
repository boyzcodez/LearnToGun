using Godot;

[GlobalClass]
public partial class Gun : Node2D
{
    [Export] public GunData gunData { get; set; }
    [Export] public Marker2D spot { get; set; }
    [Export] public AnimatedGun sprite { get; set; }
    [Export] public string type = "Nothing";
    [Export] public bool rotate = false;
    private float _cooldown = 0f;
    private BulletPool pool;

    public override void _Ready()
    {
        pool = GetTree().GetFirstNodeInGroup("BulletPool") as BulletPool;
        type = type + GetInstanceId();
        pool.PreparePool(type, gunData, gunData.SpawnAmount);

        sprite.Play(gunData.LVL + "default");
        XpHandler.AddGun(gunData.GunName, this);
    }

    public override void _Process(double delta)
    {
        if (_cooldown > 0) _cooldown -= (float)delta;
    }

    public void Shoot()
    {
        if (_cooldown > 0 || gunData.CurrentAmmo <= 0) return;
        else gunData.UseBullet();

        if (gunData == null) return;
        if (sprite != null)
        {
            sprite.FireAnimation();
            PlayAnimation();
        }

        Vector2 baseDirection = Vector2.Right.Rotated(GlobalRotation);

        float spreadRad = Mathf.DegToRad(gunData.SpreadAngle);
        float angleStep = gunData.BulletCount > 1 ? spreadRad / (gunData.BulletCount - 1) : 0f;

        for (int i = 0; i < gunData.BulletCount; i++)
        {
            float angleOffset = -spreadRad / 2f + i * angleStep;
            Vector2 direction = baseDirection.Rotated(angleOffset);

            Bullet bullet = pool.GetBullet(type, gunData);
            bullet.GlobalPosition = spot.GlobalPosition;
            bullet.Activate(direction);

            if (rotate) bullet.Rotation = GlobalRotation;
        }

        _cooldown = gunData.FireRate;
        GD.Print(gunData.CurrentAmmo);
    }

    public void AddXP(int xp)
    {
        gunData.currentXP += xp;
        if (gunData.currentXP >= gunData.maxXP)
        {
            LevelUp();
        }
    }
    public void LevelUp()
    {
        if (gunData.NextLevelData == null) return;

        var xp = gunData.currentXP - gunData.maxXP;
        gunData = gunData.NextLevelData;
        gunData.currentXP = xp;

        pool.NewBullets(type, gunData, gunData.SpawnAmount);

        sprite.Play(gunData.LVL + "default");
    }

    private async void PlayAnimation()
    {
        sprite.Play(gunData.LVL + "shoot");

        await ToSignal(sprite, "animation_finished");

        sprite.Play(gunData.LVL + "default");
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("space"))
        {
            LevelUp();
        }
    }

    
    
}

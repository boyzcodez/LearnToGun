using Godot;

[GlobalClass]
public partial class Gun : Node2D
{
    [Export] public GunData gunData { get; set; }
    [Export] public Marker2D spot { get; set; }
    [Export] public GunSprite sprite { get; set; }
    [Export] public string type = "Nothing";
    [Export] public int amount = 50;
    [Export] public bool rotate = false;
    private float _cooldown = 0f;
    private BulletPool pool;

    public override void _Ready()
    {
        pool = GetTree().GetFirstNodeInGroup("BulletPool") as BulletPool;
        type = type + GetInstanceId();
        pool.PreparePool(type, gunData, amount);
    }

    public override void _Process(double delta)
    {
        if (_cooldown > 0) _cooldown -= (float)delta;
            
    }

    public void Shoot()
    {
        if (gunData.CurrentAmmo <= 0) return;
        else gunData.UseBullet();

        if (_cooldown > 0) return;
        if (gunData == null) return;
        if (sprite != null) sprite.FireAnimation();

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
    }
}

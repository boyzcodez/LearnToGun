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
        if (_cooldown > 0)
            _cooldown -= (float)delta;
    }

    public void Shoot()
    {
        if (_cooldown > 0) return;
        if (gunData == null) return;
        if (sprite != null) sprite.FireAnimation();

        

        Bullet bullet = pool.GetBullet(type);
        bullet.GlobalPosition = spot.GlobalPosition;
        bullet.Activate(Vector2.Right.Rotated(GlobalRotation));

        if (rotate) bullet.Rotation = GlobalRotation;

        _cooldown = gunData.FireRate;
    }
}

using Godot;

[GlobalClass]
public partial class SpeedUp : Behavior
{
    public override void Initialize(Bullet bullet)
    {
    }
    public override void Update(Bullet bullet, double delta)
    {
        bullet.GlobalPosition += bullet.Direction * bullet.Speed * (float)delta * (1f + bullet._timer * 3f);
    }
    public override void OnHit(Bullet bullet)
    {
    }
}

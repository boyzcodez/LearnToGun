using Godot;

[GlobalClass]
public partial class StraightMovement : Behavior
{
    public override void Initialize(Bullet bullet)
    {
    }
    public override void Update(Bullet bullet, double delta)
    {
        bullet.GlobalPosition += bullet.direction * bullet.speed * (float)delta;
    }
    public override void OnHit(Bullet bullet)
    {
    }
}

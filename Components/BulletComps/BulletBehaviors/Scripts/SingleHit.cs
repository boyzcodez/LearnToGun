using Godot;

[GlobalClass]
public partial class SingleHit : Behavior
{
    public override void Initialize(Bullet bullet)
    {
    }
    public override void Update(Bullet bullet, double delta)
    {
    }
    public override void OnHit(Bullet bullet)
    {
        bullet.Hurtboxes[0].TakeDamage(bullet.DamageData, bullet.Direction);
        bullet.Deactivate();
    }
}

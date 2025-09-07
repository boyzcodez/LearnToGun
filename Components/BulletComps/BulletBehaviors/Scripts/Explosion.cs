using Godot;

[GlobalClass]
public partial class Explosion : Behavior
{
    public override void Initialize(Bullet bullet)
    {
    }
    public override void Update(Bullet bullet, double delta)
    {
    }
    public override void OnHit(Bullet bullet)
    {
        foreach (var hurtbox in bullet.Hurtboxes)
        {
            Vector2 direction = (hurtbox.GlobalPosition - bullet.GlobalPosition).Normalized();
            hurtbox.TakeDamage(bullet.DamageData, direction * 100f);
        }
        bullet.Deactivate();
    }
}

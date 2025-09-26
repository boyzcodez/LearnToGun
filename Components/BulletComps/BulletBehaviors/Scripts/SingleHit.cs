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
        var overlaps = bullet.GetOverlappingAreas();

        if (overlaps[0] is Hurtbox hurtbox && !hurtbox.immune)
        {
            hurtbox.TakeDamage(bullet.DamageData, bullet.Direction);
            bullet.Deactivate();
        }
    }
}

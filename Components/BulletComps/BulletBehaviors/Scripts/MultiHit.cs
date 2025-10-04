using Godot;

[GlobalClass]
public partial class MultiHit : Behavior
{
    public override void Initialize(Bullet bullet)
    {
    }
    public override void Update(Bullet bullet, double delta)
    {
        if (bullet._timer >= 0.05f)
        {
            foreach (var area in bullet.GetOverlappingAreas())
            {
                if (area is Hurtbox hurtbox && !hurtbox.immune)
                {
                    hurtbox.TakeDamage(bullet.DamageData, bullet.Direction);
                }
            }

            bullet.Deactivate();
        }
    }
    public override void OnHit(Bullet bullet)
    {
    }
}

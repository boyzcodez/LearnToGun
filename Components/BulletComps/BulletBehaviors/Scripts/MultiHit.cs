using Godot;

[GlobalClass]
public partial class MultiHit : Behavior
{
    public override void Initialize(Bullet bullet)
    {
    }
    public override void Update(Bullet bullet, double delta)
    {
        bullet._timer += (float)delta;
        if (bullet._timer >= 0.05f) Trigger(bullet); 
    }
    public override void OnHit(Bullet bullet)
    {
    }
    public void Trigger(Bullet bullet)
    {
        foreach (var hurtbox in bullet.Hurtboxes)
        {
            hurtbox.TakeDamage(bullet.DamageData, bullet.Direction);
        }
        bullet.Deactivate();
    }
}

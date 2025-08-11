using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class Hitbox : Area2D
{
    [Export] public DamageInfo damageInfo;
    private List<Hurtbox> enemiesInRange = new List<Hurtbox>();

    // this function is hooked up through the engine
    public void _on_area_entered(Node body)
    {
        if (body is Hurtbox hurtbox)
        {
            enemiesInRange.Add(hurtbox);
        }
    }

    // this function is hooked up through the engine
    public void _on_area_exited(Node body)
    {
        if (body is Hurtbox hurtbox)
        {
            enemiesInRange.Remove(hurtbox);
        }
    }

    public void Arrange()
    {
        enemiesInRange = enemiesInRange.OrderBy(h => h.GlobalPosition.DistanceTo(GlobalPosition)).ToList();
    }

    public void ApplyDamage()
    {
        if (damageInfo == null)
        {
            GD.PrintErr("DamageInfo is not set for Hitbox in " + Name);
            return;
        }
        Arrange();
        foreach (var hurtbox in enemiesInRange)
        {
            hurtbox.Damage(damageInfo, Vector2.FromAngle(GlobalRotation));
            //await ToSignal(GetTree().CreateTimer(0.05f), "timeout"); // Delay between damage applications
        }
    }
    public void SetGun(BaseGun newGun)
    {
        damageInfo = newGun.damageInfo;
        SetRange(newGun.RangeX, newGun.RangeY);
    }
    public void SetRange(float rangeX, float rangeY)
    {
        CollisionShape2D collision = GetChild<CollisionShape2D>(0);
        var shape = new RectangleShape2D();
        shape.Size = new Vector2(rangeX, rangeY);
        collision.Shape = shape;
        collision.Position = new Vector2(rangeX / 2, 0);
    }
}

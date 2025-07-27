using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class Hitbox : Area2D
{
    
    public DamageInfo damageInfo;
    private List<Hurtbox> enemiesInRange = new List<Hurtbox>();

    private void OnAreaEntered(Node body)
    {
        if (body is Hurtbox hurtbox)
        {
            enemiesInRange.Add(hurtbox);
        }
    }

    private void OnAreaExited(Node body)
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

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("attack"))
        {
            ApplyDamage();
        }
    }

    public override void _Ready()
    {
        Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
        Connect("area_exited", new Callable(this, nameof(OnAreaExited)));
    }
}

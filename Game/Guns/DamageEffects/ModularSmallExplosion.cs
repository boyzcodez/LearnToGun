using Godot;
using System.Collections.Generic;

public partial class ModularSmallExplosion : Area2D
{

    private List<Hurtbox> Hurtboxes { get; set; } = new();

    private DamageData DamageData = new DamageData(1, 200, "Zooka");

    private void _on_area_entered(Node body)
    {
        if (body is Hurtbox hurtbox && !hurtbox.immune)
        {
            Hurtboxes.Add(hurtbox);
        }
    }
    private void Damage()
    {
        foreach (var hurtbox in Hurtboxes)
        {
            Vector2 direction = (hurtbox.GlobalPosition - GlobalPosition).Normalized();
            hurtbox.TakeDamage(DamageData, direction);
        }
        QueueFree();
    }
    private void _on_timer_timeout()
    {
        Damage();
    }

}

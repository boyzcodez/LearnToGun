using Godot;
using System.Collections.Generic;

public partial class Cascade : Node2D
{
    [Export] private DamageInfo cascadeDamageInfo = new()
    {
        damage = 150,
        knockbackForce = 0f,
        damageType = "Cascade",
        typeDamage = 0
    };

    [Export] private int maxBuildUp = 15;

    private Health healthComponent;
    private int buildUpCount;
    private readonly List<Hurtbox> hurtboxesInRange = new();

    public override void _Ready()
    {
        healthComponent = GetParent<Health>();
    }

    public void CascadeBuildUp(int damageAmount)
    {
        buildUpCount += damageAmount;

        if (buildUpCount < maxBuildUp) return;
        
        TriggerCascade();
        buildUpCount = 0;
    }

    private void TriggerCascade()
    {
        foreach (var hurtbox in hurtboxesInRange)
        {
            hurtbox?.Damage(cascadeDamageInfo);
        }
    }

    // Connected through editor signal
    private void _on_area_entered(Node body)
    {
        if (body is Hurtbox hurtbox)
        {
            hurtboxesInRange.Add(hurtbox);
        }
    }

    // Connected through editor signal
    private void _on_area_exited(Node body)
    {
        if (body is Hurtbox hurtbox)
        {
            hurtboxesInRange.Remove(hurtbox);
        }
    }
}

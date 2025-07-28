using Godot;
using System;
using System.Collections.Generic;

public partial class Cascade : Node2D
{
    private Health healthComponent;
    private int buildUpCount = 0;
    private const int maxBuildUp = 15;
    private List<Hurtbox> hurtboxesInRange = new List<Hurtbox>();
    public override void _Ready()
    {
        healthComponent = GetParent<Health>();
        Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
        Connect("area_exited", new Callable(this, nameof(OnAreaExited)));
    }

    public void CascadeBuildUp(int damageAmount)
    {
        buildUpCount += damageAmount;

        if (buildUpCount >= maxBuildUp)
        {
            TriggerCascade();
            buildUpCount = 0;
        }
    }
    private void TriggerCascade()
    {
        foreach (var hurtbox in hurtboxesInRange)
        {
             hurtbox.Damage(new DamageInfo
            {
                damage = 150,
                knockbackForce = 0f,
                damageType = "Cascade",
                typeDamage = 0
             });
        }
    }

    private void OnAreaEntered(Node body)
    {
        if (body is Hurtbox hurtbox)
        {
            hurtboxesInRange.Add(hurtbox);
        }
    }

    private void OnAreaExited(Node body)
    {
        if (body is Hurtbox hurtbox)
        {
            hurtboxesInRange.Remove(hurtbox);
        }
    }
}

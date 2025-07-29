using Godot;
using System;

public partial class Obliterate : Node
{
    private Health healthComponent;
    private int buildUpCount = 0;
    private const int maxBuildUp = 15; // Example threshold for obliteration
    [Export] private DamageInfo obliterateDamageInfo = new DamageInfo
    {
        damage = 1000,
        knockbackForce = 0f,
        damageType = "Obliterate",
        typeDamage = 0
    };

    public override void _Ready()
    {
        healthComponent = GetParent<Health>();
    }

    public void ObliterateBuildUp(int damageAmount)
    {
        buildUpCount += damageAmount;

        if (buildUpCount >= maxBuildUp)
        {
            TriggerObliteration();
            buildUpCount = 0;
        }
    }
    private void TriggerObliteration()
    {
        if (healthComponent != null)
        {
            healthComponent.TakeDamage(obliterateDamageInfo);
        }
    }
}

using Godot;
using System;

public partial class Burn : Node
{
    private Health healthComponent;
    private Timer burnTimer;
    private int buildUpCount = 0;
    private int burnTriggerCount = 0;
    private const float burnTickInterval = 0.3f;
    private const int maxBuildUp = 15; // Example threshold for obliteration

    public override void _Ready()
    {
        healthComponent = GetParent<Health>();
        burnTimer = GetNode<Timer>("Timer");
        burnTimer.Connect("timeout", new Callable(this, nameof(OnTimerTimeout)));
    }

    public void BurnBuildUp(int damageAmount)
    {
        buildUpCount += damageAmount;
        if (buildUpCount >= maxBuildUp)
        {
            buildUpCount = 0;

            if (burnTriggerCount == 0)
            {
                burnTriggerCount += 5;
                burnTimer.Start(burnTickInterval);
            }
            else
            {
                burnTriggerCount += 5;  
            }
            
        }
    }
    private void OnTimerTimeout()
    {
        if (healthComponent != null)
        {
            healthComponent.TakeDamage(new DamageInfo
            {
                damage = 5,
                knockbackForce = 0f,
                damageType = "Burn",
                typeDamage = 0
            });
        }

        burnTriggerCount--;

        if (burnTriggerCount > 0)
        {
            burnTimer.Start(burnTickInterval);
        }
    }
}

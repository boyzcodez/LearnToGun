using Godot;
using System;

public partial class Obliterate : Node
{
    private Health healthComponent;

    public override void _Ready()
    {
        healthComponent = GetParent<Health>();
    }

    public void TriggerObliteration()
    {
        if (healthComponent != null)
        {
            return;
            //healthComponent.TakeDamage(1000); 
        }
    }
}

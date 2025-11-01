using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class Ability : Node2D
{
    [Export] float rotateAmount = 0f;
    [Export] Marker2D lookat;
    private List<Guns> guns = new();

    public override void _Ready()
    {
        GetOwner<Entity>().Connect(Entity.SignalName.Activation, new Callable(this, nameof(Activate)));
        GetOwner<Entity>().Connect(Entity.SignalName.Deactivation, new Callable(this, nameof(Deactivate)));

        foreach (Guns child in GetChildren())
        {
            guns.Add(child);
            child.SetProcess(false);
        }
    }


    public void TriggerAbility()
    {
        foreach (var gun in guns)
        {
            gun.Shoot();
        }
    }

    private void Activate()
    {
        foreach (var gun in guns)
        {
            gun.SetProcess(true);
        }
    }
    private void Deactivate()
    {
        foreach (var gun in guns)
        {
            gun.SetProcess(false);
        }
    }
}

using Godot;
using System;

public partial class EnemyHurtbox : Hurtbox
{
    public override void OnInit()
    {
        GetParent<Entity>().Connect(Entity.SignalName.Activation, new Callable(this, nameof(Activate)));
        GetParent<Entity>().Connect(Entity.SignalName.Deactivation, new Callable(this, nameof(Deactivate)));
    }

    public void Activate()
    {
        SetDeferred("monitoring", true);
        SetDeferred("monitorable", true);
    }
    public void Deactivate()
    {
        SetDeferred("monitoring", false);
        SetDeferred("monitorable", false);
    }

}

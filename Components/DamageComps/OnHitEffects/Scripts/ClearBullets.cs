using Godot;
using System;

[GlobalClass]
public partial class ClearBullets : BaseHitEffect
{
    public override void Trigger()
    {
        EventBus.TriggerClearBullets();
    }

}

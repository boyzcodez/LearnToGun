using Godot;

[GlobalClass]
public partial class Shake : BaseHitEffect
{
    public override void Trigger()
    {
        EventBus.TriggerScreenShake(0.4f);
    }
}

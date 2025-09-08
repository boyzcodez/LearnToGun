using Godot;

[GlobalClass]
public partial class Stun : BaseHitEffect
{
    public override void Trigger()
    {
        EventBus.TriggerClearBullets();
    }
}

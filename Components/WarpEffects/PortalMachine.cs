using Godot;

public partial class PortalMachine : Node2D
{
    private Interactable interactable;
    public override void _Ready()
    {
        interactable = GetNode<Interactable>("Interactable");
        interactable.interactFunction = new Callable(this, nameof(Trigger));
    }
    private void Trigger()
    {
        EventBus.TriggerLock();
        EventBus.TriggerScreenShake(0.6f);

        var portal = FindChild("Portal") as Portal2;
        portal.GrowPortal();
    }
}

using Godot;

public partial class PortalMachine : Node2D
{
    [Export] private bool active = true;

    private bool triggered = false;
    private CanvasGroup sprite;
    private Area2D area;
    public override void _Ready()
    {
        area = GetNode<Area2D>("Area2D");
        sprite = GetNode<CanvasGroup>("BasePortal");
        area.AreaEntered += Trigger;

        EventBus.EndWave += Activate;

        if (active) Activate();
    }


    private void Trigger(Node body)
    {
        if (!active)
            return;

        triggered = true;
        //EventBus.TriggerLock();
        EventBus.TriggerScreenShake(0.6f);

        EventBus.TriggerMapSwitch();

        // var portal = FindChild("Portal") as Portal2;
        // portal.GrowPortal();
    }
    private void Activate()
    {
        sprite.Show();
        active = true;
    }
    public override void _ExitTree()
    {
        EventBus.EndWave -= Activate;
    }

}

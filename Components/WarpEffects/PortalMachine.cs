using Godot;

public partial class PortalMachine : Node2D
{
    [Export] public WalkerHead Head;
    [Export] private bool active = false;

    private bool triggered = false;
    private CanvasGroup sprite;
    private Area2D area;
    public override void _Ready()
    {
        area = GetNode<Area2D>("Area2D");
        sprite = GetNode<CanvasGroup>("BasePortal");
        area.AreaEntered += Trigger;

        EventBus.EndRound += Activate;

        if (active) Activate();
        else
        {
            area.SetDeferred("monitoring", false);
            area.SetDeferred("monitorable", false);
        }

        Head.Explosion(1, GlobalPosition);
    }


    private void Trigger(Node body)
    {
        if (!active) return;

        Deactivate();
        
        EventBus.TriggerScreenShake(0.6f);
        EventBus.TriggerMapSwitch();
    }
    public void Activate()
    {
        sprite.Show();
        active = true;

        area.SetDeferred("monitoring", true);
        area.SetDeferred("monitorable", true);

        Head.Explosion(1, GlobalPosition);
    }
    public void Deactivate()
    {
        sprite.Hide();
        active = false;

        area.SetDeferred("monitoring", false);
        area.SetDeferred("monitorable", false);
    }
    public override void _ExitTree()
    {
        EventBus.EndRound -= Activate;
    }

}

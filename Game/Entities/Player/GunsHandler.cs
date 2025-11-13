using Godot;
using System;

public partial class GunsHandler : Node
{
    [Export] public Guns guns;
    private Timer timer;
    private bool canSwitch = true;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        timer.Timeout += CanSwitch;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.WheelUp && mouseEvent.Pressed && canSwitch)
            {
                canSwitch = false;
                guns?.SwitchGuns(1);
                timer.Start();
            }
            else if (mouseEvent.ButtonIndex == MouseButton.WheelDown && mouseEvent.Pressed && canSwitch)
            {
                canSwitch = false;
                guns?.SwitchGuns(-1);
                timer.Start();
            }
        }
        
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("attack") & guns != null)
        {
            guns.shooting = true;
        }
        if (@event.IsActionReleased("attack") & guns != null)
        {
            guns.shooting = false;
        }
    }
    
    public void AddNewGun()
    {
        GD.Print("here will added a new gun?");
    }


    public void CanSwitch()
    {
        canSwitch = true;
    }
}

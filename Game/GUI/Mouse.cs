using Godot;
using System;

public partial class Mouse : Node2D
{
    public override void _Ready()
    {
        Input.SetMouseMode(Input.MouseModeEnum.Hidden);
    }
    public override void _Process(double delta)
    {
        // Get the global mouse position (in screen coordinates)
        Vector2 mousePos = GetViewport().GetMousePosition();
        
        // Since Node2D is under a CanvasLayer, use the position directly
        Position = mousePos;
    }
}

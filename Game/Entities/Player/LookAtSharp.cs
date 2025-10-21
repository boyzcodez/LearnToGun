using Godot;
using System;

public partial class LookAtSharp : Marker2D
{
    public override void _Process(double delta)
    {
        Vector2 mousePos = GetGlobalMousePosition();
        Vector2 toMouse = mousePos - GlobalPosition;
        if (toMouse == Vector2.Zero)
            return;
        Rotation = toMouse.Angle();
    }
}

using Godot;
using System;

public partial class LookAt : Marker2D
{
    public override void _Process(double delta)
    {
        Vector2 toMouse = GetGlobalMousePosition() - GlobalPosition;
        Rotation = toMouse.Angle();
    }
}

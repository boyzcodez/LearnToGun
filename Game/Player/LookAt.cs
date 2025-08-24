using Godot;
using System;

public partial class LookAt : Marker2D
{
    public override void _Process(double delta)
    {
        Vector2 toMouse = GetGlobalMousePosition() - GlobalPosition;
        Rotation = toMouse.Angle();

        if (Rotation > -1.5f && Rotation < 1.5f)
            Scale = new Vector2(1, 1);
        else
            Scale = new Vector2(1, -1);
    }
}

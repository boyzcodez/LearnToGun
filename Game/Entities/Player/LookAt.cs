using Godot;
using System;

public partial class LookAt : Marker2D
{
    [Export] public float SnapDegrees = 5f;
    [Export] private Node2D gunSpot;
    [Export] private float BaseOffset = 14f;
    [Export] private float MinOffset = 7f;
    public override void _Process(double delta)
    {
        Vector2 toMouse = GetGlobalMousePosition() - GlobalPosition;

        float angle = toMouse.Angle();

        // Convert to degrees
        float angleDegrees = Mathf.RadToDeg(angle);

        // Snap to nearest increment (e.g., 10°)
        float snappedDegrees = Mathf.Round(angleDegrees / SnapDegrees) * SnapDegrees;

        // Convert back to radians
        float snappedAngle = Mathf.DegToRad(snappedDegrees);


        Rotation = snappedAngle;
        //toMouse.Angle();


        if (gunSpot != null)
        {
            float factor = Mathf.Abs(Mathf.Cos(snappedAngle));
            float offsetX = Mathf.Lerp(MinOffset, BaseOffset, factor);

            Vector2 spritePos = gunSpot.Position;
            spritePos.X = offsetX;
            gunSpot.Position = spritePos;
        }
    }
}

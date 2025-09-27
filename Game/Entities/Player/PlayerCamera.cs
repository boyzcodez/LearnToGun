using Godot;
using System;

public partial class PlayerCamera : Camera2D
{
    [Export] private float decay = 0.8f;
    [Export] private Vector2 maxOffset = new Vector2(100, 75);
    [Export] private float maxRoll = 0.1f;
    [Export] private Player followNode;

    private float trauma = 0.0f;
    private int traumaPower = 2;

    public override void _Ready()
    {
        EventBus.ScreenShake += AddTrauma;
        GD.Randomize();
    }
    public override void _Process(double delta)
    {
        if (followNode != null)
            GlobalPosition = followNode.GlobalPosition;

        if (trauma > 0f)
        {
            trauma = Math.Max(trauma - decay * (float)delta, 0f);
            Shake();
        }
    }

    private void AddTrauma(float amount)
    {
        trauma = Math.Min(trauma + amount, 1.0f);
    }
    private void Shake()
    {
        var amount = Math.Pow(trauma, traumaPower);
        Rotation = maxRoll * (float)amount * (float)GD.RandRange(-1, 1);
        Offset = new Vector2(maxOffset.X * (float)amount * (float)GD.RandRange(-1f, 1f), maxOffset.Y * (float)amount * (float)GD.RandRange(-1f, 1f));

    }

}

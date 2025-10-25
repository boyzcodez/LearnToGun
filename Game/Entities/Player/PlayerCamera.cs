using Godot;
using System;

public partial class PlayerCamera : Camera2D
{
    [Export] private Player target;
    [Export] public int PixelsPerUnit = 32;   // 1 world unit = 16 pixels
    [Export] public float SnapSmoothness = 40f;
    [Export] private float decay = 0.8f;
    [Export] private Vector2 maxOffset = new Vector2(100, 75);
    [Export] private float maxRoll = 0.1f;

    private float trauma = 0.0f;
    private int traumaPower = 2;

    public override void _Ready()
    {
        EventBus.ScreenShake += AddTrauma;
        GD.Randomize();
    }
    public override void _Process(double delta)
    {
        if (target != null)
        {
            //     // The player’s true position (no smoothing)
            // Vector2 playerPos = target.GlobalPosition;

            // // Compute the *snapped-to-grid* version
            // float pixelSize = 1f / PixelsPerUnit;
            // Vector2 snapped = new Vector2(
            //     Mathf.Round(playerPos.X / pixelSize) * pixelSize,
            //     Mathf.Round(playerPos.Y / pixelSize) * pixelSize
            // );

            // // Smoothly approach the snapped position
            // GlobalPosition = GlobalPosition.Lerp(snapped, (float)(SnapSmoothness * delta));
            GlobalPosition = target.GlobalPosition.Round();
        }

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

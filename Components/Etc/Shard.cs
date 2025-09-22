using Godot;
using System;

public partial class Shard : Sprite2D
{
    [Export] public float InitialForce = 100f; // Base upward strength
    [Export] public float Gravity = 400f;      // Pull back down
    [Export] public float Damping = 0.5f;      // Reduces bounce height
    [Export] public int MaxBounces = 2;        // Number of bounces
    [Export] public float GroundRandomRange = 12f; // Range for random Y landing offset

    private Vector2 _velocity;
    private int _bouncesLeft;
    private float _groundY;
    private bool _done;

    public override void _Ready()
    {
        _bouncesLeft = MaxBounces;

        // Randomize final ground position for isometric depth
        _groundY = GlobalPosition.Y + (float)GD.RandRange(-GroundRandomRange, GroundRandomRange);

        // Initial upward velocity (randomized a bit)
        float yKick = -InitialForce * (0.8f + (float)GD.RandRange(0.0, 0.3));

        // Sideways nudge (random direction/strength)
        float xKick = (float)GD.RandRange(-InitialForce * 0.5f, InitialForce * 0.5f);

        _velocity = new Vector2(xKick, yKick);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_done)
            return;

        // Apply gravity
        _velocity.Y += Gravity * (float)delta;

        // Move
        GlobalPosition += _velocity * (float)delta;

        // Check ground hit
        if (GlobalPosition.Y >= _groundY)
        {
            if (_bouncesLeft > 0)
            {
                _bouncesLeft--;

                // Bounce up, reduced
                _velocity.Y = -Mathf.Abs(_velocity.Y) * Damping;

                // Clamp to ground
                GlobalPosition = new Vector2(GlobalPosition.X, _groundY);
            }
            else
            {
                // Stop moving
                GlobalPosition = new Vector2(GlobalPosition.X, _groundY);
                _velocity = Vector2.Zero;
                _done = true;
            }
        }
    }
}

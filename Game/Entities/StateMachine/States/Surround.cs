using Godot;
using System;

[GlobalClass]
public partial class Surround : State
{
    private float rngNum;
    private Vector2 velocity = Vector2.Zero;
    [Export] private float moveSpeed = 50f;
    [Export] private float howClose = 100f;
    [Export] private float prevRange = 900f;
    [Export] private float nextRange = 100f;

    public override void Enter()
    {
        RandomNumberGenerator rng = new();
        rng.Randomize();
        rngNum = rng.Randf();

    }
    public override void PhysicsProcess(double delta)
    {
        Move(GetCirclePosition(rngNum), (float)delta);

        Vector2 direction = player.GlobalPosition - parent.GlobalPosition;

        if (direction.Length() < nextRange && NextState != "Nothing")
        {
            EmitSignal("Transitioned", this, NextState);
        }
        else if (direction.Length() > prevRange && PrevState != "Nothing")
        {
            EmitSignal("Transitioned", this, PrevState);
        }
    }
    public void Move(Vector2 target,float delta)
    {
        Vector2 direction = (target - parent.GlobalPosition).Normalized();
        Vector2 desiredVelocity = direction * moveSpeed;
        Vector2 steering = (desiredVelocity - velocity) * delta * 2.5f;
        velocity += steering;
        parent.direction = velocity;
    }
    public Vector2 GetCirclePosition(float random)
    {
        Vector2 center = player.GlobalPosition;
        float angle = random * Mathf.Pi * 2;
        float x = center.X + Mathf.Cos(angle) * howClose;
        float y = center.Y + Mathf.Sin(angle) * howClose;
        return new Vector2(x, y);
    }
}

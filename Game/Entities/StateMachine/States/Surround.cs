using Godot;
using System;

[GlobalClass]
public partial class Surround : State
{
    private Vector2 velocity = Vector2.Zero;
    private Vector2 targetPosition;
    
    [Export] private float moveSpeed = 50f;
    [Export] private float circleRadius = 100f;
    [Export] private float arrivalThreshold = 10f;
    [Export] private float prevRange = 900f;
    [Export] private float nextRange = 100f;

    public override void Enter()
    {
        targetPosition = GetRandomPositionAroundPlayer();
    }

    public override void PhysicsProcess(double delta)
    {
        Vector2 toTarget = targetPosition - parent.GlobalPosition;
        Vector2 toPlayer = player.GlobalPosition - parent.GlobalPosition;

        ApplyMovement(toTarget, (float)delta);
        CheckStateTransitions(toTarget, toPlayer);
    }

    private void ApplyMovement(Vector2 toTarget, float delta)
    {
        Vector2 direction = toTarget.Normalized();
        Vector2 desiredVelocity = direction * moveSpeed;
        velocity += (desiredVelocity - velocity) * delta * 2.5f;
        parent.direction = velocity;
    }

    private void CheckStateTransitions(Vector2 toTarget, Vector2 toPlayer)
    {
        if (toTarget.Length() < arrivalThreshold)
        {
            EmitSignal(SignalName.Transitioned, this, NextState);
        }
        else if (toPlayer.Length() > prevRange && PrevState != "Nothing")
        {
            EmitSignal(SignalName.Transitioned, this, PrevState);
        }
    }

    private Vector2 GetRandomPositionAroundPlayer()
    {
        var rng = new RandomNumberGenerator();
        rng.Randomize();
        float angle = rng.Randf() * Mathf.Tau; // Tau = 2π
        
        return player.GlobalPosition + Vector2.Right.Rotated(angle) * circleRadius;
    }
}

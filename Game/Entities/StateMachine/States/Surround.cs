using Godot;
using System;

[GlobalClass]
public partial class Surround : State
{
    private Vector2 velocity = Vector2.Zero;
    private Vector2 targetPosition;
    
    [Export] private float moveSpeed = 2000f;
    [Export] private float circleRadius = 100f;
    [Export] private float arrivalThreshold = 10f;
    [Export] private float prevRange = 900f;
    [Export] private float nextRange = 100f;

    // public override void Enter()
    // {
    //     targetPosition = GetRandomPositionAroundPlayer();
    //     //parent.Goal = targetPosition;
    //     parent.speed = moveSpeed;
    // }

    // public override void PhysicsProcess(double delta)
    // {
    //     Vector2 toTarget = targetPosition - parent.GlobalPosition;
    //     Vector2 toPlayer = player.GlobalPosition - parent.GlobalPosition;

    //     parent.Goal = player.GlobalPosition + targetPosition;

    //     //CheckStateTransitions(toTarget, toPlayer);

    //     if ((player.GlobalPosition + targetPosition - parent.GlobalPosition).Length() < arrivalThreshold)
    //     {
    //         targetPosition = GetRandomPositionAroundPlayer();
    //     }
    // }

    // // private void ApplyMovement(Vector2 toTarget, float delta)
    // // {
    // //     Vector2 direction = toTarget.Normalized();
    // //     Vector2 desiredVelocity = direction * moveSpeed;
    // //     velocity += (desiredVelocity - velocity) * delta * 2.5f;
    // //     parent.Goal = velocity;
    // // }

    // private void CheckStateTransitions(Vector2 toTarget, Vector2 toPlayer)
    // {
    //     if (toTarget.Length() < arrivalThreshold)
    //     {
    //         parent.Goal = GetRandomPositionAroundPlayer();
    //     }
    //     else if (toPlayer.Length() > prevRange && PrevState != "Nothing")
    //     {
    //         EmitSignal(SignalName.Transitioned, this, PrevState);
    //     }
    // }

    // private Vector2 GetRandomPositionAroundPlayer()
    // {
    //     var rng = new RandomNumberGenerator();
    //     rng.Randomize();
    //     float angle = rng.Randf() * Mathf.Tau; // Tau = 2π
        
    //     return Vector2.Right.Rotated(angle) * circleRadius;
    // }
}

using Godot;
using System;

[GlobalClass]
public partial class Idle : State
{
    private Vector2 moveDirection;
    private float wanderTime;
    [Export] private float speed = 25f;
    [Export] private float dedectRange = 800f;


    private void RandomizeWander()
    {
        moveDirection = new Vector2(GD.Randf() * 2 - 1, GD.Randf() * 2 - 1).Normalized();
        wanderTime = GD.Randf() * 2 + 1; // Random time between 1 and 3 seconds
    }

    public override void Enter()
    {
        RandomizeWander();
    }
    public override void Update(double delta)
    {
        wanderTime -= (float)delta;
        if (wanderTime <= 0)
        {
            RandomizeWander();
        }
    }
    public override void PhysicsProcess(double delta)
    {
        parent.Velocity = moveDirection * speed;

        var direction = player.GlobalPosition - parent.GlobalPosition;
        if (direction.Length() < dedectRange && NextState != "Nothing")
        {
            EmitSignal("Transitioned", this, NextState);
        }
    }
}
